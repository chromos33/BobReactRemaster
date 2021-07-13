using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Meetings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Services.Scheduler.Tasks
{
    public class EventCreationTask : IScheduledTask
    {
        private IServiceScopeFactory factory;
        private int? IntervalID;
        private int TemplateID;
        private DateTime NextExecutionDate;
        private bool removalQueued;
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
            //Repeat once a week and prevent exectuting twice
            NextExecutionDate = NextExecutionDate.AddDays(7);
            //TODO Get DBContext from Factory and Trigger the Create Function on MeetingTemplate
            var scope = factory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var meetingTemplate = context.MeetingTemplates.Include(x => x.Dates).Include(x => x.LiveMeetings).Include(x => x.ReminderTemplate).Include(x => x.Members).ThenInclude(x => x.RegisteredMember).First(x => x.ID == TemplateID);
            
            foreach(Meeting meeting in meetingTemplate.CreateMeetingsForNextWeek(DateTime.Today))
            {
                context.Meetings.Add(meeting);
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
