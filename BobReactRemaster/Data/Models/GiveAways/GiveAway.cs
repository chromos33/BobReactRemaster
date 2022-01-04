using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.Data.Models.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BobReactRemaster.Data.Models.GiveAways
{
    public class GiveAway
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public TextChannel TextChannel { get; set; }

        public List<GiveAway_Member> Admins { get; set; }

        public List<Gift> Gifts { get; set; }
    }
}
