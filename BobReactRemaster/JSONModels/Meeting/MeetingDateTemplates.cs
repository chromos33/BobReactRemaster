using BobReactRemaster.Data.Models.Meetings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.JSONModels.Meeting
{
    public class MeetingDateTemplateJSONData
    {
        public int MeetingID { get; set; }
        public MeetingDateTemplateJSON[] Templates { get; set; }
    }
    public class MeetingDateTemplateJSON
    {
        public int id { get; set; }
        public DayOfWeek day { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }

    public class StaticMeetingData
    {
        public int MeetingID { get; set; }
        public DateTime date { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }
}
