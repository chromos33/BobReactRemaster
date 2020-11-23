using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BobReactRemaster.Services.Scheduler.Tasks
{
    public class IntervalCommandTask: IScheduledTask
    {
        private IServiceScopeFactory factory;
        private int? IntervalID;
        private DateTime NextExecutionDate;
        public IntervalCommandTask()
        {

        }
        public bool Executable()
        {
            return DateTime.Compare(NextExecutionDate, DateTime.Now) < 0;
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public bool Removeable()
        {
            return true;
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
