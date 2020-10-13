using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.EventBus.BaseClasses;
using BobReactRemaster.Services.Chat.GeneralClasses;

namespace BobReactRemaster.Services.Chat.Twitch
{
    public class TwitchRelayMessage: RelayMessage
    {   
        private string StreamName { get; set; }
        public TwitchRelayMessage(string streamName, string message)
        {
            StreamName = streamName;
            Message = message;
        }

        public override List<BaseMessageData> GetMessageBusMessages(ApplicationDbContext context)
        {
            throw new NotImplementedException();
        }
    }
}
