using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.EventBus.BaseClasses;

namespace BobReactRemaster.EventBus.MessageDataTypes
{
    public class TwitchStreamStartMessageData: BaseMessageData
    {
        public string Streamname { get; set; }
        public string Title { get; set; }

    }
}
