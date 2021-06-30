using BobReactRemaster.JSONModels.Meeting;
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
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public void Update(MeetingDateTemplateJSON data)
        {
            DayOfWeek = data.day;
            Start = data.start;
            End = data.end;
        }
    }
}
