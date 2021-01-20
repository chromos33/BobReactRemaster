using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Data.Models.Meetings
{
    public class MeetingDateTemplate
    {
        [Key]
        public int ID { get; set; }

        public MeetingTemplate Template { get; set; }

        public WeekDay DayOfWeek { get; set; }
        public DateTime Time { get; set; }
    }
}
