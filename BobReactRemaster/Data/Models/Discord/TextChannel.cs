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

        public TextChannel(SocketTextChannel channel)
        {
            ChannelID = channel.Id;
            Name = channel.Name;
            IsPermanentRelayChannel = false;
            IsRelayChannel = true;
            Guild = channel.Guild.Name;
        }

        public TextChannel(string Name)
        {
            this.Name = Name;
            IsPermanentRelayChannel = false;
            IsRelayChannel = false;
        }

        public void EnableRelayChannel()
        {
            throw new NotImplementedException();
        }

        public void DisableRelayChannel()
        {
            throw new NotImplementedException();
        }

        public void EnablePermanentRelayChannel()
        {
            throw new NotImplementedException();
        }

        public void DisablePermanentRelayChannel()
        {
            throw new NotImplementedException();
        }

        public void Update(SocketTextChannel socketTextChannel)
        {
            Name = socketTextChannel.Name;
            Guild = socketTextChannel.Guild.Name;
        }
    }
}