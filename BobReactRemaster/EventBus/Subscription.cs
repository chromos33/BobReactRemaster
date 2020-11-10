using BobReactRemaster.EventBus.BaseClasses;
using System;
using BobReactRemaster.EventBus.Interfaces;

namespace BobReactRemaster.EventBus
{
    public class Subscription<TBaseMessageData> : ISubscription where TBaseMessageData : BaseMessageData
    {
        private readonly Action<TBaseMessageData> _action;

        public Subscription(Action<TBaseMessageData> action)
        {
            _action = action;
        }

        public void Publish(BaseMessageData content)
        {
            try
            {
                _action.Invoke(content as TBaseMessageData);
            }
            catch (InvalidOperationException e)
            {
                //Empty Receiver List did not find a way to prevent this or find out if this is the case
            }
                
            
        }
    }
}