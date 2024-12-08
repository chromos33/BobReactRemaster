using System.Collections.Generic;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.Services.Chat.GeneralClasses;

namespace BobReactRemaster.Services.Chat
{
    public class RelayService: IRelayService
    {
        private readonly IMessageBus _messageBus;
        public RelayService(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }
        //A LiveStream in the List requires a reference to RelayChannel otherwise nothing will be Relayed because that signals no Relay enabled
        public void RelayMessage(RelayMessage MessageObject,List<LiveStream> LiveStreamsWithRelayChannelDataIncluded)
        {
            var Messages = MessageObject.GetMessageBusMessages(LiveStreamsWithRelayChannelDataIncluded);
            foreach (var Message in Messages)
            {
                if (Message != null)
                {
                    _messageBus.Publish(Message);
                }
            }
        }
    }
}
