using BobReactRemaster.Data.Models.User;
using System.ComponentModel.DataAnnotations;

namespace BobReactRemaster.Data.Models.GiveAways
{
    public class GiveAway_Member
    {

        [Key]
        public int ID { get; set; }
        public Member Member { get; set; }
        public GiveAway GiveAway { get; set; }
    }
}
