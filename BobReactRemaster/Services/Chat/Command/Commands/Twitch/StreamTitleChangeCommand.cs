using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.BaseClasses;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.Services.Chat.Commands.Base;
using BobReactRemaster.Services.Chat.Commands.Interfaces;
using Newtonsoft.Json;
using TwitchLib.Api;
using TwitchLib.Api.V5.Models.Channels;

namespace BobReactRemaster.Services.Chat.Command.Commands.Twitch
{
    public class TwitchStreamTitleChangeCommand: ICommand
    {
        private readonly string Trigger = "!title";
        private readonly IMessageBus Bus;
        private readonly TwitchStream _livestream;
        private HttpClient Client = new HttpClient();

        public TwitchStreamTitleChangeCommand(IMessageBus bus, TwitchStream livestream)
        {
            Bus = bus;
            _livestream = livestream;
            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {livestream.APICredential.Token}");
            Client.DefaultRequestHeaders.Add("Client-Id", $"{livestream.APICredential.ClientID}");
            
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
            string uri = $"https://api.twitch.tv/helix/channels?broadcaster_id={_livestream.StreamID}";
            var data = JsonConvert.SerializeObject(new {title = newTitle});
            var result = await Client.PatchAsync(uri, new StringContent(data,System.Text.Encoding.UTF8,"application/json"));
            BaseMessageData busMessage = null;
            if (result.IsSuccessStatusCode)
            {
                busMessage = _livestream.getRelayMessageData("Title updated");
            }
            else
            {
                busMessage = _livestream.getRelayMessageData("Something went wrong");
            }
            Bus.Publish(busMessage);
            return;
        }
    }
}
