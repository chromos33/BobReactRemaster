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
        public WeekDay day { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }
}
