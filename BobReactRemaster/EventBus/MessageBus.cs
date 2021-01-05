using BobReactRemaster.EventBus.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using BobReactRemaster.EventBus.Interfaces;

namespace BobReactRemaster.EventBus
{
    public class MessageBus : IMessageBus
    {
        private List<ISubscription> Events;

        public MessageBus()
        {
            Events = new List<ISubscription>();
        }

        public void Publish(BaseMessageData message)
        {
            foreach (var Event in Events.Where(x =>
                x.GetType().GenericTypeArguments.First().Name == message.GetType().Name))
            {
                Event.Publish(message);
            }
        }

        public void RegisterToEvent<TEventBase>(Action<TEventBase> action) where TEventBase : BaseMessageData
        {
            try
            {
                Events.Add(new Subscription<TEventBase>(action));
            }
            catch (Exception e)
            {
                    Console.WriteLine(e);
                    throw;
            }

            Console.WriteLine("test");
        }
    }
}