using BobReactRemaster.EventBus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Stream.Twitch;
using BobReactRemaster.EventBus.MessageDataTypes;
using Microsoft.Extensions.DependencyInjection;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;

namespace BobReactRemaster.Services.Chat.Twitch
{
    public class TwitchRelay : BackgroundService
    {
        private IMessageBus MessageBus;
        private TwitchClient client;
        private readonly IServiceScopeFactory _scopeFactory;
        #region Initialisation
        public TwitchRelay(IMessageBus messageBus, IServiceScopeFactory scopeFactory)
        {
            MessageBus = messageBus;
            _scopeFactory = scopeFactory;
            SubscribeToBusEvents();
            client = new TwitchClient();
        }

        private void InitTwitchClient()
        {
            
            client.OnMessageReceived += OnMessageReceived;
            client.OnConnected += Connected;
            client.OnJoinedChannel += ChannelJoined;
            client.OnConnectionError += NotConnected;
            client.OnError += Errored;
            client.OnIncorrectLogin += LoginAuthFailed;
            client.OnFailureToReceiveJoinConfirmation += NoJoinConf;
            client.OnNoPermissionError += NoPermission;
            client.OnExistingUsersDetected += ExistingUSerDetected;
        }

        private void ExistingUSerDetected(object? sender, OnExistingUsersDetectedArgs e)
        {
            throw new NotImplementedException();
        }

        private void NoPermission(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void NoJoinConf(object? sender, OnFailureToReceiveJoinConfirmationArgs e)
        {
            throw new NotImplementedException();
        }

        private void LoginAuthFailed(object? sender, OnIncorrectLoginArgs e)
        {
            ConnectionChangeInProgress = false;
        }

        private void Errored(object sender, OnErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void NotConnected(object sender, OnConnectionErrorArgs e)
        {
            ConnectionChangeInProgress = false;
        }

        private ConnectionCredentials GetTwitchCredentials()
        {
            var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            TwitchCredential Credential = context.TwitchCredentials.FirstOrDefault(x => x.isMainAccount && x.Token != "");
            return Credential?.GetRelayConnectionCredentials();
        }

        private void ChannelJoined(object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine("Joined");
            client.SendMessage(e.Channel,"Test");
        }

        private void Connected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine("Connected");
            ConnectionChangeInProgress = false;
            client.JoinChannel("chromos33");
        }

        private bool ConnectionChangeInProgress;
        private void Connect()
        {
            if (!ConnectionChangeInProgress)
            {
                ConnectionChangeInProgress = true;
                var Credential = GetTwitchCredentials();
                if (Credential != null)
                {
                    client.Initialize(Credential);
                    client.Connect();
                }
                
            }
        }
        private void SubscribeToBusEvents()
        {
            MessageBus.RegisterToEvent<ChatMessageToStreamChat>(RelayMessageReceived);
        }
        #endregion
        #region Events
        private void RelayMessageReceived(ChatMessageToStreamChat obj)
        {
            //TODO use Dictionary With RelayChannel objects add Message to Corresponding RelayChannel Message "TODO"-List
            throw new NotImplementedException();
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            RelayMessage(e.ChatMessage.Message, e.ChatMessage.Channel);
        }
        #endregion
        #region Functions or something rename
        //TODO
        //Needs Timer
        //TODO loop through Dictionary if RelayChannel has ElevatedPermissions (i.e. is Mod) forgo the burst protection otherwise 1 channel/message at a time
        private void RelayMessage(string message, string streamname)
        {
            MessageBus.Publish(new RelayMessageFromStreamChat() { Message = message, DiscordChannel = streamname });
        }
        private bool SendMessage(string channelName, string message)
        {
            //Try catch here in case
            try
            {
                client.SendMessage(client.JoinedChannels.Where(x => x.Channel.ToLower() == channelName.ToLower()).First(), message);
                return true;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }
        #endregion

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //Reconnect on Disconnect
                if (!client.IsConnected && !ConnectionChangeInProgress)
                {
                    InitTwitchClient();
                    Connect();
                }
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
