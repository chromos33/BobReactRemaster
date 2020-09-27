using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Data.Models.Discord
{
    public class Guild
    {
        [Key]
        public string Name { get; set; }
        public List<TextChannel> TextChannels { get; set; }
    }
}
