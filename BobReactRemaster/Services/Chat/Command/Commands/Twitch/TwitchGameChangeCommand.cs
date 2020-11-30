using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.BaseClasses;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.Services.Chat.Commands.Base;
using BobReactRemaster.Services.Chat.Commands.Interfaces;
using TwitchLib.Api;

namespace BobReactRemaster.Services.Chat.Command.Commands.Twitch
{
    public class TwitchGameChangeCommand : ICommand
    {
        private readonly string Trigger = "!game";
        private readonly IMessageBus Bus;
        private readonly LiveStream _livestream;
        private TwitchAPI api;

        public TwitchGameChangeCommand(IMessageBus bus, TwitchStream livestream)
        {
            Bus = bus;
            _livestream = livestream;
            api = new TwitchAPI();
            api.Settings.ClientId = livestream.APICredential.ClientID;
            api.Settings.AccessToken = livestream.APICredential.Token;
        }

        public bool IsTriggerable(CommandMessage msg)
        {
            return msg.Message.StartsWith(Trigger);
        }

        public void TriggerCommand(CommandMessage msg)
        {
            string Parameter = msg.Message.Replace(Trigger, "");
            if (Parameter.Length > 0)
            {
                ChangeStreamTitle(Parameter);
            }
            else
            {

                var busMessage = _livestream.getRelayMessageData("Kein Game gefunden. Command: '!game [Game Name]");
                Bus.Publish(busMessage);
            }
        }

        private async Task ChangeStreamTitle(string newGame)
        {
            var result = await api.V5.Channels.UpdateChannelAsync(_livestream.Id.ToString(),null, newGame);
            BaseMessageData busMessage = null;
            if (result.Status == newGame)
            {
                busMessage = _livestream.getRelayMessageData("Game updated");
            }
            else
            {
                busMessage = _livestream.getRelayMessageData("Twitch game mismatch");
            }
            Bus.Publish(busMessage);

            return;
        }
    }
}
