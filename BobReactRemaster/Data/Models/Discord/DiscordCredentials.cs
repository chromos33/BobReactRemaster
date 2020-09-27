using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Data.Models.Discord
{
    public class DiscordCredentials
    {
        [Key]
        public int id { get; set; }
        public string ClientID { get; set; }
        public string Token { get; set; }
    }
}
