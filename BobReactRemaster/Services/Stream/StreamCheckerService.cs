using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.Services.Chat;
using BobReactRemaster.Services.Stream.Twitch;
using Microsoft.Extensions.DependencyInjection;

namespace BobReactRemaster.Services.Stream
{
    public class StreamCheckerService  : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private List<IStreamChecker> StreamChecker { get; set; }
        private System.Timers.Timer timer { get; set; }
        private IMessageBus MessageBus { get; set; }
        public StreamCheckerService(IServiceScopeFactory scopeFactory, IMessageBus messageBus)
        {
            _scopeFactory = scopeFactory;
            MessageBus = messageBus;
            StreamChecker = new List<IStreamChecker>();
        }

        private void InitializeStreamChecker()
        {
            StreamChecker.Add(new TwitchStreamChecker(MessageBus,_scopeFactory));
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            InitializeStreamChecker();
            InitializeClock();
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);
            }

            return;
        }

        private void InitializeClock()
        {
            timer = new System.Timers.Timer(10000);
            timer.Elapsed += (sender, args) => TriggerStreamCheck();
            timer.Start();
        }

        private void TriggerStreamCheck()
        {
            foreach (IStreamChecker streamChecker in StreamChecker)
            {
                var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                streamChecker.CheckStreams();
            }
        }
    }
}
