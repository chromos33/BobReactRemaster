using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.EventBus.BaseClasses;
using BobReactRemaster.EventBus.MessageDataTypes;
using BobReactRemaster.Services.Chat.GeneralClasses;
using Microsoft.EntityFrameworkCore;

namespace BobReactRemaster.Services.Chat.Twitch
{
    //is received from Twitch Channel
    public class RelayMessageFromTwitch: RelayMessage
    {   
        private string StreamName { get; set; }
        public RelayMessageFromTwitch(string streamName, string message)
        {
            StreamName = streamName;
            Message = message;
        }

        public override List<BaseMessageData> GetMessageBusMessages(ApplicationDbContext context)
        {
            List<BaseMessageData> List = new List<BaseMessageData>();
            var Stream = context.TwitchStreams.Include(x => x.RelayChannel).FirstOrDefault(x =>
                String.Equals(x.StreamName, StreamName, StringComparison.CurrentCultureIgnoreCase) && 
                x.RelayChannel != null);
            if (Stream != null)
            {
                DiscordRelayMessageData data = new DiscordRelayMessageData();
                data.Message = Message;
                data.DiscordServer = Stream.RelayChannel.Guild;
                data.DiscordChannel = Stream.RelayChannel.Name;
                List.Add(data);
            }

            return List;
        }
    }
}
