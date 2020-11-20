using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Services.Chat.Commands.Base;

namespace BobReactRemaster.Services.Chat.Command.Messages
{
    public class TwitchCommandMessage: CommandMessage
    {
        private string Channel;
        private string Author;
        public TwitchCommandMessage(string Message, string channel, string author)
        {
            Channel = channel;
            Author = author;
            this.Message = Message;
        }

    }
}
