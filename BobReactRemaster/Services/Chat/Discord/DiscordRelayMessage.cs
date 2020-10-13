using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.EventBus.BaseClasses;
using BobReactRemaster.Services.Chat.GeneralClasses;

namespace BobReactRemaster.Services.Chat.Discord
{
    public class DiscordRelayMessage: RelayMessage
    {
        private string Channel { get; set; }
        private string Guild { get; set; }

        public DiscordRelayMessage( string guild, string channel, string message)
        {
            Channel = channel;
            Guild = guild;
            Message = message;
        }
        public override List<BaseMessageData> GetMessageBusMessages(ApplicationDbContext context)
        {
            throw new NotImplementedException();
        }
    }
}
