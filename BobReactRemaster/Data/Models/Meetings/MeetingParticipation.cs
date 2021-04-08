using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.User;

namespace BobReactRemaster.Data.Models.Meetings
{
    public class MeetingParticipation
    {
        [Key]
        public int ID { get; set; }
        public Member Subscriber { get; set; }
        public Meeting Meeting { get; set; }

        public bool IsAuthor { get; set; }

        public string Message { get; set; }

        public MeetingParticipationState State { get; set; }


    }
}
