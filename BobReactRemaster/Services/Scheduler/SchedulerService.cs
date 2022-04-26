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

namespace BobReactRemaster.Services.Scheduler
{
    public class SchedulerService : BackgroundService
    {
        private List<IScheduledTask> Tasks = new List<IScheduledTask>();
        private readonly IServiceScopeFactory _scopeFactory;

        public SchedulerService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            Setup();
        }
        /* redundant or some oversight remove later if really not used
        private void AddTwitchOAuthRefreshTask(TwitchOAuthedMessageData obj)
        {
            AddTask(new TwitchOAuthRefreshTask(obj.ExpireTime,obj.ID,obj.ServiceScopeFactory));
        }
        */
        private void Setup()
        {

            var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            #region TwitchCredentials

            foreach (TwitchCredential cred in context.TwitchCredentials.AsEnumerable().Where(x => !x.RefreshToken.IsNullOrEmpty()))
            {
                Tasks.Add(new TwitchOAuthRefreshTask(cred.ExpireDate.Subtract(TimeSpan.FromMinutes(5)),cred.id,_scopeFactory));
            }
            #endregion
            #region MeetingTasks
            foreach (MeetingTemplate template in context.MeetingTemplates.AsQueryable().Include(x => x.Dates).Include(y => y.LiveMeetings).Where(x => x.Dates.Count > 0))
            {
                Tasks.Add(new EventCreationTask(template.ID, _scopeFactory, template.NextCreateDateTime()));
                Tasks.Add(new EventReminderTask(template.ID, _scopeFactory, template.NextReminderDateTime()));
            }
            #endregion
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ExecuteOnceAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            } 
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task ExecuteOnceAsync(CancellationToken stoppingToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            foreach (IScheduledTask Task in Tasks.Where(x => x.Executable()))
            {
                Task.Execute();
            }
            Tasks.RemoveAll(x => x.Removeable());
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
            }
        }

        public void UpdateTask(IScheduledTask Task)
        {
            Tasks.FirstOrDefault(x => x.isThisTask(Task))?.Update(Task);
        }
    }
}
