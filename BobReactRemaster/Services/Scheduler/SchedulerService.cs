using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Meetings;
using BobReactRemaster.Data.Models.Stream.Twitch;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.EventBus.MessageDataTypes;
using BobReactRemaster.Services.Chat;
using BobReactRemaster.Services.Scheduler.Tasks;
using Duende.IdentityServer.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BobReactRemaster.Services.Scheduler
{
    public class SchedulerService : BackgroundService
    {
        private List<IScheduledTask> Tasks = new List<IScheduledTask>();
        private readonly IServiceScopeFactory _scopeFactory;

        public SchedulerService(IServiceScopeFactory scopeFactory, ILogger<SchedulerService> logger)
            : base(logger)
        {
            _scopeFactory = scopeFactory;
            Setup();
            _logger.LogInformation("SchedulerService initialized and tasks set up.");
        }

        private void Setup()
        {
            var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            #region TwitchCredentials

            foreach (TwitchCredential cred in context.TwitchCredentials.AsEnumerable().Where(x => !x.RefreshToken.IsNullOrEmpty()))
            {
                Tasks.Add(new TwitchOAuthRefreshTask(cred.ExpireDate.Subtract(TimeSpan.FromMinutes(5)), cred.id, _scopeFactory));
                _logger.LogInformation("Added TwitchOAuthRefreshTask for credential ID {CredentialId}", cred.id);
            }
            #endregion
            #region MeetingTasks
            foreach (MeetingTemplate template in context.MeetingTemplates.AsQueryable().Include(x => x.Dates).Include(y => y.LiveMeetings).Where(x => x.Dates.Count > 0))
            {
                Tasks.Add(new EventCreationTask(template.ID, _scopeFactory, template.NextCreateDateTime()));
                _logger.LogInformation("Added EventCreationTask for meeting template ID {TemplateId}", template.ID);
            }
            Tasks.Add(new EventReminderTask(_scopeFactory));
            _logger.LogInformation("Added EventReminderTask.");
            #endregion
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SchedulerService background execution started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                await ExecuteOnceAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
            _logger.LogInformation("SchedulerService background execution stopped.");
        }

        private async Task ExecuteOnceAsync(CancellationToken stoppingToken)
        {
            foreach (IScheduledTask Task in Tasks.Where(x => x.Executable()))
            {
                try
                {
                    Task.Execute();
                    _logger.LogInformation("Executed scheduled task: {TaskType}", Task.GetType().Name);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception while executing scheduled task: {TaskType}", Task.GetType().Name);
                }
            }
            int removed = Tasks.RemoveAll(x => x.Removeable());
            if (removed > 0)
            {
                _logger.LogInformation("Removed {Count} completed or removable tasks.", removed);
            }
        }

        public bool ContainsTask(IScheduledTask Task)
        {
            if (Tasks.IsNullOrEmpty())
            {
                return false;
            }
            return Tasks.Any(x => x.isThisTask(Task));
        }

        public void AddTask(IScheduledTask Task)
        {
            if (Tasks.All(x => !x.isThisTask(Task)))
            {
                Task.setScopeFactory(_scopeFactory);
                Tasks.Add(Task);
                _logger.LogInformation("Added new scheduled task: {TaskType}", Task.GetType().Name);
            }
        }

        public void UpdateTask(IScheduledTask Task)
        {
            var existing = Tasks.FirstOrDefault(x => x.isThisTask(Task));
            if (existing != null)
            {
                existing.Update(Task);
                _logger.LogInformation("Updated scheduled task: {TaskType}", Task.GetType().Name);
            }
        }
    }
}
