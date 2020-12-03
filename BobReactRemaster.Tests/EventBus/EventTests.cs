using System;
using System.Collections.Generic;
using System.Text;
using BobReactRemaster.EventBus;
using NUnit.Framework;

namespace BobReactRemaster.Tests.EventBus
{
    class EventTests
    {
        [Test]
        public void EventConstructor_ValidPayload_CorrectState()
        {
            var Event = new Event<string>("test");
            Assert.AreEqual("test",Event.PayLoad);
        }
    }
}
