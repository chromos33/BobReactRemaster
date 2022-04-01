using System;
using System.Collections.Generic;
using System.Linq;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Stream;
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
        public override List<BaseMessageData> GetMessageBusMessages(List<LiveStream> LiveStreams)
        {
            List<BaseMessageData> List = new List<BaseMessageData>();
            var Stream = LiveStreams.FirstOrDefault(x =>
                String.Equals(x.StreamName, StreamName, StringComparison.CurrentCultureIgnoreCase) && 
                x.RelayChannel != null);
            if (Stream is { RelayChannel: { } })
            {
                DiscordRelayMessageData data = new DiscordRelayMessageData();
                data.Message = Message;
                data.DiscordChannelID = Stream.RelayChannel.ChannelID;
                List.Add(data);
            }

            return List;
        }
    }
}
