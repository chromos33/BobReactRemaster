using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Meetings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace BobReactRemaster.Services.Scheduler.Tasks
{
    public class EventCreationTask : IScheduledTask
    {
        private IServiceScopeFactory factory;
        private int? IntervalID;
        private int TemplateID;
        private DateTime NextExecutionDate;
        private bool removalQueued;
        private SchedulerService scheduler;
        public EventCreationTask(int TemplateID, IServiceScopeFactory factory,DateTime ExecutionDate)
        {
            this.factory = factory;
            NextExecutionDate = ExecutionDate;
            this.TemplateID = TemplateID;
        }
        public bool Executable()
        {
            return DateTime.Compare(NextExecutionDate, DateTime.Now) < 0;
        }

        public void Execute()
        {
            //Repeat once a day and prevent exectuting twice
            NextExecutionDate = NextExecutionDate.AddDays(1);
            //TODO Get DBContext from Factory and Trigger the Create Function on MeetingTemplate
            var scope = factory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var tmp = scope.ServiceProvider.GetServices<IHostedService>()
                .FirstOrDefault(x => x.GetType() == typeof(SchedulerService));
            if (tmp != null)
            {
                scheduler = (SchedulerService)tmp;
            }

            CreateEvents(context);
            DeletePassedEvents(context);
           
            

            
            
        }

        private void DeletePassedEvents(ApplicationDbContext context)
        {
            var meetingTemplate = context.MeetingTemplates.Include(x => x.Dates).Include(x => x.LiveMeetings).Include(x => x.ReminderTemplate).Include(x => x.Members).ThenInclude(x => x.RegisteredMember).First(x => x.ID == TemplateID);

            List<Meeting> toRemove = new List<Meeting>();
            foreach (Meeting meeting in meetingTemplate.LiveMeetings.Where(x => DateTime.Compare(x.MeetingDateStart,DateTime.Now) < 0))
            {
                toRemove.Add(meeting);
            }
            foreach (Meeting meeting in toRemove)
            {
                context.Meetings.Remove(meeting);
            }
            context.SaveChanges();
        }

        public void CreateEvents(ApplicationDbContext context)
        {
            var meetingTemplate = context.MeetingTemplates.Include(x => x.Dates).Include(x => x.LiveMeetings).Include(x => x.ReminderTemplate).Include(x => x.Members).ThenInclude(x => x.RegisteredMember).First(x => x.ID == TemplateID);
            var reminderTaskCreated = false;
            foreach (Meeting meeting in meetingTemplate.CreateMeetingsForNextWeek(DateTime.Today))
            {
                bool meetingExists = context.Meetings.Any(m =>
                    m.MeetingDateStart == meeting.MeetingDateStart &&
                    m.MeetingDateEnd == meeting.MeetingDateEnd &&
                    m.MeetingTemplateID == meeting.MeetingTemplateID);

                if (!meetingExists)
                {
                    context.Meetings.Add(meeting);
                    /* changed reminderTask to just check once every minute if there's a meeting that needs a reminder
                    if (scheduler != null && !reminderTaskCreated)
                    {
                        scheduler.AddTask(new EventReminderTask(meeting.MeetingTemplateID, factory, meeting.ReminderDate));
                    }*/
                }

            }
            context.SaveChanges();
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
            //nothing to update here
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
