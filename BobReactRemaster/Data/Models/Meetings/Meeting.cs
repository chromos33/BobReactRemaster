﻿using BobReactRemaster.Data.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Data.Models.Meetings
{
    public class Meeting
    {
        [Key]
        public int ID { get; set; }
        public List<MeetingParticipation> Subscriber { get; set; }
        public DateTime MeetingDateStart { get; set; }
        public DateTime MeetingDateEnd { get; set; }

        public DateTime ReminderDate { get; set; }

        public bool ReminderSent { get; set; }

        public int MeetingTemplateID { get; set; }
        public MeetingTemplate MeetingTemplate { get; set; }    
        public bool IsSingleEvent { get; set; }

        private Meeting()
        {

        }
        public Meeting(List<MeetingTemplate_Member> Members,MeetingTemplate MeetingTemplate, DateTime MeetingDateStart, DateTime MeetingDateEnd, DateTime? ReminderDate)
        {
            IsSingleEvent = false;
            this.MeetingTemplate = MeetingTemplate;
            this.MeetingTemplateID = MeetingTemplate.ID;
            this.MeetingDateStart = MeetingDateStart;
            this.MeetingDateEnd = MeetingDateEnd;
            if (ReminderDate != null)
            {
                this.ReminderDate = (DateTime) ReminderDate;
            }
            
            this.Subscriber = new List<MeetingParticipation>();
            foreach(MeetingTemplate_Member Member in Members)
            {
                bool IsAuthor = Member.IsAuthor;
                MeetingParticipation tmp = new MeetingParticipation(this, Member.RegisteredMember,IsAuthor);
                Subscriber.Add(tmp);
            }
        }
    }
}
