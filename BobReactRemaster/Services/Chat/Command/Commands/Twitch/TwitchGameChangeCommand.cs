using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using BobReactRemaster.APIs;
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
        private HttpClient Client;
        public string UpdatedMessage
        {
            get { return "Game updated"; }
        }

        public string ErrorMessage
        {
            get { return "Something went wrong"; }
        }

        public string HelpMessage
        {
            get { return "Kein Titel gefunden. Command: '!title [Titel]"; }
        }
        public string NotFoundMessage
        {
            get { return "Game updated"; }
        }
        public TwitchGameChangeCommand(IMessageBus bus, TwitchStream livestream, HttpClient? client = null)
        {
            Bus = bus;
            _livestream = livestream;
            if (client == null)
            {
                Client = new HttpClient();
            }
            else
            {
                Client = client;
            }
            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {livestream.APICredential.Token}");
            Client.DefaultRequestHeaders.Add("Client-Id", $"{livestream.APICredential.ClientID}");
        }

        public bool IsTriggerable(CommandMessage msg)
        {
            if (msg.Message.StartsWith(Trigger))
            {
                return msg.Message.Split(" ").FirstOrDefault()?.Equals(Trigger) ?? false;
            }
            return false;
        }

        public void TriggerCommand(CommandMessage msg)
        {
            string Parameter = msg.Message.Replace(Trigger, "");
            if (Parameter.Length > 0)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                ChangeStreamTitle(Parameter);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
            else
            {

                var busMessage = _livestream.getRelayMessageData("Command: '!game [Game Name]");
                Bus.Publish(busMessage);
            }
        }

        private async Task ChangeStreamTitle(string newGame)
        {
            string GameID = await TwitchCustomAPI.GetTwitchGameIDFromName(newGame, Client);
            
            BaseMessageData BusMessage = _livestream.getRelayMessageData("Error");
            if (GameID != null)
            {
                if (await TwitchCustomAPI.TryToSetTwitchGame(_livestream.StreamID,GameID,Client))
                {
                    BusMessage = _livestream.getRelayMessageData("Game updated");
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
