using BobReactRemaster.Data;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.Services.Chat.Discord;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Meetings;
using TwitchLib.Client.Models;

namespace BobReactRemaster.Services.Scheduler.Tasks
{
    public class EventReminderTask : IScheduledTask
    {
        private IServiceScopeFactory factory;
        private int? IntervalID;
        private DateTime NextExecutionDate;
        private int TemplateID;
        private bool removalQueued;
        private readonly IMessageBus Bus;
        public EventReminderTask(IServiceScopeFactory factory)
        {
            this.factory = factory;
            NextExecutionDate = DateTime.Now;
            this.TemplateID = TemplateID;
            var scope = factory.CreateScope();
            Bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
        }
        public bool Executable()
        {
            return DateTime.Compare(NextExecutionDate, DateTime.Now) < 0;
        }

        public void Execute()
        {
            List<DiscordWhisperData> Output = new List<DiscordWhisperData>();
            var scope = factory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var Meetings = context.Meetings.Include(x => x.Subscriber).ThenInclude(x => x.Subscriber).Include(x => x.MeetingTemplate).AsQueryable().Where(x => !x.ReminderSent && x.ReminderDate < DateTime.Now);
            foreach (Meeting meeting in Meetings)
            {
                foreach (var Subscription in meeting.Subscriber.Where(x =>
                             x.State == Data.Models.Meetings.MeetingParticipationState.Undecided))
                {
                    if (Subscription.Subscriber != null && Subscription.Subscriber.DiscordUserName != null)
                    {
                        var WhisperData = Output.FirstOrDefault(x => x.MemberName == Subscription.Subscriber.DiscordUserName);
                        if (WhisperData == null)
                        {
                            WhisperData = new DiscordWhisperData(Subscription.Subscriber.DiscordUserName, Subscription.GetReminderMessage());
                            Output.Add(WhisperData);
                        }
                        else
                        {
                            WhisperData.AddToMessage(Subscription.GetReminderMessage());
                        }
                    }
                    
                }

                meeting.ReminderSent = true;
            }
            context.SaveChangesAsync();
            foreach (DiscordWhisperData EventData in Output)
            {
                Bus.Publish(EventData);
            }
            NextExecutionDate = DateTime.Now;
            /*

            var meetingTemplate = context.MeetingTemplates.Include(x => x.Dates).Include(x => x.LiveMeetings).ThenInclude(y => y.Subscriber).Include(x => x.ReminderTemplate).Include(x => x.Members).ThenInclude(x => x.RegisteredMember).First(x => x.ID == TemplateID);
            foreach(var LiveMeeting in meetingTemplate.LiveMeetings)
            {
                foreach(var Subscription in LiveMeeting.Subscriber.Where(x => x.State == Data.Models.Meetings.MeetingParticipationState.Undecided))
                {
                    var WhisperData = Output.FirstOrDefault(x => x.MemberName == Subscription.Subscriber.DiscordUserName);
                    if (WhisperData == null)
                    {
                        WhisperData = new DiscordWhisperData(Subscription.Subscriber.DiscordUserName,Subscription.GetReminderMessage());
                        Output.Add(WhisperData);
                    }
                    else
                    {
                        WhisperData.AddToMessage(Subscription.GetReminderMessage());
                    }
                }
            }
            foreach(DiscordWhisperData EventData in Output)
            {
                Bus.Publish(EventData);
            }
            QueueRemoval();
            */
        }

        public int? GetID()
        {
            return IntervalID;
        }

        public bool InitializeID(int ID)
        {
            if (IntervalID == null)
            {
                IntervalID = ID;
                return true;
            }
            return false;
        }

        public bool isRefreshableTask()
        {
            return false;
        }

        public void Update(IScheduledTask task)
        {
            //ignore nothing to update
        }

        public bool isThisTask(int ID)
        {
            return ID == IntervalID;
        }

        public bool isThisTask(IScheduledTask Task)
        {
            return Task.GetID() == IntervalID;
        }

        public void QueueRemoval()
        {
            removalQueued = true;
        }

        public bool Removeable()
        {
            return removalQueued;
        }

        public void setScopeFactory(IServiceScopeFactory Factory)
        {
            factory = Factory;
        }
    }
}
