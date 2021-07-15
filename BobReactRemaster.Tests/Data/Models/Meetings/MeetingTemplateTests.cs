using BobReactRemaster.Data.Models.Meetings;
using BobReactRemaster.Data.Models.User;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

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

            //Assert.AreEqual(null, command.LiveStream);
            var result = testcase.CreateMeetingsForNextWeek(new DateTime(2020, 1, 2, 0, 0, 0));

            Assert.AreEqual(2, result.Count);
            foreach(var meeting in result)
            {
                Assert.AreEqual(2, meeting.Subscriber.Count);
                Assert.AreEqual(new DateTime(2020, 1, 6, 0, 0, 0), meeting.MeetingDateStart);
            }

        }
    }
}
