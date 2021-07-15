using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Extensions;

namespace BobReactRemaster.Data.Models.Meetings
{
    public class MeetingTemplate
    {
        [Key]
        public int ID { get; set; }
        public List<MeetingDateTemplate> Dates { get; set; }
        public List<MeetingTemplate_Member> Members { get; set; }

        public ReminderTemplate ReminderTemplate { get; set; }

        public List<Meeting> LiveMeetings { get; set; }

        public string Name { get; set; }

        public MeetingTemplate()
        {
            this.Dates = new List<MeetingDateTemplate>();
            Members = new List<MeetingTemplate_Member>();
            LiveMeetings = new List<Meeting>();
        }
        public DateTime NextCreateDateTime()
        {
            var MostCurrentMeeting = LiveMeetings.Where(x => x.MeetingDateStart == LiveMeetings.Max(x => x.MeetingDateStart)).FirstOrDefault();
            if(MostCurrentMeeting != null)
            {
                if(MostCurrentMeeting.MeetingDateStart.DayOfWeek == DayOfWeek.Sunday)
                {
                    return MostCurrentMeeting.MeetingDateStart.SetTime(23, 59);
                }
                else
                {
                    return MostCurrentMeeting.MeetingDateStart.GetNextDateTimeFromTodayWithDayAndTime(DayOfWeek.Sunday);
                }
                
            }
            //today on 23 o'clock create new Meetings from Template if none exist
            return DateTime.Today.SetTime(18,0);
        }
        //DateTime Parameter for better Testing
        public List<Meeting> CreateMeetingsForNextWeek(DateTime Today)
        {
            List<Meeting> Meetings = new List<Meeting>();
            foreach(MeetingDateTemplate template in Dates)
            {
                DateTime nextMeetingStart = Today.GetNextDateTimeWithDayAndTime(template.Start, template.DayOfWeek);
                DateTime nextMeetingEnd = Today.GetNextDateTimeWithDayAndTime(template.End, template.DayOfWeek);
                DateTime nextReminder = Today.GetNextDateTimeWithDayAndTime(ReminderTemplate.ReminderTime, ReminderTemplate.ReminderDay);
                Meeting tmp = new Meeting(Members,ID, nextMeetingStart, nextMeetingEnd, nextReminder);
                Meetings.Add(tmp);
            }
            return Meetings;
        }
    }
}
