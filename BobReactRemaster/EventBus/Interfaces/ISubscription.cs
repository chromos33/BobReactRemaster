using BobReactRemaster.EventBus.BaseClasses;

namespace BobReactRemaster.EventBus.Interfaces
{
    internal interface ISubscription
    {
        void Publish(BaseMessageData data);
    }
}