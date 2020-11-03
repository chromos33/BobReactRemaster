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
using Timer = System.Timers.Timer;

namespace BobReactRemaster.Services.Chat.Twitch
{
    public class TwitchChat : BackgroundService
    {
        private IMessageBus MessageBus;
        private TwitchClient client;
        private readonly IServiceScopeFactory _scopeFactory;
        private RelayService _relayService;
        public bool IsAuthed = false;
        private List<TwitchMessageQueue> Queues = new List<TwitchMessageQueue>();
        private const int BurstLimit = 4;
        #region Initialisation
        public TwitchChat(IMessageBus messageBus, IServiceScopeFactory scopeFactory, RelayService relayService)
        {
            MessageBus = messageBus;
            _scopeFactory = scopeFactory;
            _relayService = relayService;
            SubscribeToBusEvents();
        }

        private void InitTwitchClient()
        {
            client = new TwitchClient();
            client.OnMessageReceived += OnMessageReceived;
            client.OnConnected += Connected;
            client.OnJoinedChannel += ChannelJoined;
            client.OnLeftChannel += ChannelLeft;
            client.OnConnectionError += NotConnected;
            client.OnIncorrectLogin += LoginAuthFailed;
        }


        private void LoginAuthFailed(object sender, OnIncorrectLoginArgs e)
        {
            ConnectionChangeInProgress = false;
            IsAuthed = false;
            client.Disconnect();
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
            if (!MessageQueueExists(e.Channel))
            {
                Queues.Add(new TwitchMessageQueue(e.Channel,false,TimeSpan.FromMilliseconds(200)));
            }
            client.SendMessage(e.Channel,"Relay enabled");
        }
        private void ChannelLeft(object sender, OnLeftChannelArgs e)
        {
            if (MessageQueueExists(e.Channel))
            {
                var tmp = Queues.FirstOrDefault(x => x.ChannelName.ToLower() == e.Channel.ToLower());
                if (tmp != null)
                {
                    Queues.Remove(tmp);
                }
            }
        }

        private bool MessageQueueExists(string channelname)
        {
            return Queues.FirstOrDefault(x => String.Equals(x.ChannelName, channelname, StringComparison.CurrentCultureIgnoreCase)) != null;
        }

        private void Connected(object sender, OnConnectedArgs e)
        {
            ConnectionChangeInProgress = false;
            IsAuthed = true;
        }

        private bool ConnectionChangeInProgress;
        private Timer _MessageTimer;

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
            MessageBus.RegisterToEvent<TwitchRelayPulseMessageData>(HandleRelayPulse);
            MessageBus.RegisterToEvent<TwitchStreamStopMessageData>(HandleRelayStop);
        }

        #endregion
        #region Events
        private void RelayMessageReceived(TwitchRelayMessageData obj)
        {
            Queues.FirstOrDefault(x => String.Equals(x.ChannelName, obj.StreamName, StringComparison.CurrentCultureIgnoreCase))
                ?.AddMessage(obj.Message);
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            HandleQueueModeratorStatus(e);
            string MessageWithUserName = $"{e.ChatMessage.Username}: {e.ChatMessage.Message}";
            _relayService.RelayMessage(new RelayMessageFromTwitch(e.ChatMessage.Channel,MessageWithUserName));
        }

        private void HandleQueueModeratorStatus(OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.IsMe && e.ChatMessage.IsModerator)
            {
                Queues.FirstOrDefault(x => String.Equals(x.ChannelName, e.ChatMessage.Channel, StringComparison.CurrentCultureIgnoreCase))?.EnableModeratorMode();
            }
        }
        #endregion
        #region Functions or something rename
        private bool SendMessage(string channelName, string message)
        {
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
            _MessageTimer = new System.Timers.Timer(50);
            _MessageTimer.Elapsed += (sender, args) => MessageClock();
            _MessageTimer.Start();
            while (!stoppingToken.IsCancellationRequested)
            {
                //Reconnect on Disconnect
                if (!IsAuthed && !ConnectionChangeInProgress)
                {
                    InitTwitchClient();
                    Connect();
                }
                await Task.Delay(5000, stoppingToken);
            }
        }
        private void HandleRelayStop(TwitchStreamStopMessageData obj)
        {
            if (IsAuthed && client.IsConnected)
            {
                SendMessage(obj.StreamName, "Relay disabled");
                client.LeaveChannel(obj.StreamName);
            }
        }
        private void HandleRelayPulse(TwitchRelayPulseMessageData obj)
        {
            if (IsAuthed && client.IsConnected)
            {
                client.JoinChannel(obj.StreamName);
            }
        }

        private bool MessagesInProgress = false;
        private async void MessageClock()
        {
            //Do not need to wait as Moderator has less stringent limits
            handleModeratorQueues();
            if (!MessagesInProgress)
            {
                MessagesInProgress = true;
                
                await handleNormalQueues();
                MessagesInProgress = false;
            }
            

        }

        private async Task handleNormalQueues()
        {
            int burstcount = 0;
            foreach (TwitchMessageQueue NormalQueues in Queues.Where(x => !x.isModerator))
            {
                var nextMessage = NormalQueues.NextQueuedMessage();
                if (nextMessage != null)
                {
                    SendMessage(NormalQueues.ChannelName, nextMessage);
                }
                burstcount++;
                if (BurstLimit == burstcount)
                {
                    await Task.Delay(2000);
                    burstcount = 0;
                }
                
            }
        }

        private void handleModeratorQueues()
        {
            foreach (TwitchMessageQueue ModeratorQueue in Queues.Where(x => x.isModerator))
            {
                var nextMessage = ModeratorQueue.NextQueuedMessage();
                if (nextMessage != null)
                {
                    SendMessage(ModeratorQueue.ChannelName, nextMessage);
                }
                
            }
        }
    }
}
