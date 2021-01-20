using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.User;

namespace BobReactRemaster.Data.Models.Meetings
{
    public class MeetingTemplate_Member
    {
        [Key]
        public int ID { get; set; }
        public Member RegisteredMember { get; set; }
        public MeetingTemplate MeetingTemplate { get; set; }
    }
}
