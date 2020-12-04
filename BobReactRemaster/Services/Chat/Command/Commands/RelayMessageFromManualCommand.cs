using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.BaseClasses;
using BobReactRemaster.EventBus.MessageDataTypes;
using BobReactRemaster.Services.Chat.GeneralClasses;
using TwitchLib.PubSub.Models.Responses.Messages;

namespace BobReactRemaster.Services.Chat.Command.Commands
{
    public class RelayMessageFromManualCommand: RelayMessage
    {
        private LiveStream stream;
        private string message;
        public RelayMessageFromManualCommand(LiveStream Stream,string Message)
        {
            stream = Stream;
            message = Message;
        }
        public override List<BaseMessageData> GetMessageBusMessages(List<LiveStream> LiveStreams)
        {
            List<BaseMessageData> List = new List<BaseMessageData>();
            if (stream != null)
            {
                List.Add(stream.getRelayMessageData(message));
            }
            return List;
        }
    }
}
