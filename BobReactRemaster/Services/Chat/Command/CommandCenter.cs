using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BobReactRemaster.Services.Chat.Commands.Base;
using BobReactRemaster.Services.Chat.Commands.Interfaces;

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
        public CommandCenter()
        {

        }
        private List<ICommand> Commands;

        public void HandleCommandMessage(CommandMessage msg)
        {

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
