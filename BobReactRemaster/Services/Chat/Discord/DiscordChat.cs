using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.EventBus;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.EventBus.MessageDataTypes;
using BobReactRemaster.Services.Chat.Twitch;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BobReactRemaster.Services.Chat.Discord
{
    public class DiscordChat : BackgroundService
    {
        private DiscordSocketClient _client;
        private IMessageBus MessageBus;
        private readonly IServiceScopeFactory _scopeFactory;
        private RelayService _relayService;

        public DiscordChat(IMessageBus messageBus, IServiceScopeFactory scopeFactory, RelayService relayService)
        {
            InitClient();
            InitEvents();
            MessageBus = messageBus;
            _scopeFactory = scopeFactory;
            _relayService = relayService;
            SubscribeToBusEvents();
        }

        private void SubscribeToBusEvents()
        {
            MessageBus.RegisterToEvent<DiscordRelayMessageData>(RelayMessageReceived);
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
            return Task.CompletedTask;
        }

        private bool isMessage(SocketMessage arg)
        {
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
