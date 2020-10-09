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

        public DiscordChat(IMessageBus messageBus, IServiceScopeFactory scopeFactory)
        {
            InitClient();
            InitEvents();
            MessageBus = messageBus;
            _scopeFactory = scopeFactory;
            SubscribeToBusEvents();
        }

        private void SubscribeToBusEvents()
        {
            MessageBus.RegisterToEvent<RelayMessageFromStreamChat>(RelayMessageReceived);
        }

        private void RelayMessageReceived(RelayMessageFromStreamChat obj)
        {
            //TODO implement after DB has Discordservers, channels and stuff  so we can decide where to Relay Message to
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

        private async Task MessageReceived(SocketMessage arg)
        {
            //TODO: If Message should be relayed aka if originated from Server/Channel configured in a stream
            if (isMessageToBeRelayed(arg))
            {
                var message = ExtractRelayMessage(arg);
                RelayMessage(message);
            }
        }
        private bool isMessageToBeRelayed(SocketMessage arg)
        {
            //TODO Implement
            throw new NotImplementedException();
            return false;
        }
        private ChatMessageToStreamChat ExtractRelayMessage(SocketMessage arg)
        {
            return new ChatMessageToStreamChat() { Message = arg.Content, StreamName = "test", StreamType = "Test" };
        }
        private void RelayMessage(ChatMessageToStreamChat message)
        {
            MessageBus.Publish(message);
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
