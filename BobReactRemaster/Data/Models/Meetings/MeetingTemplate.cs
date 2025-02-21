using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Extensions;
using BobReactRemaster.JSONModels.Meeting;

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
            Name = "Neues Meeting";
            this.Dates = new List<MeetingDateTemplate>();
            Members = new List<MeetingTemplate_Member>();
            LiveMeetings = new List<Meeting>();
        }
        public DateTime NextCreateDateTime()
        {
            //We just do it NOW as this is only caused by Starting the application and  then done Daily thus not as time sensitive as to only run one a Week
            //thus we prevent Meetings not being created if you add it and the new CreateDateTime is still a week away
            return DateTime.Now;
            /*
            var MostCurrentMeeting = LiveMeetings.Where(x => x.MeetingDateStart == LiveMeetings.Max(x => x.MeetingDateStart)).FirstOrDefault();
            if(MostCurrentMeeting != null)
            {
                var test = MostCurrentMeeting.MeetingDateStart.GetNextDateTimeFromTodayWithDayAndTime(DayOfWeek.Sunday);
                return test;
            }
            return DateTime.Today.SetTime(23,0);
            */
        }

        internal DateTime NextReminderDateTime()
        {
            var MostCurrentMeeting = LiveMeetings.Where(x => x.MeetingDateStart == LiveMeetings.Max(x => x.MeetingDateStart)).FirstOrDefault();
            if (MostCurrentMeeting != null)
            {
                return MostCurrentMeeting.ReminderDate;
            }
            return DateTime.Today.SetTime(18, 0);
        }
        //DateTime Parameter for better Testing
        public List<Meeting> CreateMeetingsForNextWeek(DateTime Today)
        {
            List<Meeting> Meetings = new List<Meeting>();
            foreach(MeetingDateTemplate template in Dates)
            {
                DateTime nextMeetingStart = Today.GetNextDateTimeWithDayAndTime(template.Start, template.DayOfWeek);
                DateTime nextMeetingEnd = Today.GetNextDateTimeWithDayAndTime(template.End, template.DayOfWeek);
                DateTime? nextReminder = null;
                if (ReminderTemplate != null)
                {
                    nextReminder = Today.GetNextDateTimeWithDayAndTime(ReminderTemplate.ReminderTime, ReminderTemplate.ReminderDay);
                }
                
                Meeting tmp = new Meeting(Members,this, nextMeetingStart, nextMeetingEnd, nextReminder);
                Meetings.Add(tmp);
            }
            return Meetings;
        }

        public void AddStaticMeeting(StaticMeetingData data)
        {
            DateTime nextMeetingStart = data.date.SetTime(data.start.Hour, data.start.Minute, data.start.Second);
            DateTime nextMeetingEnd = data.date.SetTime(data.end.Hour, data.end.Minute, data.end.Second);
            DateTime nextReminder;
            if (ReminderTemplate != null)
            {
                nextReminder = data.start.Subtract(TimeSpan.FromDays(1)).SetTime(ReminderTemplate.ReminderTime.Hour, ReminderTemplate.ReminderTime.Minute, ReminderTemplate.ReminderTime.Second);
            }
            else
            {
                nextReminder = data.start.Subtract(TimeSpan.FromDays(1)).SetTime(18, 0, 0);
            }
            
            Meeting tmp = new Meeting(Members, this, nextMeetingStart, nextMeetingEnd, nextReminder);
            tmp.IsSingleEvent = true;
            LiveMeetings.Add(tmp);
        }
    }
}
