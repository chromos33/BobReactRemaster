using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Commands;
using BobReactRemaster.Services.Chat.Command.Interfaces;
using BobReactRemaster.Services.Chat.Commands.Base;
using BobReactRemaster.Services.Chat.Commands.Interfaces;
using BobReactRemaster.Services.Scheduler.Tasks;

namespace BobReactRemaster.Services.Chat.Command.Commands
{
    public class IntervalRelayCommand: IIntervalCommand,IDisposable
    {
        public IntervalCommandRelayTask RelayTask { get; }
        public IntervalRelayCommand(IntervalCommand command,int id)
        {
            RelayTask = new IntervalCommandRelayTask(id,command.AutoInverval,command.LiveStream,command.Response);
        }

        public void Dispose()
        {
            RelayTask?.QueueRemoval();
        }
    }
}
