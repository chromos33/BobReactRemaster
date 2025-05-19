using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.Data.Models.User;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.EventBus.MessageDataTypes;
using Discord.WebSocket;
using Discord;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TokenType = Discord.TokenType;

namespace BobReactRemaster.Services.Chat.Discord
{
    public class DiscordChat : BackgroundService
    {
        private DiscordSocketClient _client;
        private IMessageBus MessageBus;
        private readonly IServiceScopeFactory _scopeFactory;
        private IRelayService _relayService;
        private readonly IConfiguration _configuration;
        private bool isConnected = false;
        public DiscordChat(IMessageBus messageBus, IServiceScopeFactory scopeFactory, IRelayService relayService, IConfiguration configuration,
            ILogger<DiscordChat> logger) : base(logger)
        {
            MessageBus = messageBus;
            _scopeFactory = scopeFactory;
            _relayService = relayService;
            _configuration = configuration;
            InitClient();
            InitEvents();
            SubscribeToBusEvents();
        }

        private void SubscribeToBusEvents()
        {
            MessageBus.RegisterToEvent<DiscordRelayMessageData>(RelayMessageReceived);
            MessageBus.RegisterToEvent<TwitchStreamStartMessageData>(StreamStarted);
            MessageBus.RegisterToEvent<StreamCreatedMessageData>(StreamCreated);
            MessageBus.RegisterToEvent<DiscordWhisperData>(SendWhisper);
        }

        private async void SendWhisper(DiscordWhisperData obj)
        {
            while (_client.ConnectionState != ConnectionState.Connected && _client.DMChannels.Count() == 0)
            {
                await Task.Delay(1000);
            }
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var member = context.Members.FirstOrDefault(x => x.DiscordUserName == obj.MemberName);
            if (member != null && obj.Message != "" && member.DiscordID != null)
            {
                var chatmember = await _client.GetUserAsync((ulong)member.DiscordID);
                if (chatmember != null)
                {
                    await chatmember.SendMessageAsync(obj.Message);
                    _logger.LogInformation("Sent whisper to {MemberName}", obj.MemberName);
                }
                else
                {
                    _logger.LogWarning("Could not find Discord user for whisper: {MemberName}", obj.MemberName);
                }
            }
            else
            {
                _logger.LogWarning("Invalid whisper request: MemberName={MemberName}, MessageEmpty={IsEmpty}, DiscordIDNull={IsNull}",
                    obj.MemberName, string.IsNullOrEmpty(obj.Message), member?.DiscordID == null);
            }
        }

        private void StreamCreated(StreamCreatedMessageData obj)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            foreach (Member member in context.Members.AsEnumerable().Where(x => x.canBeFoundOnDiscord()))
            {
                string message = obj.Stream.GetSubscriptionCreatedMessage();
                // _client.GetUser(member.DiscordUserName, member.DiscordDiscriminator).SendMessageAsync(message);
                _logger.LogInformation("Would notify {User} about new stream: {Message}", member.DiscordUserName, message);
            }
        }

        internal async Task<SocketGuildUser?> GetMemberByName(string userName)
        {
            var users = _client.Guilds.Where(x => x.Name == "Deathmic").FirstOrDefault()?.Users.Where(x => x.Username.ToLower() == userName.ToLower());
            if (users != null && users.Count() == 1)
            {
                _logger.LogDebug("Found Discord user by name: {UserName}", userName);
                return users.FirstOrDefault();
            }
            _logger.LogWarning("Could not find unique Discord user by name: {UserName}", userName);
            return null;
        }

