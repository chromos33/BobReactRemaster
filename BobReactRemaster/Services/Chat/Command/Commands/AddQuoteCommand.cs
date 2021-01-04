using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.EventBus.MessageDataTypes.Relay;
using BobReactRemaster.Services.Chat.Commands.Base;
using BobReactRemaster.Services.Chat.Commands.Interfaces;
using IdentityServer4.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace BobReactRemaster.Services.Chat.Command.Commands
{
    public class AddQuoteCommand : ICommand
    {
        private readonly IMessageBus Bus;
        private readonly LiveStream _livestream;
        private readonly string Trigger = "!addquote";
        private readonly IServiceScopeFactory _scopeFactory;

        public AddQuoteCommand(IMessageBus bus, LiveStream livestream, IServiceScopeFactory scopefactory)
        {
            Bus = bus;
            _livestream = livestream;
            _scopeFactory = scopefactory;
        }
        public bool IsTriggerable(CommandMessage msg)
        {
            return msg.Message.StartsWith(Trigger) && msg.IsElevated;
        }

        public void TriggerCommand(CommandMessage msg)
        {
            var quotetext = msg.Message.Replace(Trigger,"");
            if (!quotetext.IsNullOrEmpty())
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                Quote tmp = new Quote();
                tmp.LiveStreamID = _livestream.Id;
                tmp.Created = DateTime.Now;
                tmp.Text = quotetext;
                context.Quotes.Add(tmp);
                context.SaveChanges();
                Bus.Publish(_livestream.getRelayMessageData($"Quote added ({tmp.Id})"));
                Bus.Publish(new QuoteCommandAdded(_livestream,tmp));
            }
            else
            {
                Bus.Publish(_livestream.getRelayMessageData("No quote given. !addquote [QuoteText] "));
            }
        }
    }
}
