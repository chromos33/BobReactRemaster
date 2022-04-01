using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.EventBus.MessageDataTypes;
using BobReactRemaster.EventBus.MessageDataTypes.Relay;
using BobReactRemaster.Migrations;
using BobReactRemaster.Services.Chat.Commands.Base;
using BobReactRemaster.Services.Chat.Commands.Interfaces;

namespace BobReactRemaster.Services.Chat.Command.Commands
{
    public class QuoteCommand : ICommand
    {
        private readonly IMessageBus Bus;
        private readonly LiveStream LiveStream;
        private readonly List<Quote> Quotes;
        private readonly string Trigger = "!quote";
        private Random R = new Random();
        public QuoteCommand(IMessageBus bus, LiveStream liveStream)
        {
            Bus = bus;
            LiveStream = liveStream;
            Quotes = liveStream.Quotes;
        }

        public void AddQuoteCommand(Quote obj)
        {
            Quotes.Add(obj);
        }

        public bool IsTriggerable(CommandMessage msg)
        {
            return msg.Message.StartsWith(Trigger);
        }

        public void TriggerCommand(CommandMessage msg)
        {
            var parameters = msg.Message.Replace(Trigger, "");
            Quote tmpquote;
            if (parameters != "")
            {
                Int32.TryParse(parameters, out int idparam);
                tmpquote = Quotes.FirstOrDefault(x => x.Id == idparam);
            }
            else
            {
                tmpquote = Quotes.ElementAt(R.Next(0, Quotes.Count()));
            }

            if (tmpquote != null)
            {
                Bus.Publish(LiveStream.getRelayMessageData(tmpquote.ToString()));
            }
            else
            {
                Bus.Publish(LiveStream.getRelayMessageData("Kein Quote gefunden. !quote [optionale ID]"));
            }
        }

        public bool IsFromLiveStream(LiveStream stream)
        {
            return stream.Id == LiveStream.Id;
        }
    }
}
