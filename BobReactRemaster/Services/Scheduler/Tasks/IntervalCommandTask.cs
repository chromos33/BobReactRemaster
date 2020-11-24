using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Stream;
using Microsoft.Extensions.DependencyInjection;

namespace BobReactRemaster.Services.Scheduler.Tasks
{
    public class IntervalCommandTask: IScheduledTask
    {
        private IServiceScopeFactory factory;
        private int? IntervalID;
        private DateTime NextExecutionDate;
        private bool removalQueued;
        private int interval;
        private LiveStream stream;
        public IntervalCommandTask(int ID,int Interval, LiveStream stream)
        {
            IntervalID = ID;
            interval = Interval;
            this.stream = stream;
            NextExecutionDate = DateTime.Now.Add(TimeSpan.FromMinutes(interval));
        }
        public bool Executable()
        {
            return DateTime.Compare(NextExecutionDate, DateTime.Now) < 0;
        }

        public void Execute()
        {
            Console.WriteLine("IntervalCommandExecuted");
        }

        public bool Removeable()
        {
            return removalQueued;
        }

        public void QueueRemoval()
        {
            removalQueued = true;
        }

        public void setScopeFactory(IServiceScopeFactory Factory)
        {
            factory = Factory;
        }

        public bool isThisTask(int ID)
        {
            return ID == IntervalID;
        }

        public bool isThisTask(IScheduledTask Task)
        {
            return Task.GetID() == IntervalID;
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
    }
}
