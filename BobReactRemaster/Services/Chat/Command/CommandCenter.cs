using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.EventBus.MessageDataTypes;
using BobReactRemaster.EventBus.MessageDataTypes.Relay;
using BobReactRemaster.EventBus.MessageDataTypes.Relay.Twitch;
using BobReactRemaster.Services.Chat.Command;
using BobReactRemaster.Services.Chat.Command.Commands;
using BobReactRemaster.Services.Chat.Commands.Base;
using BobReactRemaster.Services.Chat.Commands.Interfaces;
using BobReactRemaster.Services.Scheduler;
using BobReactRemaster.Services.Scheduler.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BobReactRemaster.Services.Chat.Commands
{
    //Punny
    public class CommandCenter : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMessageBus bus;
        private SchedulerService scheduler;
        private List<StreamTasksStorage> StreamTasksStorage = new List<StreamTasksStorage>();
        private List<ICommand> ManualCommands = new List<ICommand>();
        private List<ICommand> StaticStreamCommands = new List<ICommand>();
        private List<QuoteCommand> QuoteCommands = new List<QuoteCommand>();

        public CommandCenter(
            IServiceScopeFactory scopeFactory,
            IMessageBus bus,
            ILogger<CommandCenter> logger
        ) : base(logger)
        {
            _scopeFactory = scopeFactory;
            this.bus = bus;
            bus.RegisterToEvent<RefreshManualRelayCommands>(RefreshManualCommands);
            bus.RegisterToEvent<RefreshIntervalRelayCommands>(RefreshIntervalCommands);
            bus.RegisterToEvent<RelayStartedMessageData>(InitRelayStartCommands);
            bus.RegisterToEvent<RelayStoppedMessageData>(RemoveRelayCommands);
            bus.RegisterToEvent<QuoteCommandAdded>(AddQuoteCommand);
            RefreshManualCommands();
            _logger.LogInformation("CommandCenter initialized and event handlers registered.");
        }

        private void AddQuoteCommand(QuoteCommandAdded obj)
        {
            try
            {
                QuoteCommands.FirstOrDefault(x => x.IsFromLiveStream(obj.Stream))?.AddQuoteCommand(obj.Quote);
                _logger.LogInformation("Added quote command for stream: {StreamName}", obj.Stream.StreamName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding quote command for stream: {StreamName}", obj.Stream.StreamName);
            }
        }

        private void RemoveRelayCommands(RelayStoppedMessageData obj)
        {
            var Storage = StreamTasksStorage.FirstOrDefault(x => x.StreamName.ToLower() == obj.Stream.StreamName.ToLower());
            if (Storage != null)
            {
                Storage.StopTasks();
                StreamTasksStorage.Remove(Storage);
                _logger.LogInformation("Removed relay commands for stream: {StreamName}", obj.Stream.StreamName);
            }
        }

        private void InitRelayStartCommands(RelayStartedMessageData obj)
        {
            RefreshIntervalCommands();
            if (obj.Stream.UpTimeInterval > 0)
            {
                var Storage = StreamTasksStorage.FirstOrDefault(x => x.StreamName.ToLower() == obj.Stream.StreamName.ToLower());
                if (Storage == null)
                {
                    Storage = new StreamTasksStorage(obj.Stream.StreamName);
                }
                var UpTimeTask = obj.Stream.GetUpTimeTask();
                Storage.AddTask(UpTimeTask);
                scheduler.AddTask(UpTimeTask);
                _logger.LogInformation("Added UpTimeTask for stream: {StreamName}", obj.Stream.StreamName);
            }

            if (obj.Stream.HasStaticCommands())
            {
                var Commands = obj.Stream.GetStaticCommands(bus).ToList();
                Commands.Add(new AddQuoteCommand(bus, obj.Stream, _scopeFactory));
                StaticStreamCommands.AddRange(Commands);
                _logger.LogInformation("Added static commands for stream: {StreamName}", obj.Stream.StreamName);
            }

            if (obj.Stream.HasQuotes())
            {
                try
                {
                    var Command = new QuoteCommand(bus, obj.Stream);
                    QuoteCommands.Add(Command);
                    _logger.LogInformation("Added quote command for stream: {StreamName}", obj.Stream.StreamName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding quote command for stream: {StreamName}", obj.Stream.StreamName);
                }
            }
        }

        private void RefreshIntervalCommands(RefreshIntervalRelayCommands? obj = null)
        {
            StreamTasksStorage.ForEach(x => x.ClearRefreshableTasks());
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            int index = 0;
            foreach (var IntervalCommand in context.IntervalCommands.Include(x => x.LiveStream).Where(x => x.LiveStream != null && x.LiveStream.State == StreamState.Running))
            {
                if (IntervalCommand.LiveStream != null)
                {
                    index++;
                    var Task = new IntervalCommandRelayTask(index, IntervalCommand.AutoInverval, IntervalCommand.LiveStream, IntervalCommand.Response);
                    var thisStorage = StreamTasksStorage.FirstOrDefault(x => x.StreamName == IntervalCommand.LiveStream.StreamName);
                    if (thisStorage == null)
                    {
                        thisStorage = new StreamTasksStorage(IntervalCommand.LiveStream.StreamName);
                    }

                    thisStorage.AddTask(Task);
                    if (scheduler.ContainsTask(Task))
                    {
                        scheduler.UpdateTask(Task);
                        _logger.LogInformation("Updated interval command task for stream: {StreamName}", IntervalCommand.LiveStream.StreamName);
                    }
                    else
                    {
                        scheduler.AddTask(Task);
                        _logger.LogInformation("Added interval command task for stream: {StreamName}", IntervalCommand.LiveStream.StreamName);
                    }
                }
            }
        }

        private void RefreshManualCommands(RefreshManualRelayCommands? obj = null)
        {
            ManualCommands = new List<ICommand>();
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            foreach (var ManualCommand in context.ManualCommands.Include(x => x.LiveStream))
            {
                if (ManualCommand is { LiveStream: { } })
                {
                    ManualCommands.Add(new ManualRelayCommand(ManualCommand.Trigger, ManualCommand.Response, bus, ManualCommand.LiveStream));
                    _logger.LogInformation("Added manual command: {Trigger} for stream: {StreamName}", ManualCommand.Trigger, ManualCommand.LiveStream.StreamName);
                }
            }
        }

        public async Task HandleCommandMessageAsync(CommandMessage msg)
        {
            try
            {
                HandleCommandList(ManualCommands, msg);
                HandleCommandList(StaticStreamCommands, msg);
                HandleCommandList(QuoteCommands, msg);
                _logger.LogInformation("Handled command message: {Message}", msg.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling command message: {Message}", msg.Message);
            }
        }

        private void HandleCommandList(IEnumerable<ICommand> List, CommandMessage msg)
        {
            foreach (var command in List)
            {
                try
                {
                    if (command.IsTriggerable(msg))
                    {
                        command.TriggerCommand(msg);
                        _logger.LogInformation("Triggered command for message: {Message}", msg.Message);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error triggering command for message: {Message}", msg.Message);
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CommandCenter background service started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                if (scheduler == null)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    scheduler = (SchedulerService)scope.ServiceProvider.GetServices<IHostedService>()
                        .FirstOrDefault(x => x.GetType() == typeof(SchedulerService));
                    RefreshIntervalCommands();
                    _logger.LogInformation("SchedulerService resolved and interval commands refreshed.");
                }
                await Task.Delay(5000, stoppingToken);
            }
            _logger.LogInformation("CommandCenter background service stopping.");
        }
    }
}
