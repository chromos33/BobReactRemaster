using System;
using System.Collections.Generic;
using System.Text;
using BobReactRemaster.EventBus;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace BobReactRemaster.Tests.EventBus
{
    class EventTests
    {
        [Test]
        public void EventConstructor_ValidPayload_CorrectState()
        {
            var Event = new Event<string>("test");
            ClassicAssert.AreEqual("test",Event.PayLoad);
        }
    }
}
