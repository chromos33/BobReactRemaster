using System;
using System.Collections.Generic;
using System.Text;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.Services.Chat.Twitch;
using BobReactRemaster.Services.Scheduler.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;

namespace BobReactRemaster.Tests.Services.Scheduler.Tasks
{
    class TwitchOAuthRefreshTaskTests
    {
        [Test]
        public void Executable_ValidTaskWithDateInPast_TaskIsExecutable()
        {
            TwitchOAuthRefreshTask task = new TwitchOAuthRefreshTask(DateTime.Now.Subtract(TimeSpan.FromSeconds(1)),5,null);
            Assert.IsTrue(task.Executable());
        }
        [Test]
        public void Executable_ValidTaskWithDateInFuture_TaskIsNotExecutable()
        {
            TwitchOAuthRefreshTask task = new TwitchOAuthRefreshTask(DateTime.Now.Add(TimeSpan.FromMinutes(5)),5,null);
            Assert.IsFalse(task.Executable());
        }
    }
}
