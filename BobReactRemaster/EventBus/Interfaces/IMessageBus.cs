using BobReactRemaster.EventBus.BaseClasses;
using System;

namespace BobReactRemaster.EventBus
{
    public interface IMessageBus
    {
        public void RegisterToEvent<TEventBase>(Action<TEventBase> test) where TEventBase : BaseMessageData;
        public void Publish(BaseMessageData message);
    }
}
