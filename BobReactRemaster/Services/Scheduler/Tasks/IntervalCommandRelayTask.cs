using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BobReactRemaster.Services.Scheduler.Tasks
{
    public class IntervalCommandRelayTask: IScheduledTask
    {
        private IServiceScopeFactory factory;
        private int? IntervalID;
        private DateTime NextExecutionDate;
        private bool removalQueued;
        private int interval;
        private LiveStream stream;
        private string Message;
        public IntervalCommandRelayTask(int ID,int Interval, LiveStream stream, string message)
        {
            IntervalID = ID;
            interval = Interval;
            this.stream = stream;
            Message = message;
            NextExecutionDate = DateTime.Now.Add(TimeSpan.FromMinutes(interval));
        }
        public bool Executable()
        {
            return DateTime.Compare(NextExecutionDate, DateTime.Now) < 0;
        }

        public void Execute()
        {
            var scope = factory.CreateScope();
            IMessageBus bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
            if (bus != null)
            {
                bus.Publish(stream.getRelayMessageData(Message));
            }

        }

        public bool Removeable()
        {
            return removalQueued;
        }

        public void QueueRemoval()
        {
            removalQueued = true;
        }

        public bool isRefreshableTask()
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
