using BobReactRemaster.Data.Models.Meetings;
using BobReactRemaster.Data.Models.User;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BobReactRemaster.Extensions;
using BobReactRemaster.JSONModels.Meeting;
using NUnit.Framework.Legacy;

namespace BobReactRemaster.Tests.Data.Models.Meetings
{
    class MeetingTemplateTests
    {
        [Test]
        public void InitData_ValidState_CorrectMeeting()
        {
            MeetingTemplate testcase = new MeetingTemplate();
            testcase.ID = 5;
            MeetingDateTemplate template1 = new MeetingDateTemplate();
            template1.Template = testcase;
            template1.DayOfWeek = DayOfWeek.Monday;
            template1.Start = new DateTime(2020,1,2,0,0,0);
            template1.End = new DateTime(2020, 1, 2, 4, 0, 0);
            testcase.Dates.Add(template1);

            MeetingDateTemplate template2 = new MeetingDateTemplate();
            template2.Template = testcase;
            template2.DayOfWeek = DayOfWeek.Monday;
            template2.Start = new DateTime(2020, 1, 2, 0, 0, 0);
            template2.End = new DateTime(2020, 1, 2, 4, 0, 0);
            testcase.Dates.Add(template2);

            Member member1 = new Member("test1", "password", UserRole.Admin);
            MeetingTemplate_Member templatemember1 = new MeetingTemplate_Member(member1, null);

            Member member2 = new Member("test2", "password", UserRole.User);
            MeetingTemplate_Member templatemember2 = new MeetingTemplate_Member(member2, null);

            testcase.Members.Add(templatemember1);
            testcase.Members.Add(templatemember2);

            ReminderTemplate reminder = new ReminderTemplate();
            reminder.ID = 5;
            reminder.ReminderDay = DayOfWeek.Saturday;
            reminder.ReminderTime = new DateTime(2020, 1, 1, 0, 0, 0);
            reminder.MeetingTemplateId = 5;
            reminder.MeetingTemplate = testcase;

            testcase.ReminderTemplate = reminder;

            //ClassicAssert.AreEqual(null, command.LiveStream);
            var result = testcase.CreateMeetingsForNextWeek(new DateTime(2020, 1, 2, 0, 0, 0));

            ClassicAssert.AreEqual(2, result.Count);
            foreach(var meeting in result)
            {
                ClassicAssert.AreEqual(2, meeting.Subscriber.Count);
                ClassicAssert.AreEqual(new DateTime(2020, 1, 6, 0, 0, 0), meeting.MeetingDateStart);
            }

        }
        [Test]
        public void AddStaticMeeting_ValidState_CorrectMeetingDates()
        {
            MeetingTemplate testcase = new MeetingTemplate();
            testcase.ID = 5;
            

            Member member1 = new Member("test1", "password", UserRole.Admin);
            MeetingTemplate_Member templatemember1 = new MeetingTemplate_Member(member1, null);

            Member member2 = new Member("test2", "password", UserRole.User);
            MeetingTemplate_Member templatemember2 = new MeetingTemplate_Member(member2, null);

            testcase.Members.Add(templatemember1);
            testcase.Members.Add(templatemember2);
            var date = DateTime.Now.Add(TimeSpan.FromHours(1));
            var start = DateTime.Now.Add(TimeSpan.FromHours(1));
            var end = DateTime.Now.Add(TimeSpan.FromHours(1));
            StaticMeetingData data = new StaticMeetingData()
            {
                date = date,
                MeetingID = 5,
                start = start,
                end = end
            };

            testcase.AddStaticMeeting(data);

            ClassicAssert.IsTrue(testcase.LiveMeetings.Any(x => x.MeetingDateStart == date.SetTime(start.Hour,start.Minute,start.Second)));
            ClassicAssert.IsTrue(testcase.LiveMeetings.Any(x => x.MeetingDateEnd == date.SetTime(end.Hour, end.Minute, end.Second)));
        }
        [Test]
        public void AddStaticMeeting_ValidState_CorrectMeetingReminderDate()
        {
            MeetingTemplate testcase = new MeetingTemplate();
            testcase.ID = 5;


            Member member1 = new Member("test1", "password", UserRole.Admin);
            MeetingTemplate_Member templatemember1 = new MeetingTemplate_Member(member1, null);

            Member member2 = new Member("test2", "password", UserRole.User);
            MeetingTemplate_Member templatemember2 = new MeetingTemplate_Member(member2, null);

            testcase.Members.Add(templatemember1);
            testcase.Members.Add(templatemember2);
            var date = DateTime.Now.Add(TimeSpan.FromHours(1));
            var start = DateTime.Now.Add(TimeSpan.FromHours(1));
            var end = DateTime.Now.Add(TimeSpan.FromHours(1));
            StaticMeetingData data = new StaticMeetingData()
            {
                date = date,
                MeetingID = 5,
                start = start,
                end = end
            };

            testcase.AddStaticMeeting(data);

            ClassicAssert.IsTrue(testcase.LiveMeetings.Any(x => x.ReminderDate == date.Subtract(TimeSpan.FromDays(1)).SetTime(18, 0, 0)));
        }
        [Test]
        public void AddStaticMeeting_ValidState_AllMembersAddedAsParticipationRecords()
        {
            MeetingTemplate template = new MeetingTemplate();
            template.ID = 5;


            Member member1 = new Member("test1", "password", UserRole.Admin);
            MeetingTemplate_Member templatemember1 = new MeetingTemplate_Member(member1, null);

            Member member2 = new Member("test2", "password", UserRole.User);
            MeetingTemplate_Member templatemember2 = new MeetingTemplate_Member(member2, null);

            template.Members.Add(templatemember1);
            template.Members.Add(templatemember2);
            var date = DateTime.Now.Add(TimeSpan.FromHours(1));
            var start = DateTime.Now.Add(TimeSpan.FromHours(1));
            var end = DateTime.Now.Add(TimeSpan.FromHours(1));
            StaticMeetingData data = new StaticMeetingData()
            {
                date = date,
                MeetingID = 5,
                start = start,
                end = end
            };

            template.AddStaticMeeting(data);
            Meeting testcase = template.LiveMeetings.First();
            ClassicAssert.IsTrue(template.LiveMeetings.Count() == 1);
            ClassicAssert.IsTrue(testcase.Subscriber.Any(x => x.Subscriber.UserName == member1.UserName));
            ClassicAssert.IsTrue(testcase.Subscriber.Any(x => x.Subscriber.UserName == member2.UserName));
        }
    }
}
