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
    public class TwitchStreamTitleChangeCommand: ICommand
    {
        private readonly string Trigger = "!title";
        private readonly IMessageBus Bus;
        private readonly LiveStream _livestream;
        private TwitchAPI api;

        public TwitchStreamTitleChangeCommand(IMessageBus bus, TwitchStream livestream)
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

                var busMessage =_livestream.getRelayMessageData("Kein Titel gefunden. Command: '!title [Titel]");
                Bus.Publish(busMessage);
            }
        }

        private async Task ChangeStreamTitle(string newTitle)
        {
            var result = await api.V5.Channels.UpdateChannelAsync(_livestream.Id.ToString(), newTitle);
            BaseMessageData busMessage = null;
            if (result.Status == newTitle)
            {
                busMessage = _livestream.getRelayMessageData("Title updated");
            }
            else
            {
                busMessage = _livestream.getRelayMessageData("Title not updated");
            }
            Bus.Publish(busMessage);

            return;
        }
    }
}
