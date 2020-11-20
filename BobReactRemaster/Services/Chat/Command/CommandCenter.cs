using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.EventBus.MessageDataTypes;
using BobReactRemaster.Services.Chat.Command.Commands;
using BobReactRemaster.Services.Chat.Commands.Base;
using BobReactRemaster.Services.Chat.Commands.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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
            throw new NotImplementedException();
        }

        private void RefreshManualCommands(RefreshManualRelayCommands obj = null)
        {
            ManualCommands = new List<ICommand>();
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            foreach (var ManualCommand in context.ManualCommands)
            {
                ManualCommands.Add(new ManualCommand(ManualCommand.Trigger,ManualCommand.Response,bus));
            }
        }

        private List<ICommand> ManualCommands = new List<ICommand>();
        private List<ICommand> IntervalCommands = new List<ICommand>();
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
                await Task.Delay(5000, stoppingToken);
            }

            return;
        }
    }
}
