using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.EventBus.BaseClasses;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.Services.Chat.Commands.Interfaces;
using BobReactRemaster.Services.Scheduler;

namespace BobReactRemaster.Data.Models.Stream.DLive
{
    public class DLiveStream: LiveStream
    {
        public override void SetURL(string URL)
        {
            throw new NotImplementedException();
        }

        public override void StartStream()
        {
            throw new NotImplementedException();
        }

        public override void SetStreamStarted(DateTime date)
        {
            throw new NotImplementedException();
        }

        public override void StopStream()
        {
            throw new NotImplementedException();
        }

        public override BaseMessageData getRelayMessageData(string message)
        {
            throw new NotImplementedException();
        }

        public override IScheduledTask GetUpTimeTask()
        {
            throw new NotImplementedException();
        }

        public override bool HasStaticCommands()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<ICommand> GetStaticCommands(IMessageBus bus)
        {
            throw new NotImplementedException();
        }
    }
}
