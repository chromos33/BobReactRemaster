using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.EventBus.BaseClasses;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.EventBus.MessageDataTypes;
using BobReactRemaster.Services.Chat.GeneralClasses;
using Microsoft.Extensions.DependencyInjection;
using TwitchLib.Api.Core.Models.Undocumented.Comments;

namespace BobReactRemaster.Services.Chat
{
    public class RelayService: IRelayService
    {
        //TODO: Create Tests for this ... and figure out how to mock/something dbcontext
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMessageBus _messageBus;
        public RelayService(IServiceScopeFactory scopeFactory, IMessageBus messageBus)
        {
            _scopeFactory = scopeFactory;
            _messageBus = messageBus;
        }
        public void RelayMessage(RelayMessage MessageObject)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var Messages = MessageObject.GetMessageBusMessages(context);
            foreach (var Message in Messages)
            {
                if (Message != null)
                {
                    _messageBus.Publish(Message);
                }
            }
        }
    }
    public enum SourceType
    {
        Discord,
        Twitch
    }
}
