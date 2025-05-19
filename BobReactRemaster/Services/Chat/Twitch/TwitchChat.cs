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
using Microsoft.Extensions.Logging;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;
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

        public TwitchChat(
            IMessageBus messageBus,
            IServiceScopeFactory scopeFactory,
            IRelayService relayService,
            ILogger<TwitchChat> logger
        ) : base(logger)
        {
            MessageBus = messageBus;
            _scopeFactory = scopeFactory;
            _relayService = relayService;
            SubscribeToBusEvents();
            _logger.LogInformation("TwitchChat service initialized.");
        }

        private void InitTwitchClient()
        {
            client = new TwitchClient();
#pragma warning disable CS8622
            client.OnMessageReceived += OnMessageReceived;
            client.OnChatCommandReceived += OnChatCommandReceived;
            client.OnWhisperReceived += OnWhisperReceived;
            client.OnConnected += OnConnected;
            client.OnConnected += Connected;
            client.OnJoinedChannel += ChannelJoined;
            client.OnLeftChannel += ChannelLeft;
            client.OnConnectionError += NotConnected;
            client.OnIncorrectLogin += LoginAuthFailed;
            client.OnDisconnected += Disconnected;
            client.OnUnaccountedFor += Unaccounted;
#pragma warning restore CS8622
            _logger.LogInformation("Twitch client events initialized.");
        }

        private void Unaccounted(object? sender, OnUnaccountedForArgs e)
        {
            _logger.LogWarning("Unaccounted event received from Twitch client.");
        }

        private void Disconnected(object? sender, OnDisconnectedEventArgs e)
        {
            ConnectionChangeInProgress = false;
            IsAuthed = false;
            _logger.LogWarning("Twitch client disconnected.");
        }

        private void OnWhisperReceived(object? sender, OnWhisperReceivedArgs e)
        {
            _logger.LogInformation("Whisper received from {User}", e.WhisperMessage.Username);
        }

        private void OnConnected(object? sender, OnConnectedArgs e)
        {
            _logger.LogInformation("Twitch client connected to {Server} as {BotName}", e.ToString(), e.BotUsername);
        }

        private void OnChatCommandReceived(object? sender, OnChatCommandReceivedArgs e)
        {
            _logger.LogInformation("Chat command received: {Command} in {Channel}", e.Command.CommandText, e.Command.ChatMessage.Channel);
        }

        private void LoginAuthFailed(object sender, OnIncorrectLoginArgs e)
        {
            ConnectionChangeInProgress = false;
            IsAuthed = false;
            client.Disconnect();
            _logger.LogError("Twitch login authentication failed: {ErrorMessage}", e.Exception?.Message ?? "Unknown error");
        }

        private void NotConnected(object sender, OnConnectionErrorArgs e)
        {
            ConnectionChangeInProgress = false;
            _logger.LogError("Twitch connection error: {ErrorMessage}", e.Error.Message);
        }

        private ConnectionCredentials? GetTwitchCredentials()
        {
            var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            TwitchCredential Credential = context.TwitchCredentials.FirstOrDefault(x => x.isMainAccount && x.Token != "");
            if (Credential == null)
            {
                _logger.LogError("No main Twitch credentials found in database.");
            }
            return Credential?.GetRelayConnectionCredentials();
        }

        private List<LiveStream> GetRelayLiveStreams()
        {
            var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var streams = context.TwitchStreams.Include(x => x.RelayChannel).AsEnumerable();
            var LiveStreams = streams.Cast<LiveStream>();
            _logger.LogDebug("Fetched {Count} relay live streams.", LiveStreams.Count());
            return LiveStreams.ToList();
        }

        private void ChannelJoined(object sender, OnJoinedChannelArgs e)
        {
            if (!MessageQueueExists(e.Channel))
            {
                Queues.Add(new TwitchMessageQueue(e.Channel, false, TimeSpan.FromMilliseconds(200)));
                _logger.LogInformation("Joined channel: {Channel}", e.Channel);
            }
            client.SendMessage(e.Channel, "Relay enabled");
            _relayService.RelayMessage(new RelayMessageFromTwitch(e.Channel, "Relay enabled"), GetRelayLiveStreams());
            var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            TwitchStream stream = context.TwitchStreams.Include(x => x.APICredential).Include(x => x.Quotes).FirstOrDefault(x => x.StreamName.ToLower() == e.Channel.ToLower());
            if (stream != null)
            {
                MessageBus.Publish(new RelayStartedMessageData(stream));
                _logger.LogInformation("Published RelayStartedMessageData for stream: {StreamName}", stream.StreamName);
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
                    _logger.LogInformation("Left channel and removed message queue: {Channel}", e.Channel);
                }
            }
            var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            TwitchStream stream = context.TwitchStreams.Include(x => x.APICredential).FirstOrDefault(x => x.StreamName.ToLower() == e.Channel.ToLower());
            if (stream != null)
            {
                MessageBus.Publish(new RelayStoppedMessageData(stream));
                _logger.LogInformation("Published RelayStoppedMessageData for stream: {StreamName}", stream.StreamName);
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
            _logger.LogInformation("Twitch client fully connected and authenticated.");
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
                    _logger.LogInformation("Twitch client initialized and connecting...");
                }
                else
                {
                    _logger.LogError("Twitch client could not connect: No credentials available.");
                }
            }
        }

        private void SubscribeToBusEvents()
        {
            MessageBus.RegisterToEvent<TwitchRelayMessageData>(RelayMessageReceived);
            MessageBus.RegisterToEvent<TwitchRelayPulseMessageData>(HandleRelayPulse);
            MessageBus.RegisterToEvent<TwitchStreamStopMessageData>(HandleRelayStop);
            _logger.LogInformation("Subscribed to Twitch bus events.");
        }

        private void RelayMessageReceived(TwitchRelayMessageData obj)
        {
            Queues.FirstOrDefault(x => String.Equals(x.ChannelName, obj.StreamName, StringComparison.CurrentCultureIgnoreCase))
                ?.AddMessage(obj.Message);
            _logger.LogInformation("Relayed message to channel {Channel}: {Message}", obj.StreamName, obj.Message);
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            HandleQueueModeratorStatus(e);
            string MessageWithUserName = $"{e.ChatMessage.Username}: {e.ChatMessage.Message}";
            _relayService.RelayMessage(new RelayMessageFromTwitch(e.ChatMessage.Channel, MessageWithUserName), GetRelayLiveStreams());
            _logger.LogInformation("Received message in {Channel} from {User}: {Message}", e.ChatMessage.Channel, e.ChatMessage.Username, e.ChatMessage.Message);
            commandCenter?.HandleCommandMessageAsync(new TwitchCommandMessage(e.ChatMessage.Message, e.ChatMessage.Channel, e.ChatMessage.Username, e.ChatMessage.IsModerator || e.ChatMessage.IsBroadcaster));
        }

        private void HandleQueueModeratorStatus(OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.IsMe && e.ChatMessage.IsModerator)
            {
                Queues.FirstOrDefault(x => String.Equals(x.ChannelName, e.ChatMessage.Channel, StringComparison.CurrentCultureIgnoreCase))?.EnableModeratorMode();
                _logger.LogDebug("Enabled moderator mode for channel: {Channel}", e.ChatMessage.Channel);
            }
        }

        private bool SendMessage(string channelName, string message)
        {
            try
            {
                var channel = client.JoinedChannels.First(x => x.Channel.ToLower() == channelName.ToLower());
                client.SendMessage(channel, message);
                _logger.LogInformation("Sent message to {Channel}: {Message}", channelName, message);
                return true;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Failed to send message to {Channel}", channelName);
                return false;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _MessageTimer = new System.Timers.Timer(50);
            _MessageTimer.Elapsed += (sender, args) => MessageClock();
            _MessageTimer.Start();
            _logger.LogInformation("TwitchChat background service started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!IsAuthed && !ConnectionChangeInProgress)
                {
                    InitTwitchClient();
                    Connect();
                }

                if (commandCenter == null)
                {
                    var internalScope = _scopeFactory.CreateScope();
                    commandCenter = (CommandCenter)internalScope.ServiceProvider.GetServices<IHostedService>()
                            .FirstOrDefault(x => x.GetType() == typeof(CommandCenter));
                    if (commandCenter != null)
                    {
                        _logger.LogInformation("CommandCenter resolved and assigned.");
                    }
                }
                await Task.Delay(5000, stoppingToken);
            }
            _logger.LogInformation("TwitchChat background service stopping.");
        }

        private void HandleRelayStop(TwitchStreamStopMessageData obj)
        {
            if (IsAuthed && client.IsConnected && client.JoinedChannels.Any(x => x.Channel.ToLower() == obj.StreamName.ToLower()))
            {
                client.SendMessage(obj.StreamName, "Relay disabled");
                _relayService.RelayMessage(new RelayMessageFromTwitch(obj.StreamName, "Relay disabled"), GetRelayLiveStreams());
                client.LeaveChannel(obj.StreamName);
                _logger.LogInformation("Relay stopped and left channel: {Channel}", obj.StreamName);
            }
        }

        private void HandleRelayPulse(TwitchRelayPulseMessageData obj)
        {
            if (IsAuthed && client.IsConnected && client.JoinedChannels.All(x => x.Channel.ToLower() != obj.StreamName.ToLower()))
            {
                client.JoinChannel(obj.StreamName);
                _logger.LogInformation("Relay pulse: joined channel {Channel}", obj.StreamName);
            }
        }

        private bool MessagesInProgress = false;
        private async void MessageClock()
        {
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
