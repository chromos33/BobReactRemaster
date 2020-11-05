using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.BaseClasses;

namespace BobReactRemaster.EventBus.MessageDataTypes
{
    public class StreamCreatedMessageData : BaseMessageData
    {
        public StreamCreatedMessageData(TwitchStream stream)
        {
            Stream = stream;
        }

        public LiveStream Stream { get; set; }
    }
}
