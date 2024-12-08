using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using BobReactRemaster.EventBus;
using BobReactRemaster.EventBus.BaseClasses;
using NUnit.Framework.Legacy;

namespace BobRemastered.Tests.Models.EventBus
{
    class MessageBusTests
    {
        [Test]
        public void RegisterAndPublish_RegisteredType_EventTriggered()
        {
            MessageBus bus = new MessageBus();
            string payload = "Test";
            bus.RegisterToEvent<TestMessageData>((x) =>
            {
                ClassicAssert.AreEqual(x.Payload, payload);
            });
            bus.Publish(new TestMessageData(payload));
            
        }
    }
    public class TestMessageData: BaseMessageData
    {
        public string Payload;

        public TestMessageData(string payload)
        {
            Payload = payload;
        }

    }
}
