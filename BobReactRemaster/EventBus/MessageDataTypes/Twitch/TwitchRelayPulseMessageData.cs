using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.EventBus.BaseClasses;

namespace BobReactRemaster.EventBus.MessageDataTypes
{
    //for starting / keeping Relay alive in case of some error
    public class TwitchRelayPulseMessageData : BaseMessageData
    {
        public string StreamName { get; set; }

    }
}
