using BobReactRemaster.EventBus.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.EventBus.MessageDataTypes
{
    public class ChatMessageToStreamChat : BaseMessageData
    {
        public string Message;

        public string StreamName;

        //Class of Database Entry
        public string StreamType;
    }
}