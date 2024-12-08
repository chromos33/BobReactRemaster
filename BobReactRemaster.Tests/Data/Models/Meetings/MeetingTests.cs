using BobReactRemaster.Data.Models.Meetings;
using BobReactRemaster.Data.Models.User;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework.Legacy;

namespace BobReactRemaster.Tests.Data.Models.Meetings
{
    class MeetingTests
    {
        [Test]
        public void MeetingConstructor_ValidInput_CorrectMeetingWithChilObjects()
        {
            Member member1 = new Member("test1","password",UserRole.Admin);
            MeetingTemplate_Member template1 = new MeetingTemplate_Member(member1,null);

            Member member2 = new Member("test2","password",UserRole.User);
            MeetingTemplate_Member template2 = new MeetingTemplate_Member(member2, null);
            List<MeetingTemplate_Member> list = new List<MeetingTemplate_Member>();
            list.Add(template1);
            list.Add(template1);

            int MeetingTemplateID = 5;
            MeetingTemplate template = new MeetingTemplate();
            template.ID = MeetingTemplateID;

            DateTime MeetingDate = DateTime.Now;
            DateTime ReminderDate = DateTime.Now.Subtract(TimeSpan.FromHours(1));

            Meeting testcase = new Meeting(list, template, MeetingDate, MeetingDate, ReminderDate);




            ClassicAssert.AreEqual(MeetingDate, testcase.MeetingDateStart);
            ClassicAssert.AreEqual(ReminderDate, testcase.ReminderDate);
            ClassicAssert.AreEqual(2, testcase.Subscriber.Count);
            ClassicAssert.AreEqual(MeetingTemplateID,testcase.MeetingTemplateID);

        }
        [Test]
        public void MeetingParticipationConstructor_ValidInput_CorrectState()
        {
            List<MeetingTemplate_Member> list = new List<MeetingTemplate_Member>();

            int MeetingTemplateID = 5;
            MeetingTemplate template = new MeetingTemplate();
            template.ID = MeetingTemplateID;
            DateTime MeetingDate = DateTime.Now;
            DateTime ReminderDate = DateTime.Now.Subtract(TimeSpan.FromHours(1));

            Meeting meeting = new Meeting(list, template, MeetingDate, MeetingDate, ReminderDate);
            Member member1 = new Member("test1", "password", UserRole.Admin);
            MeetingParticipation testcase = new MeetingParticipation(meeting, member1, true);

            ClassicAssert.AreEqual(member1, testcase.Subscriber);
            ClassicAssert.AreEqual("test1", testcase.Subscriber.UserName);
        }
    }
}
