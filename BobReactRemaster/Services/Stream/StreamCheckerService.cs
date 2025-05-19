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
using Microsoft.Extensions.Logging;

namespace BobReactRemaster.Services.Stream
{
    public class StreamCheckerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private List<IStreamChecker> StreamChecker { get; set; }
        private System.Timers.Timer timer { get; set; }
        private IMessageBus MessageBus { get; set; }

        public StreamCheckerService(
            IServiceScopeFactory scopeFactory,
            IMessageBus messageBus,
            ILogger<StreamCheckerService> logger
        ) : base(logger)
        {
            _scopeFactory = scopeFactory;
            MessageBus = messageBus;
            StreamChecker = new List<IStreamChecker>();
            _logger.LogInformation("StreamCheckerService initialized.");
        }

        private void InitializeStreamChecker()
        {
            StreamChecker.Add(new TwitchStreamChecker(MessageBus, _scopeFactory));
            _logger.LogInformation("Initialized TwitchStreamChecker.");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("StreamCheckerService background execution started.");
            InitializeStreamChecker();
            InitializeClock();
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);
            }
            _logger.LogInformation("StreamCheckerService background execution stopped.");
        }

        private void InitializeClock()
        {
            timer = new System.Timers.Timer(60000);
            timer.Elapsed += (sender, args) => TriggerStreamCheck();
            timer.Start();
            _logger.LogInformation("StreamCheckerService timer started (interval: 60s).");
        }

        private void TriggerStreamCheck()
        {
            _logger.LogDebug("Triggering stream checks for {Count} checkers.", StreamChecker.Count);
            foreach (IStreamChecker streamChecker in StreamChecker)
            {
                try
                {
                    streamChecker.CheckStreams();
                    _logger.LogInformation("Stream check executed for {CheckerType}.", streamChecker.GetType().Name);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception during stream check for {CheckerType}.", streamChecker.GetType().Name);
                }
            }
        }
    }
}
