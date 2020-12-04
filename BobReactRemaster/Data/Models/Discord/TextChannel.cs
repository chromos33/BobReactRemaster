using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace BobReactRemaster.Data.Models.Discord
{
    [Table("DiscordTextChannels")]
    public class TextChannel
    {
        [Key] public int id { get; set; }
        public ulong ChannelID { get; set; }

        public string Name { get; set; }

        public bool IsPermanentRelayChannel { get; set; }

        public bool IsRelayChannel { get; set; }

        public string Guild { get; set; }

        public int? LiveStreamID {get; set; }
        private TextChannel()
        {
        }

        public TextChannel(ulong channelId,string name,string guildname)
        {
            ChannelID = channelId;
            Name = name;
            IsPermanentRelayChannel = false;
            IsRelayChannel = true;
            Guild = guildname;
        }

        public void Update(string channelName,string guildName)
        {
            Name = channelName;
            Guild = guildName;
        }
    }
}