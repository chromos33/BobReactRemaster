using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.BaseClasses;
using BobReactRemaster.EventBus.Interfaces;

namespace BobReactRemaster.EventBus.MessageDataTypes.Relay.Twitch
{
    public class RelayStartedMessageData: BaseMessageData
    {
        public LiveStream Stream;

        public RelayStartedMessageData(LiveStream stream)
        {
            Stream = stream;
        }
        
    }
}
