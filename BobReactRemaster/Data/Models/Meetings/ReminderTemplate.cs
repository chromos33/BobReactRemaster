using BobReactRemaster.JSONModels.Meeting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Data.Models.Meetings
{
    public class ReminderTemplate
    {
        [Key]
        public int ID { get; set; }
        public WeekDay ReminderDay { get; set; }
        public DateTime ReminderTime { get; set; }

        public int MeetingTemplateId { get; set; }
        public MeetingTemplate MeetingTemplate { get; set; }

        internal void UpdateData(MeetingReminderJSONData data)
        {
            ReminderDay = data.WeekDay;
            ReminderTime = data.Time;
        }
    }
}
