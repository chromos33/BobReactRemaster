using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Data.Models.Meetings
{
    public class MeetingTemplate
    {
        [Key]
        public int ID { get; set; }
        public List<MeetingDateTemplate> Dates { get; set; }
        public List<MeetingTemplate_Member> Members { get; set; }

        public ReminderTemplate ReminderTemplate { get; set; }
    }
}
