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
        public IntervalCommandTask Task { get; }
        public IntervalRelayCommand(IntervalCommand command,int id)
        {
            Task = new IntervalCommandTask(id,command.AutoInverval,command.LiveStream);
        }

        public void Dispose()
        {
            Task?.QueueRemoval();
        }
    }
}
