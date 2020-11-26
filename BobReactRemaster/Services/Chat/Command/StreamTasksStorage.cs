using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Services.Scheduler;
using BobReactRemaster.Services.Scheduler.Tasks;

namespace BobReactRemaster.Services.Chat.Command
{
    public class StreamTasksStorage
    {
        public string StreamName;
        public List<IScheduledTask> Tasks = new List<IScheduledTask>();

        public StreamTasksStorage(string streamname)
        {
            StreamName = streamname;
        }
        public void StopTasks()
        {
            foreach (var Task in Tasks)
            {
                Task.QueueRemoval();
            }
        }

        public void ClearRefreshableTasks()
        {
            foreach (var Task in Tasks.Where(x => x.isRefreshableTask()))
            {
                Task.QueueRemoval();
            }
            Tasks.RemoveAll(x => x.isRefreshableTask());
        }

        public void AddTask(IScheduledTask task)
        {
            Tasks.Add(task);
        }

    }
}
