using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Data.Models.Discord
{
    [Table("DiscordTextChannels")]
    public class TextChannel
    {
        [Key]
        public int id { get; set; }

        public string Name { get; set; }

        public bool IsPermanentRelayChannel { get; set; }

        public bool IsRelayChannel { get; set; }

        public Guild Guild { get; set; }
        private TextChannel()
        {

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
    }
}
