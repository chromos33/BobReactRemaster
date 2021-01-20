using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.User;

namespace BobReactRemaster.Data.Models.Meetings
{
    public class MeetingSubscription
    {
        [Key]
        public int ID { get; set; }
        public Member Subscriber { get; set; }
        public Meeting Meeting { get; set; }
    }
}
