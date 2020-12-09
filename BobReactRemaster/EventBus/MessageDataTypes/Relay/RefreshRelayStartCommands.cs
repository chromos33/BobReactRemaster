using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.BaseClasses;

namespace BobReactRemaster.EventBus.MessageDataTypes.Relay
{
    public class RefreshQuoteCommands: BaseMessageData
    {
        public LiveStream Stream;

        public RefreshQuoteCommands(LiveStream stream)
        {
            Stream = stream;
        }
    }
}
