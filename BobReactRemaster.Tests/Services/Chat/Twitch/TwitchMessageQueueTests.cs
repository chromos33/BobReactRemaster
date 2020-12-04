using System;
using System.Collections.Generic;
using System.Text;
using BobReactRemaster.Services.Chat.Twitch;
using BobReactRemaster.Services.Scheduler.Tasks;
using NUnit.Framework;

namespace BobReactRemaster.Tests.Services.Chat.Twitch
{
    class TwitchMessageQueueTests
    {
        [Test]
        public void Constructor_ValidData_ValidObject()
        {
            var ChannelName = "Channel";
            var isModerator = true;
            var TimeSpan = System.TimeSpan.FromSeconds(1);
            TwitchMessageQueue queue = new TwitchMessageQueue(ChannelName,isModerator,TimeSpan);
            Assert.AreEqual(ChannelName,queue.ChannelName);
            Assert.AreEqual(isModerator,queue.isModerator);
        }
        [Test]
        public void AddTask_ValidTask_getTasksReturnsListWithTaskInIt()
        {
            var ChannelName = "Channel";
            var isModerator = true;
            var TimeSpan = System.TimeSpan.FromSeconds(1);
            TwitchMessageQueue queue = new TwitchMessageQueue(ChannelName, isModerator, TimeSpan);
            var task = new IntervalCommandRelayTask(5, 1, null, "message");
            queue.AddTask(task);
            Assert.Contains(task,queue.getTasks());
        }
        
    }
}
