using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.Services.Chat.Commands.Base;
using BobReactRemaster.Services.Chat.Commands.Interfaces;

namespace BobReactRemaster.Services.Chat.Command.Commands
{
    public class QuoteCommand : ICommand
    {
        private readonly IMessageBus Bus;
        private readonly LiveStream _livestream;
        private readonly Quote quote;
        private readonly string Trigger = "!quote";
        public QuoteCommand(IMessageBus bus, LiveStream livestream, Quote quote)
        {
            Bus = bus;
            _livestream = livestream;
            this.quote = quote;
        }
        public bool IsTriggerable(CommandMessage msg)
        {
            if (msg.Message.StartsWith(Trigger))
            {
                var parameter = msg.Message.Replace(Trigger, "");
                int ID = 0;
                return parameter.Length > 0 && Int32.TryParse(parameter, out ID) && quote.Id == ID;
            }
            return false;
        }

        public void TriggerCommand(CommandMessage msg)
        {
            Bus.Publish(_livestream.getRelayMessageData(quote.ToString()));
        }

        public bool IsFromLiveStream(LiveStream stream)
        {
            return stream.Id == _livestream.Id;
        }
    }
}
