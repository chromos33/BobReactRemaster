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
        private DateTime NextExecutionDate;
        private bool removalQueued;
        public EventCreationTask(IServiceScopeFactory factory,DateTime ExecutionDate)
        {
            this.factory = factory;
            NextExecutionDate = ExecutionDate;
        }
        public bool Executable()
        {
            return DateTime.Compare(NextExecutionDate, DateTime.Now) < 0;
        }

        public void Execute()
        {
            //TODO Get DBContext from Factory and Trigger the Create Function on MeetingTemplate

            //Repeat once a week
            NextExecutionDate = NextExecutionDate.AddDays(7);
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
