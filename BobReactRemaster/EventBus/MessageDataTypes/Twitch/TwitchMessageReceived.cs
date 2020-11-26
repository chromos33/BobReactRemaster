using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.EventBus.BaseClasses;

namespace BobReactRemaster.EventBus.MessageDataTypes
{
    public class TwitchMessageReceived: BaseMessageData
    {
        public string Message;
        public string ChannelName;
        public string Author;
    }
}
