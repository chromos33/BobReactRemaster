﻿using System;
using System.Collections.Generic;
using System.Linq;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.BaseClasses;
using BobReactRemaster.EventBus.MessageDataTypes;
using BobReactRemaster.Services.Chat.GeneralClasses;
using Microsoft.EntityFrameworkCore;

namespace BobReactRemaster.Services.Chat.Discord
{
    //is received from Discord Channel
    public class RelayMessageFromDiscord: RelayMessage
    {
        private string Channel { get; set; }
        private string Guild { get; set; }

        public RelayMessageFromDiscord( string guild, string channel, string message)
        {
            Channel = channel;
            Guild = guild;
            Message = message;
        }
        public override List<BaseMessageData> GetMessageBusMessages(List<LiveStream> LiveStreams)
        {
            List<BaseMessageData> List = new List<BaseMessageData>();

            var LiveStream = LiveStreams.FirstOrDefault(x =>
                x.RelayChannel != null &&
                String.Equals(x.RelayChannel.Guild, Guild, StringComparison.CurrentCultureIgnoreCase) &&
                String.Equals(x.RelayChannel.Name, Channel, StringComparison.CurrentCultureIgnoreCase)
            );
            if (LiveStream != null)
            {
                List.Add(LiveStream.getRelayMessageData(Message));
            }


            return List;
        }
    }
}
