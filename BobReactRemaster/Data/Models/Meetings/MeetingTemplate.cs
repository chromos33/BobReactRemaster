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
            var MostCurrentMeeting = LiveMeetings.Where(x => x.MeetingDate == LiveMeetings.Max(x => x.MeetingDate)).FirstOrDefault();
            if(MostCurrentMeeting != null)
            {
                if(MostCurrentMeeting.MeetingDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    return MostCurrentMeeting.MeetingDate.SetTime(23, 59);
                }
                else
                {
                    return MostCurrentMeeting.MeetingDate.GetNextDateTimeWithDayAndTime(DayOfWeek.Sunday);
                }
                
            }
            //today on 23 o'clock create new Meetings from Template if none exist
            return DateTime.Today.AddHours(23);


        }
        public List<Meeting> CreateMeetingsForNextWeek()
        {
            List<Meeting> Meetings = new List<Meeting>();
            foreach(MeetingDateTemplate template in Dates)
            {
                Meeting tmp = new Meeting();
            }
            return Meetings;
        }
    }
}
