using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.EventBus.MessageDataTypes;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BobReactRemaster.Services.Chat.Discord
{
    public class DiscordChat : BackgroundService
    {
        private DiscordSocketClient _client;
        private IMessageBus MessageBus;
        private readonly IServiceScopeFactory _scopeFactory;
        private RelayService _relayService;
        private readonly IConfiguration _configuration;
        public DiscordChat(IMessageBus messageBus, IServiceScopeFactory scopeFactory, RelayService relayService, IConfiguration configuration)
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
        }

        private void StreamStarted(TwitchStreamStartMessageData obj)
        {
            _client.Guilds.FirstOrDefault()?.TextChannels.FirstOrDefault()?.SendMessageAsync(obj.Streamname);
        }

        private void RelayMessageReceived(DiscordRelayMessageData obj)
        {
            _client
                .Guilds.FirstOrDefault(x =>
                    string.Equals(x.Name, obj.DiscordServer, StringComparison.CurrentCultureIgnoreCase))?
                .TextChannels.FirstOrDefault(x =>
                    string.Equals(x.Name, obj.DiscordChannel, StringComparison.CurrentCultureIgnoreCase))?
                .SendMessageAsync(obj.Message);
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
        }

        private Task MessageReceived(SocketMessage arg)
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
                    ));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
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
                        var userRegistrationService = scope.ServiceProvider.GetRequiredService<UserRegistrationService>();
                        var Password = userRegistrationService.RegisterUser(arg.Author.Username);
                        Message += $" Password: {Password}";
                    }
                    arg.Author.SendMessageAsync(Message);
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
