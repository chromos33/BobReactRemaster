﻿using System;
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
            return DateTime.Today.SetTime(23,0);
        }

        internal DateTime NextReminderDateTime()
        {
            var MostCurrentMeeting = LiveMeetings.Where(x => x.MeetingDateStart == LiveMeetings.Max(x => x.MeetingDateStart)).FirstOrDefault();
            if (MostCurrentMeeting != null)
            {
                if (MostCurrentMeeting.ReminderDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    return MostCurrentMeeting.ReminderDate.SetTime(18, 0);
                }
                else
                {
                    return MostCurrentMeeting.ReminderDate.GetNextDateTimeFromTodayWithDayAndTime(DayOfWeek.Sunday);
                }

            }
            //today on 23 o'clock create new Meetings from Template if none exist
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
                DateTime nextReminder = Today.GetNextDateTimeWithDayAndTime(ReminderTemplate.ReminderTime, ReminderTemplate.ReminderDay);
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
