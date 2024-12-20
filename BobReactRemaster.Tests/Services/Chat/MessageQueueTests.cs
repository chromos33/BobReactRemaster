﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BobReactRemaster.Services.Chat.Twitch;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace BobReactRemaster.Tests.Services.Chat.Twitch
{
    class MessageQueueTests
    {
        [Test]
        public void NextQueuedMessage_EmptyList_ReturnsNull()
        {
            MessageQueue queue = new MessageQueue(false,TimeSpan.FromMilliseconds(0));
            ClassicAssert.IsNull(queue.NextQueuedMessage());
        }
        [Test]
        public void AddMessage_NextQueuedMessage_ReturnsMessage()
        {
            MessageQueue queue = new MessageQueue( false,TimeSpan.FromMilliseconds(0));
            string testmessage = "test";
            queue.AddMessage(testmessage);
            ClassicAssert.AreEqual(testmessage,queue.NextQueuedMessage());
        }
        [Test]
        public void AddMessages_NextQueuedMessage_ReturnsFirstMessage()
        {
            MessageQueue queue = new MessageQueue( false, TimeSpan.FromMilliseconds(0));
            string testmessage = "test";
            queue.AddMessage(testmessage);
            queue.AddMessage("invalidresult");
            ClassicAssert.AreEqual(testmessage, queue.NextQueuedMessage());
        }
        [Test]
        public void NextQueuedMessage_AddedRateLimiter_ReturnsNullOnSecondCall()
        {
            MessageQueue queue = new MessageQueue( false, TimeSpan.FromMilliseconds(2000));
            string testmessage = "test";
            queue.AddMessage(testmessage);
            queue.AddMessage("invalidresult");
            ClassicAssert.AreEqual(testmessage, queue.NextQueuedMessage());
            ClassicAssert.IsNull(queue.NextQueuedMessage());
        }
        [Test]
        public void NextQueuedMessageWithRateLimiter_WaitingSufficentTime_ReturnsMessages()
        {
            int LimiterTime = 2000;
            MessageQueue queue = new MessageQueue(false, TimeSpan.FromMilliseconds(LimiterTime));
            string testmessage = "test";
            string testmessage2 = "test2";
            queue.AddMessage(testmessage);
            queue.AddMessage(testmessage2);
            ClassicAssert.AreEqual(testmessage, queue.NextQueuedMessage());
            Thread.Sleep(LimiterTime);
            ClassicAssert.AreEqual(testmessage2, queue.NextQueuedMessage());
        }
        [Test]
        public void NextQueuedMessageWithRateLimiter_WaitingInsufficentTime_ReturnsMessages()
        {
            int LimiterTime = 2000;
            MessageQueue queue = new MessageQueue(false, TimeSpan.FromMilliseconds(LimiterTime));
            string testmessage = "test";
            string testmessage2 = "test2";
            queue.AddMessage(testmessage);
            queue.AddMessage(testmessage2);
            ClassicAssert.AreEqual(testmessage, queue.NextQueuedMessage());
            Thread.Sleep(LimiterTime - 100);
            ClassicAssert.IsNull(queue.NextQueuedMessage());
        }

        [Test]
        public void EnableModeratorMode_ChangesState()
        {
            int LimiterTime = 2000;
            MessageQueue queue = new MessageQueue(false, TimeSpan.FromMilliseconds(LimiterTime));
            ClassicAssert.AreEqual(false,queue.isModerator);
            queue.EnableModeratorMode();
            ClassicAssert.AreEqual(true,queue.isModerator);
            
        }
    }
}
