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

namespace BobReactRemaster.Services.Chat.Discord
{
    public class DiscordChat : BackgroundService
    {
        private DiscordSocketClient _client;
        private IMessageBus MessageBus;
        private readonly IServiceScopeFactory _scopeFactory;
        private IRelayService _relayService;
        private readonly IConfiguration _configuration;
        public DiscordChat(IMessageBus messageBus, IServiceScopeFactory scopeFactory, IRelayService relayService, IConfiguration configuration)
        {
            InitClient();
            InitEvents();
            MessageBus = messageBus;
            _scopeFactory = scopeFactory;
            _relayService = relayService;
            _configuration = configuration;
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
            var member =  context.Members.First(x => x.DiscordUserName == obj.MemberName);
            if(member != null && obj.Message != "")
            {
                var chatmember = _client.GetUser(member.DiscordUserName, member.DiscordDiscriminator);
                chatmember.SendMessageAsync(obj.Message);
            }
        }

        private void StreamCreated(StreamCreatedMessageData obj)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            foreach (Member member in context.Members.AsEnumerable().Where(x => x.canBeFoundOnDiscord() ))
            {
                string message = obj.Stream.GetSubscriptionCreatedMessage();
                _client.GetUser(member.DiscordUserName, member.DiscordDiscriminator).SendMessageAsync(message);
            }
        }

        internal SocketGuildUser GetMemberByName(string userName)
        {
            var users = _client.Guilds.Where(x => x.Name == "Deathmic").FirstOrDefault()?.Users.Where(x => x.Username.ToLower() == userName.ToLower());
            if(users != null && users.Count() == 1)
            {
                return users.FirstOrDefault();
            }
            return null;
        }

        private void StreamStarted(TwitchStreamStartMessageData obj)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var stream = context.TwitchStreams.Include(x => x.RelayChannel).FirstOrDefault(x => x.StreamName.ToLower() == obj.Streamname.ToLower());
            string message = stream?.GetStreamStartedMessage(obj.Title);
            if (message != null)
            {
                foreach (Member member in context.Members.Include(x => x.StreamSubscriptions).ThenInclude(x => x.LiveStream).AsEnumerable().Where(x => x.canBeFoundOnDiscord() && x.HasSubscription(stream)))
                {
                    _client.GetUser(member.DiscordUserName, member.DiscordDiscriminator).SendMessageAsync(message);
                }
            }
        }

        private void RelayMessageReceived(DiscordRelayMessageData obj)
        {
            ((SocketTextChannel)_client.GetChannel(obj.DiscordChannelID)).SendMessageAsync(obj.Message);
        }

        private void InitClient()
        {
            var discordConfig = new DiscordSocketConfig { MessageCacheSize = 100 };
            discordConfig.AlwaysDownloadUsers = true;
            discordConfig.LargeThreshold = 250;
            _client = new DiscordSocketClient(discordConfig);
        }
        private void InitEvents()
        {
            _client.MessageReceived += MessageReceived;
            _client.ChannelCreated += ChannelCreated;
            _client.ChannelDestroyed += ChannelDestroyed;
            _client.ChannelUpdated += ChannelUpdated;
            _client.GuildAvailable += GuildJoined;
            _client.Connected += Connected;
        }

        private Task Connected()
        {
            Console.WriteLine("Discord Connected");
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
                        var channel = (SocketTextChannel) TextChannel;
                        var NewTextChannel = new TextChannel(channel.Id,channel.Name,channel.Guild.Name);
                        context.DiscordTextChannels.Add(NewTextChannel);
                        write = true;
                    }
                }
            }
            if (write)
            {
                context.SaveChanges();
            }
            return Task.CompletedTask;
        }

        private Task ChannelUpdated(SocketChannel arg1, SocketChannel arg2)
        {
            if (arg2.GetType() == typeof(SocketTextChannel))
            {
                var textchannel = (SocketTextChannel) arg2;
                if (textchannel.Category.Name.ToLower() == "community streams")
                {
                    using var scope = _scopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var channel = context.DiscordTextChannels.FirstOrDefault(x => x.ChannelID == arg2.Id);
                    if (channel != null)
                    {
                        var tmpchannel = (SocketTextChannel) arg2;
                        channel.Update(tmpchannel.Name,tmpchannel.Guild.Name);
                        context.SaveChanges();
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
            }

            return Task.CompletedTask;
        }

        private Task ChannelCreated(SocketChannel arg)
        {
            if (arg.GetType() == typeof(SocketTextChannel))
            {
                var TextChannel = (SocketTextChannel) arg;
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
                        string MessageWithUserName = $"{arg.Author.Username}:{arg.Content}";
                        _relayService.RelayMessage(new RelayMessageFromDiscord(

                            GuildName,
                            arg.Channel.Name,
                            MessageWithUserName
                        ),GetRelayLiveStreams());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
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
                            var Password = userRegistrationService.RegisterUser(arg.Author.Username, arg.Author.Discriminator);
                            Message += $" Password: {Password}";
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

        private async Task<bool> Connect()
        {
            var Credential = GetCredentials();
            if (Credential != null)
            {
                await _client.LoginAsync(TokenType.Bot, Credential.Token);
                await _client.StartAsync();
                return true;

            }

            return false;
        }

        private DiscordCredentials GetCredentials()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return context.DiscordCredentials.FirstOrDefault();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (await Connect())
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(5000, stoppingToken);
                }
            }
            return;
        }

    }
}
