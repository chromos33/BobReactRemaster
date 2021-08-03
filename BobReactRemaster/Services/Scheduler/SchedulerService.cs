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
using IdentityServer4.Extensions;
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

        private async Task ExecuteOnceAsync(CancellationToken stoppingToken)
        {
            foreach (IScheduledTask Task in Tasks.Where(x => x.Executable()))
            {
                Task.Execute();
            }

            Tasks.RemoveAll(x => x.Removeable());
        }

        public void AddTask(IScheduledTask Task)
        {
            if (Tasks.All(x => !x.isThisTask(Task)))
            {
                Task.setScopeFactory(_scopeFactory);
                Tasks.Add(Task);
            }
        }
    }
}
