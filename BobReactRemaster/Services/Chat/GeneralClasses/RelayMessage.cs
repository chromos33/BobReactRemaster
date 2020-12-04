using System.Collections.Generic;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.BaseClasses;

namespace BobReactRemaster.Services.Chat.GeneralClasses
{
    public abstract class RelayMessage
    {
        public string Message { get; set; }

        public abstract List<BaseMessageData> GetMessageBusMessages(List<LiveStream> LiveStreams);
    }
}
