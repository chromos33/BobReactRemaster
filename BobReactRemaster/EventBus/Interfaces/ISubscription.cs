using BobReactRemaster.EventBus.BaseClasses;

namespace BobReactRemaster.EventBus
{
    internal interface ISubscription
    {
        void Publish(BaseMessageData data);
    }
}