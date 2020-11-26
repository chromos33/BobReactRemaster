using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BobReactRemaster.Services.Scheduler.Tasks
{
    public class StreamUptimeRelayTask : IScheduledTask
    {
        private IServiceScopeFactory factory;
        private int? IntervalID;
        private DateTime NextExecutionDate;
        private bool removalQueued;
        private readonly int interval;
        private DateTime StreamStart;
        private LiveStream stream;
        public StreamUptimeRelayTask(LiveStream stream, DateTime streamStart, int Interval)
        {
            this.stream = stream;
            StreamStart = streamStart;
            IntervalID = 0;
            interval = Interval;
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
                string Message = stream.GetUptimeMessage(StreamStart);
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
            return false;
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
