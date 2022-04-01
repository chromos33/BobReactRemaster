using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.User;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.EventBus.MessageDataTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BobReactRemaster.Services.Stream
{
    public class SubscriptionService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMessageBus MessageBus;
        public SubscriptionService(IServiceScopeFactory scopeFactory, IMessageBus messageBus)
        {
            _scopeFactory = scopeFactory;
            MessageBus = messageBus;
            MessageBus.RegisterToEvent<StreamCreatedMessageData>(handleStreamCreated);
        }

        private async void handleStreamCreated(StreamCreatedMessageData obj)
        {
            var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            foreach (Member member in context.Members.Include(x => x.StreamSubscriptions))
            {
                member.AddStreamSubscription(obj.Stream,false);
            }
            await context.SaveChangesAsync();
        }
    }
}
