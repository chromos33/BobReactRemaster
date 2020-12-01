using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.BaseClasses;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.JSONModels.Twitch;
using BobReactRemaster.Services.Chat.Commands.Base;
using BobReactRemaster.Services.Chat.Commands.Interfaces;
using Newtonsoft.Json;
using TwitchLib.Api;

namespace BobReactRemaster.Services.Chat.Command.Commands.Twitch
{
    public class TwitchGameChangeCommand : ICommand
    {
        private readonly string Trigger = "!game";
        private readonly IMessageBus Bus;
        private readonly TwitchStream _livestream;
        private HttpClient Client = new HttpClient();
        public TwitchGameChangeCommand(IMessageBus bus, TwitchStream livestream)
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

                var busMessage = _livestream.getRelayMessageData("Kein Game gefunden. Command: '!game [Game Name]");
                Bus.Publish(busMessage);
            }
        }

        private async Task ChangeStreamTitle(string newGame)
        {
            string uri = $"https://api.twitch.tv/helix/games?name={HttpUtility.UrlEncode(newGame.Trim())}";
            var response = await Client.GetAsync(uri);
            BaseMessageData BusMessage = _livestream.getRelayMessageData("Error");
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var resultdata = JsonConvert.DeserializeObject<TwitchGameResponse>(result);
                if (resultdata.data.Count() > 0)
                {
                    string GameID = resultdata.data.First().id;
                    string changeUri = $"https://api.twitch.tv/helix/channels?broadcaster_id={_livestream.StreamID}";
                    var data = JsonConvert.SerializeObject(new { game_id = GameID });
                    var changeResponse = await Client.PatchAsync(changeUri, new StringContent(data, System.Text.Encoding.UTF8, "application/json"));
                    if (changeResponse.IsSuccessStatusCode)
                    {
                        BusMessage = _livestream.getRelayMessageData("Game updated");
                    }
                }
                else
                {
                    BusMessage = _livestream.getRelayMessageData("Kein Spiel unter diesem Namen gefunden");
                }
            }
            Bus.Publish(BusMessage);
            return;
        }
    }
}
