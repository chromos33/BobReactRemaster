using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Data.Models.Meetings
{
    public class Meeting
    {
        [Key]
        public int ID { get; set; }
        public List<MeetingParticipation> Subscriber { get; set; }
        public DateTime MeetingDate { get; set; }

        public DateTime ReminderDate { get; set; }

    }
}
