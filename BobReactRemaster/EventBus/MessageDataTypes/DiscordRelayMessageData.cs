using BobReactRemaster.EventBus.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.EventBus.MessageDataTypes
{
    public class DiscordRelayMessageData : BaseMessageData
    {
        //rethink how stupid you want the user to be i.e. Access database things from Sender or Receiver probably Sender
        public string Message;
        public string DiscordChannel;
        public string DiscordServer;
    }
}