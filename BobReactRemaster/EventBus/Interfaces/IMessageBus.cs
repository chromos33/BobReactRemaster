using System;
using BobReactRemaster.EventBus.BaseClasses;

namespace BobReactRemaster.EventBus.Interfaces
{
    public interface IMessageBus
    {
        public void RegisterToEvent<TEventBase>(Action<TEventBase> test) where TEventBase : BaseMessageData;
        public void Publish(BaseMessageData message);
    }
}