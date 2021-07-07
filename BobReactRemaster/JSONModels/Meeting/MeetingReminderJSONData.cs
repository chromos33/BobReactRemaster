using BobReactRemaster.Data.Models.Meetings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.JSONModels.Meeting
{
    public class MeetingReminderJSONData
    {
        public DayOfWeek WeekDay { get; set; }
        public DateTime Time { get; set; }
        public int MeetingID { get; set; }
    }
}
