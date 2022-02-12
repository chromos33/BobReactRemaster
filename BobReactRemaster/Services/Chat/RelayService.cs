using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.BaseClasses;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.EventBus.MessageDataTypes;
using BobReactRemaster.Services.Chat.GeneralClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TwitchLib.Api.Core.Models.Undocumented.Comments;

namespace BobReactRemaster.Services.Chat
{
    public class RelayService: IRelayService
    {
        private readonly IMessageBus _messageBus;
        public RelayService(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }
        //A LiveStream in the List requires a reference to RelayChannel otherwise nothing will be Relayed because that signals no Relay enabled
        public void RelayMessage(RelayMessage MessageObject,List<LiveStream> LiveStreamsWithRelayChannelDataIncluded)
        {
            var Messages = MessageObject.GetMessageBusMessages(LiveStreamsWithRelayChannelDataIncluded);
            foreach (var Message in Messages)
            {
                if (Message != null)
                {
                    //_messageBus.Publish(Message);
                }
            }
        }
    }
}
