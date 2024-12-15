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
        public CommandCenter(IServiceScopeFactory scopeFactory, IMessageBus bus)
        {
            _scopeFactory = scopeFactory;
            this.bus = bus;
            bus.RegisterToEvent<RefreshManualRelayCommands>(RefreshManualCommands);
            bus.RegisterToEvent<RefreshIntervalRelayCommands>(RefreshIntervalCommands);
            bus.RegisterToEvent<RelayStartedMessageData>(InitRelayStartCommands);
            bus.RegisterToEvent<RelayStoppedMessageData>(RemoveRelayCommands);
            bus.RegisterToEvent<QuoteCommandAdded>(AddQuoteCommand);
            RefreshManualCommands();
        }

        private void AddQuoteCommand(QuoteCommandAdded obj)
        {
            QuoteCommands.FirstOrDefault(x => x.IsFromLiveStream(obj.Stream))?.AddQuoteCommand(obj.Quote);
        }

        private void RemoveRelayCommands(RelayStoppedMessageData obj)
        {
            var Storage = StreamTasksStorage.FirstOrDefault(x => x.StreamName.ToLower() == obj.Stream.StreamName.ToLower());
            if (Storage != null)
            {
                Storage.StopTasks();
                StreamTasksStorage.Remove(Storage);
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
            }

            if (obj.Stream.HasStaticCommands())
            {
                var Commands = obj.Stream.GetStaticCommands(bus).ToList();
                //Commands that Require _scopeFactory as they need DataBase Access
                Commands.Add(new AddQuoteCommand(bus,obj.Stream,_scopeFactory));
                StaticStreamCommands.AddRange(Commands);
            }
            
            if (obj.Stream.HasQuotes())
            {
                try
                {
                    var Command = new QuoteCommand(bus, obj.Stream);
                    QuoteCommands.Add(Command);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
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
                    }
                    else
                    {
                        scheduler.AddTask(Task);
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
                    ManualCommands.Add(new ManualRelayCommand(ManualCommand.Trigger, ManualCommand.Response, bus,
                        ManualCommand.LiveStream));
            }
        }

        

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task HandleCommandMessageAsync(CommandMessage msg)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            HandleCommandList(ManualCommands,msg);
            HandleCommandList(StaticStreamCommands,msg);
            HandleCommandList(QuoteCommands,msg);
        }

        private void HandleCommandList(IEnumerable<ICommand> List,CommandMessage msg)
        {
            foreach (var command in List)
            {
                if (command.IsTriggerable(msg))
                {
                    command.TriggerCommand(msg);
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (scheduler == null)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    scheduler = (SchedulerService)scope.ServiceProvider.GetServices<IHostedService>()
                    .FirstOrDefault(x => x.GetType() == typeof(SchedulerService));
                    RefreshIntervalCommands();
                }
                await Task.Delay(5000, stoppingToken);
            }

            return;
        }
    }
}
