using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Services.Chat.Twitch
{
    public class TwitchMessageQueue : MessageQueue
    {
        public string ChannelName { get; private set; }
        public TwitchMessageQueue(string ChannelName, bool isModerator, TimeSpan messageRate) : base(isModerator, messageRate)
        {
            this.ChannelName = ChannelName;
        }
    }
}
