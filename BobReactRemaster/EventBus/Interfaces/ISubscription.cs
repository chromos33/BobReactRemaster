using BobReactRemaster.EventBus.BaseClasses;

namespace BobReactRemaster.EventBus
{
    interface ISubscription
    {
        void Publish(BaseMessageData data);
    }
}
