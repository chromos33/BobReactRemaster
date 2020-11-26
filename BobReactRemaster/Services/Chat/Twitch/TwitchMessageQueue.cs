using System;
using System.Collections.Generic;
using BobReactRemaster.Services.Scheduler;

namespace BobReactRemaster.Services.Chat.Twitch
{
    public class TwitchMessageQueue : MessageQueue
    {
        public string ChannelName { get; private set; }

        private List<IScheduledTask> Tasks = new List<IScheduledTask>();
        public TwitchMessageQueue(string ChannelName, bool isModerator, TimeSpan messageRate) : base(isModerator, messageRate)
        {
            this.ChannelName = ChannelName;
        }

        public void AddTask(IScheduledTask task)
        {
            Tasks.Add(task);
        }

        public List<IScheduledTask> getTasks()
        {
            return Tasks;
        }
    }
}
