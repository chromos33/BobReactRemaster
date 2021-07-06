using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Services.Scheduler.Tasks
{
    public class EventReminderTask : IScheduledTask
    {
        private IServiceScopeFactory factory;
        private int? IntervalID;
        private DateTime NextExecutionDate;
        private bool removalQueued;
        public EventReminderTask()
        {

        }
        public bool Executable()
        {
            throw new NotImplementedException();
        }

        public void Execute()
        {
            throw new NotImplementedException();
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
