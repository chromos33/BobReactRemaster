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
        private RelayRouter RelayRouter;
        #region Initialisation
        public TwitchRelay(IMessageBus messageBus, IServiceScopeFactory scopeFactory, RelayRouter relayRouter)
        {
            MessageBus = messageBus;
            _scopeFactory = scopeFactory;
            RelayRouter = relayRouter;
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
            //TODO create/add channel that handles when to send messages
            //or Subscribe to MessageBus Event for Stream Started that handles creation of said channel object
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
            MessageBus.RegisterToEvent<TwitchRelayMessageData>(RelayMessageReceived);
        }
        #endregion
        #region Events
        private void RelayMessageReceived(TwitchRelayMessageData obj)
        {
            client.SendMessage("chromos",obj.Message);
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            RelayRouter.RelayMessage(new RelayMessage(){channel= e.ChatMessage.Channel,message= e.ChatMessage.Message,SourceType = SourceType.Twitch});
        }
        #endregion
        #region Functions or something rename
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