        private void StreamStarted(TwitchStreamStartMessageData obj)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            TwitchStream? stream = context.TwitchStreams.Include(x => x.RelayChannel).FirstOrDefault(x => String.Equals(x.StreamName, obj.Streamname, StringComparison.CurrentCultureIgnoreCase));
            if (stream != null)
            {
                string? message = stream.GetStreamStartedMessage(obj.Title);
                if (message != null)
                {
                    foreach (Member member in context.Members.Include(x => x.StreamSubscriptions).ThenInclude(x => x.LiveStream).AsEnumerable().Where(x => x.canBeFoundOnDiscord() && x.HasSubscription(stream)))
                    {
                        _client.GetUser(member.DiscordUserName, member.DiscordDiscriminator).SendMessageAsync(message);
                        _logger.LogInformation("Notified {User} about stream start: {StreamName}", member.DiscordUserName, obj.Streamname);
                    }
                }
            }
            else
            {
                _logger.LogWarning("Stream not found for StreamStarted event: {StreamName}", obj.Streamname);
            }
        }

        private void RelayMessageReceived(DiscordRelayMessageData obj)
        {
            ((SocketTextChannel)_client.GetChannel(obj.DiscordChannelID)).SendMessageAsync(obj.Message);
            _logger.LogInformation("Relayed message to channel {ChannelId}: {Message}", obj.DiscordChannelID, obj.Message);
        }

        private void InitClient()
        {
            var discordConfig = new DiscordSocketConfig { MessageCacheSize = 100 };
            discordConfig.AlwaysDownloadUsers = true;
            discordConfig.LargeThreshold = 250;
            discordConfig.GatewayIntents = GatewayIntents.All;
            _client = new DiscordSocketClient(discordConfig);
            _logger.LogInformation("Initialized Discord client");
        }
        private void InitEvents()
        {
            _client.MessageReceived += MessageReceived;
            _client.ChannelCreated += ChannelCreated;
            _client.ChannelDestroyed += ChannelDestroyed;
            _client.ChannelUpdated += ChannelUpdated;
            _client.GuildAvailable += GuildJoined;
            _client.Connected += Connected;
            _client.Ready += Ready;
            _client.Disconnected += Disconnected;
            _logger.LogInformation("Initialized Discord client events");
        }

        private async Task Disconnected(Exception exception)
        {
            isConnected = false;
            _logger.LogWarning(exception, "Discord Disconnected at {Time}", DateTime.Now);
            try
            {
                await Restart();
                await Connect();
                await Task.Delay(60000);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reconnect attempt failed at {Time}", DateTime.Now);
            }
            _logger.LogInformation("Discord Reconnected at {Time}", DateTime.Now);
        }

        private Task Ready()
        {
            _logger.LogInformation("Discord client is ready");
            _client.DownloadUsersAsync(_client.Guilds.Where(x => x.Name == "Deathmic"));
            _client.DownloadUsersAsync(_client.Guilds.Where(x => x.Name == "Phantastische Partie"));
            return Task.CompletedTask;
        }

        private Task Connected()
        {
            _logger.LogInformation("Discord Connected at {Time}", DateTime.Now);
            isConnected = true;
            return Task.CompletedTask;
        }

        private Task GuildJoined(SocketGuild arg)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            bool write = false;
            foreach (var CategoryChannel in arg.CategoryChannels.Where(x =>
                    x.Name.ToLower() == "community streams"))
            {
                foreach (SocketGuildChannel TextChannel in CategoryChannel.Channels.Where(x => x.GetType() == typeof(SocketTextChannel)))
                {
                    if (!context.DiscordTextChannels.Any(x => x.ChannelID == TextChannel.Id))
                    {
                        var channel = (SocketTextChannel)TextChannel;
                        var NewTextChannel = new TextChannel(channel.Id, channel.Name, channel.Guild.Name);
                        context.DiscordTextChannels.Add(NewTextChannel);
                        write = true;
                        _logger.LogInformation("Added new Discord text channel: {ChannelName} ({ChannelId})", channel.Name, channel.Id);
                    }
                }
            }
            if (write)
            {
                context.SaveChanges();
                _logger.LogInformation("Saved new Discord text channels to database");
            }
            return Task.CompletedTask;
        }

        private Task ChannelUpdated(SocketChannel arg1, SocketChannel arg2)
        {
            if (arg2.GetType() == typeof(SocketTextChannel))
            {
                var textchannel = (SocketTextChannel)arg2;
                if (textchannel.Category.Name.ToLower() == "community streams")
                {
                    using var scope = _scopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var channel = context.DiscordTextChannels.FirstOrDefault(x => x.ChannelID == arg2.Id);
                    if (channel != null)
                    {
                        var tmpchannel = (SocketTextChannel)arg2;
                        channel.Update(tmpchannel.Name, tmpchannel.Guild.Name);
                        context.SaveChanges();
                        _logger.LogInformation("Updated Discord text channel: {ChannelName} ({ChannelId})", tmpchannel.Name, tmpchannel.Id);
                    }
                }
            }
            return Task.CompletedTask;
        }

        private Task ChannelDestroyed(SocketChannel arg)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var channel = context.DiscordTextChannels.FirstOrDefault(x => x.ChannelID == arg.Id);
            if (channel != null)
            {
                context.DiscordTextChannels.Remove(channel);
                context.SaveChanges();
                _logger.LogInformation("Removed Discord text channel: {ChannelId}", arg.Id);
            }
            return Task.CompletedTask;
        }

        private Task ChannelCreated(SocketChannel arg)
        {
            if (arg.GetType() == typeof(SocketTextChannel))
            {
                var TextChannel = (SocketTextChannel)arg;
                if (TextChannel.Category.Name.ToLower() == "community streams")
                {
                    using var scope = _scopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    if (!context.DiscordTextChannels.Any(x => x.ChannelID == TextChannel.Id))
                    {
                        var channel = (SocketTextChannel)TextChannel;
                        var NewTextChannel = new TextChannel(channel.Id, channel.Name, channel.Guild.Name);
                        //next line quick fix need detection for random channel and actual
                        NewTextChannel.IsPermanentRelayChannel = true;
                        context.DiscordTextChannels.Add(NewTextChannel);
                        context.SaveChanges();
                        _logger.LogInformation("Created new Discord text channel: {ChannelName} ({ChannelId})", channel.Name, channel.Id);
                    }
                }
            }
            return Task.CompletedTask;
        }
        private List<LiveStream> GetRelayLiveStreams()
        {
            var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var streams = context.TwitchStreams.Include(x => x.RelayChannel).AsEnumerable();
            var LiveStreams = streams.Cast<LiveStream>();
            _logger.LogDebug("Fetched {Count} relay live streams", LiveStreams.Count());
            return LiveStreams.ToList();
        }
        private Task MessageReceived(SocketMessage arg)
        {
            if (!arg.Author.IsBot)
            {
                if (isMessage(arg))
                {
                    try
                    {
                        var GuildName = ((SocketTextChannel)arg.Channel).Guild.Name;
                        string MessageWithUserName = $"{arg.Author.Username}: {arg.Content}";
                        _relayService.RelayMessage(new RelayMessageFromDiscord(
                            GuildName,
                            arg.Channel.Name,
                            MessageWithUserName
                        ), GetRelayLiveStreams());
                        _logger.LogInformation("Relayed message from Discord user {User}: {Message}", arg.Author.Username, arg.Content);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Exception in MessageReceived");
                    }
                }
                else
                {
                    //Manual filter because other Commands will be handled by a Service
                    if (arg.Content.Equals("!wil", StringComparison.CurrentCultureIgnoreCase))
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        var user = context.Members.FirstOrDefault(x =>
                            String.Equals(x.UserName, arg.Author.Username, StringComparison.CurrentCultureIgnoreCase));
                        string WebserverAddress = _configuration.GetValue<string>("WebServerWebAddress");
                        var Message = $"Adresse: {WebserverAddress}";

                        if (user == null)
                        {
                            var userRegistrationService = scope.ServiceProvider.GetRequiredService<IUserRegistrationService>();
                            var Password = userRegistrationService.RegisterUser(arg.Author.Username, arg.Author.Id);
                            Message += $" Password: {Password}";
                            _logger.LogInformation("Registered new user {UserName} via !wil command", arg.Author.Username);
                        }
                        arg.Author.SendMessageAsync(Message);
                    }
                    if (arg.Content.Equals("!wilnsb", StringComparison.CurrentCultureIgnoreCase))
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        var user = context.Members.FirstOrDefault(x =>
                            String.Equals(x.UserName, arg.Author.Username, StringComparison.CurrentCultureIgnoreCase));
                        string WebserverAddress = _configuration.GetValue<string>("WebServerWebAddress");
                        var Message = $"Adresse: {WebserverAddress}";

                        if (user == null)
                        {
                            var userRegistrationService = scope.ServiceProvider.GetRequiredService<IUserRegistrationService>();
                            var Password = userRegistrationService.RegisterUser(arg.Author.Username, arg.Author.Id, false);
                            Message += $" Password: {Password}";
                            _logger.LogInformation("Registered new user {UserName} via !wilnsb command", arg.Author.Username);
                        }
                        arg.Author.SendMessageAsync(Message);
                    }
                }
            }
            return Task.CompletedTask;
        }

        private bool isMessage(SocketMessage arg)
        {
            if (arg.Content.StartsWith('!'))
            {
                return false;
            }
            //aka does not contain Command
            return true;
        }

        private async Task<bool> Restart()
        {
            await _client.DisposeAsync();
            InitClient();
            InitEvents();
            _logger.LogInformation("Restarted Discord client");
            return true;
        }
        private async Task<bool> Connect()
        {
            var Credential = GetCredentials();
            if (Credential != null)
            {
                await _client.LoginAsync(TokenType.Bot, Credential.Token);
                await _client.StartAsync();
                _logger.LogInformation("Connected Discord client as bot");
                return true;
            }
            _logger.LogError("Failed to connect Discord client: No credentials found");
            return false;
        }

        private DiscordCredentials GetCredentials()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var creds = context.DiscordCredentials.FirstOrDefault();
            if (creds == null)
            {
                _logger.LogError("No Discord credentials found in database");
            }
            return creds;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting DiscordChat background service");
            if (await Connect())
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(5000, stoppingToken);
                }
            }
            _logger.LogInformation("Stopping DiscordChat background service");
            return;
        }
    }
}
