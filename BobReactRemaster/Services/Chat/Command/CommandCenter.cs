using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.EventBus.MessageDataTypes;
using BobReactRemaster.Services.Chat.Command.Commands;
using BobReactRemaster.Services.Chat.Command.Interfaces;
using BobReactRemaster.Services.Chat.Commands.Base;
using BobReactRemaster.Services.Chat.Commands.Interfaces;
using BobReactRemaster.Services.Scheduler;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BobReactRemaster.Services.Chat.Commands
{
    //Punny
    public class CommandCenter : BackgroundService
    {
        //TODO: Initialize Commands from Database on Startup
        //Differentiate between Manual Commands and Interval commands
        //Change ExecuteAsync or Add Task to Scheduler for IntervalCommands
        //TODO: Add Other Commands Statically like TwitchAPI Commands and such
        //TODO add "Event" to MessageBus when Commands are updated to renew
        //
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMessageBus bus;
        private SchedulerService scheduler;
        public CommandCenter(IServiceScopeFactory scopeFactory, IMessageBus bus)
        {
            _scopeFactory = scopeFactory;
            this.bus = bus;
            bus.RegisterToEvent<RefreshManualRelayCommands>(RefreshManualCommands);
            bus.RegisterToEvent<RefreshIntervalRelayCommands>(RefreshIntervalCommands);
            RefreshManualCommands();
        }

        private void RefreshIntervalCommands(RefreshIntervalRelayCommands obj = null)
        {
            try
            {
                foreach (IntervalRelayCommand disposableCommand in IntervalCommands)
                {
                    disposableCommand.Dispose();
                }
            }
            catch (InvalidCastException e)
            {
            }
            
            IntervalCommands = new List<IIntervalCommand>();
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            int index = 0;
            foreach (var IntervalCommand in context.IntervalCommands.Include(x => x.LiveStream).Where(x => x.LiveStream.State == StreamState.Running))
            {
                index++;
                var command = new IntervalRelayCommand(IntervalCommand, index);
                scheduler.AddTask(command.RelayTask);
                IntervalCommands.Add(command);
            }
            //TODO add IntervalCommand (rename to prevent confusion with Data.Models.Commands.IntervalCommand) implementing ICommand
        }

        private void RefreshManualCommands(RefreshManualRelayCommands obj = null)
        {
            ManualCommands = new List<ICommand>();
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            foreach (var ManualCommand in context.ManualCommands.Include(x => x.LiveStream).Where(x => x.LiveStream.State == StreamState.Running))
            {
                ManualCommands.Add(new ManualRelayCommand(ManualCommand.Trigger,ManualCommand.Response,bus,ManualCommand.LiveStream));
            }
        }

        private List<ICommand> ManualCommands = new List<ICommand>();
        private List<IIntervalCommand> IntervalCommands = new List<IIntervalCommand>();
        private List<ICommand> StaticCommands = new List<ICommand>();

        public void HandleCommandMessage(CommandMessage msg)
        {
            foreach (var command in ManualCommands)
            {
                if (command.IsTriggerable(msg))
                {
                    command.TriggerCommand();
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
