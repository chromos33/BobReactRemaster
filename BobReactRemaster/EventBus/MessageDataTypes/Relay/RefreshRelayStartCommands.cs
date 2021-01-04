using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.BaseClasses;

namespace BobReactRemaster.EventBus.MessageDataTypes.Relay
{
    public class QuoteCommandAdded: BaseMessageData
    {
        public LiveStream Stream;
        public Quote Quote;

        public QuoteCommandAdded(LiveStream stream, Quote quote)
        {
            Stream = stream;
            Quote = quote;
        }
    }
}
