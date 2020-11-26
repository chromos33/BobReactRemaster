using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.BaseClasses;

namespace BobReactRemaster.EventBus.MessageDataTypes.Relay.Twitch
{
    public class RelayStartedMessageData: BaseMessageData
    {
        public LiveStream Stream;

        public RelayStartedMessageData(LiveStream stream)
        {
            Stream = stream;
        }
    }
}
