using BobReactRemaster.EventBus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.Data.Models.Stream.Twitch;
using BobReactRemaster.EventBus.MessageDataTypes;
using BobReactRemaster.EventBus.MessageDataTypes.Relay.Twitch;
using BobReactRemaster.Services.Chat.Command.Messages;
using BobReactRemaster.Services.Chat.Commands;
using BobReactRemaster.Services.Scheduler;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        private IRelayService _relayService;
        public bool IsAuthed = false;
        private List<TwitchMessageQueue> Queues = new List<TwitchMessageQueue>();
        private const int BurstLimit = 4;
        private CommandCenter commandCenter;
        #region Initialisation
        public TwitchChat(IMessageBus messageBus, IServiceScopeFactory scopeFactory, IRelayService relayService)
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
            client.OnChatCommandReceived += OnChatCommandReceived;
            client.OnWhisperReceived += OnWhisperReceived;
            client.OnConnected += OnConnected;
            client.OnConnected += Connected;
            client.OnJoinedChannel += ChannelJoined;
            client.OnLeftChannel += ChannelLeft;
            client.OnConnectionError += NotConnected;
            client.OnIncorrectLogin += LoginAuthFailed;
        }

        private void OnWhisperReceived(object? sender, OnWhisperReceivedArgs e)
        {
            Console.WriteLine("test");
        }

        private void OnConnected(object? sender, OnConnectedArgs e)
        {
            Console.WriteLine("test");
        }

        private void OnChatCommandReceived(object? sender, OnChatCommandReceivedArgs e)
        {
            Console.WriteLine("test");
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

        private List<LiveStream> GetRelayLiveStreams()
        {
            var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var test = context.DiscordTextChannels.ToList();
            var streams = context.TwitchStreams.Include(x => x.RelayChannel).AsEnumerable();
            var LiveStreams = streams.Cast<LiveStream>();
            return LiveStreams.ToList();
        }

        private void ChannelJoined(object sender, OnJoinedChannelArgs e)
        {
            if (!MessageQueueExists(e.Channel))
            {
                Queues.Add(new TwitchMessageQueue(e.Channel,false,TimeSpan.FromMilliseconds(200)));
            }
            client.SendMessage(e.Channel,"Relay enabled");
            _relayService.RelayMessage(new RelayMessageFromTwitch(e.Channel,"Relay enabled"),GetRelayLiveStreams());
            //Add Uptime Task
            var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            TwitchStream stream = context.TwitchStreams.Include(x => x.APICredential).Include(x => x.Quotes).FirstOrDefault(x => x.StreamName.ToLower() == e.Channel.ToLower());
            if (stream != null)
            {
                MessageBus.Publish(new RelayStartedMessageData(stream));
            }
            
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
                //Remove UptimeTask for Stream
            }
            var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            TwitchStream stream = context.TwitchStreams.Include(x => x.APICredential).FirstOrDefault(x => x.StreamName.ToLower() == e.Channel.ToLower());
            if (stream != null)
            {
                MessageBus.Publish(new RelayStoppedMessageData(stream));
            }
        }

        private bool MessageQueueExists(string channelname)
        {
            return Queues.Any(x => String.Equals(x.ChannelName, channelname, StringComparison.CurrentCultureIgnoreCase));
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
            _relayService.RelayMessage(new RelayMessageFromTwitch(e.ChatMessage.Channel,MessageWithUserName),GetRelayLiveStreams());
            commandCenter?.HandleCommandMessageAsync(new TwitchCommandMessage(e.ChatMessage.Message,e.ChatMessage.Channel,e.ChatMessage.Username,e.ChatMessage.IsModerator || e.ChatMessage.IsBroadcaster));
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

                if (commandCenter == null)
                {
                    var internalScope = _scopeFactory.CreateScope();
                    //TODO: Might Error because casting null is possible
                    commandCenter = (CommandCenter)internalScope.ServiceProvider.GetServices<IHostedService>()
                            .FirstOrDefault(x => x.GetType() == typeof(CommandCenter));
                }
                await Task.Delay(5000, stoppingToken);
            }
        }
        private void HandleRelayStop(TwitchStreamStopMessageData obj)
        {
            if (IsAuthed && client.IsConnected && client.JoinedChannels.Any(x => x.Channel.ToLower() == obj.StreamName.ToLower()))
            {
                client.SendMessage(obj.StreamName, "Relay disabled");
                _relayService.RelayMessage(new RelayMessageFromTwitch(obj.StreamName, "Relay disabled"),GetRelayLiveStreams());
                client.LeaveChannel(obj.StreamName);
            }
        }
        private void HandleRelayPulse(TwitchRelayPulseMessageData obj)
        {
            if (IsAuthed && client.IsConnected && client.JoinedChannels.All(x => x.Channel.ToLower() != obj.StreamName.ToLower()))
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
