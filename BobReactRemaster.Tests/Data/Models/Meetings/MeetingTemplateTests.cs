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
            MeetingDateTemplate template1 = new MeetingDateTemplate();
            template1.Template = testcase;
            template1.DayOfWeek = DayOfWeek.Monday;
            template1.Start = DateTime.Now;
            template1.End = DateTime.Now.Add(TimeSpan.FromHours(4));
            testcase.Dates.Add(template1);

            MeetingDateTemplate template2 = new MeetingDateTemplate();
            template2.Template = testcase;
            template2.DayOfWeek = DayOfWeek.Tuesday;
            template2.Start = DateTime.Now;
            template2.End = DateTime.Now.Add(TimeSpan.FromHours(4));
            testcase.Dates.Add(template2);

            Member member1 = new Member("test1", "password", UserRole.Admin);
            MeetingTemplate_Member templatemember1 = new MeetingTemplate_Member(member1, null);

            Member member2 = new Member("test2", "password", UserRole.User);
            MeetingTemplate_Member templatemember2 = new MeetingTemplate_Member(member2, null);

            testcase.Members.Add(templatemember1);
            testcase.Members.Add(templatemember2);

            ReminderTemplate reminder = new ReminderTemplate();
            reminder.ReminderDay = DayOfWeek.Saturday;
            reminder.ReminderTime = DateTime.Now;
            reminder.MeetingTemplateId = 5;
            reminder.MeetingTemplate = testcase;

            testcase.ReminderTemplate = reminder;

            //Assert.AreEqual(null, command.LiveStream);
            var result = testcase.CreateMeetingsForNextWeek();

            Assert.AreEqual(2, result.Count);
            foreach(var meeting in result)
            {
                Assert.AreEqual(2, meeting.Subscriber);
            }

        }
    }
}
