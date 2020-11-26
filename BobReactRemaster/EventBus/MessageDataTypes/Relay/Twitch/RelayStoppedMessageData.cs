using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.BaseClasses;

namespace BobReactRemaster.EventBus.MessageDataTypes.Relay.Twitch
{
    public class RelayStoppedMessageData: BaseMessageData
    {
        public LiveStream Stream;
        public RelayStoppedMessageData(LiveStream stream)
        {
            Stream = stream;
        }
    }
}
