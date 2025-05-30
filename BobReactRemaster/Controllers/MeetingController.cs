﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Auth;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Meetings;
using BobReactRemaster.Data.Models.User;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BobReactRemaster.JSONModels.Meeting;

namespace BobReactRemaster.Controllers
{
    [ApiController]
    [Route("Meeting")]
    public class MeetingController : Controller
    {
         private readonly ApplicationDbContext _context;
         private readonly IMessageBus _MessageBus;
        public MeetingController(ApplicationDbContext context, IMessageBus messageBus)
        {
            _context = context;
            _MessageBus = messageBus;
        }

        [HttpGet]
        [Route("CreateMeeting")]
        [Authorize(Policy = Policies.User)]
        public IActionResult CreateMeeting()
        {
            var UserName = User.FindFirst("fullName")?.Value;
            if (UserName != null)
            {
                var user = _context.Members.Include(x => x.RegisteredToMeetingTemplates).First(q => q.UserName.ToLower() == UserName);
                MeetingTemplate tmp = new MeetingTemplate();
                MeetingTemplate_Member tmpmanymany = new MeetingTemplate_Member(user,tmp);
                tmpmanymany.IsAuthor = true;
                _context.MeetingTemplates_Members.Add(tmpmanymany);
                
                _context.MeetingTemplates.Add(tmp);
                _context.SaveChanges();


                return Ok(new { MeetingID = tmp.ID });
            }

            return NotFound();

        }
        [HttpGet]
        [Route("DeleteMeetingTemplate")]
        [Authorize(Policy = Policies.User)]
        public IActionResult DeleteMeetingTemplate(int ID)
        {
            var UserName = User.FindFirst("fullName")?.Value;
            if (UserName != null)
            {
                var user = _context.Members.Include(x => x.RegisteredToMeetingTemplates).First(q => q.UserName.ToLower() == UserName);
                MeetingTemplate? tmp = _context.MeetingTemplates.FirstOrDefault(x => x.ID == ID);
                if (tmp != null)
                {
                    _context.MeetingTemplates.Remove(tmp);
                    _context.SaveChanges();
                }
                return Ok();
            }

            return NotFound();

        }
        [HttpGet]
        [Route("DeleteSingleMeeting")]
        [Authorize(Policy = Policies.User)]
        public IActionResult DeleteSingleMeeting(int ID)
        {
            var UserName = User.FindFirst("fullName")?.Value;
            if (UserName != null)
            {
                var user = _context.Members.Include(x => x.RegisteredToMeetingTemplates).First(q => q.UserName.ToLower() == UserName);
                Meeting? tmp = _context.Meetings.Include(x => x.MeetingTemplate).Include(x => x.Subscriber).ThenInclude(x => x.Subscriber).FirstOrDefault(x => x.ID == ID && x.Subscriber.Any(x => x.IsAuthor && x.Subscriber == user));
                if (tmp != null)
                {
                    _context.Meetings.Remove(tmp);
                    _context.SaveChanges();
                    return Ok();
                }
                
            }

            return NotFound();

        }
        [HttpGet]
        [Route("LoadGeneralMeetingData")]
        [Authorize(Policy = Policies.User)]
        public IActionResult LoadGeneralMeetingData(int ID)
        {
            MeetingTemplate meetingTemplate = _context.MeetingTemplates.Include(x => x.Members).ThenInclude(y => y.RegisteredMember).FirstOrDefault(x => x.ID == ID);
            if (meetingTemplate == null)
            {
                return NotFound();
            }

            var Members = _context.Members.AsQueryable();
            List<dynamic> MembersForTransfer = new List<dynamic>();
            foreach (var Member in Members)
            {
                var found = false;
                foreach (var TemplateMember in meetingTemplate.Members)
                {
                    if (TemplateMember.RegisteredMember.UserName == Member.UserName)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    MembersForTransfer.Add(new { UserName= Member.UserName, Registered = false});
                }
                else
                {
                    MembersForTransfer.Add(new { UserName= Member.UserName, Registered = true});
                }
            }


            return Ok(new {MembersForTransfer = MembersForTransfer,Name= meetingTemplate.Name,ID = ID});
        }

        [HttpGet]
        [Route("LoadDatesMeetingData")]
        [Authorize(Policy = Policies.User)]
        public IActionResult LoadDatesMeetingData(int ID)
        {
            MeetingTemplate meetingTemplate = _context.MeetingTemplates.AsQueryable().Include(x => x.Dates).FirstOrDefault(x => x.ID == ID);
            if (meetingTemplate == null)
            {
                return NotFound();
            }
            List<dynamic> Dates = new List<dynamic>();
            foreach (MeetingDateTemplate date in meetingTemplate.Dates)
            {
                Dates.Add(new { ID= date.ID, Day = date.DayOfWeek, Start = date.Start.ToString("HH:mm"), End = date.End.ToString("HH:mm")});
            }
            return Ok(Dates);
        }

        [HttpGet]
        [Route("LoadReminderData")]
        [Authorize(Policy = Policies.User)]
        public IActionResult LoadReminderData(int ID)
        {
            MeetingTemplate meetingTemplate = _context.MeetingTemplates.AsQueryable().Include(x => x.Dates).Include(x => x.ReminderTemplate).FirstOrDefault(x => x.ID == ID);
            if (meetingTemplate == null)
            {
                return NotFound();
            }
            dynamic Data;
            if(meetingTemplate.ReminderTemplate != null)
            {
                var template = meetingTemplate.ReminderTemplate;
                Data = new { WeekDay =  template.ReminderDay,ReminderTime = template.ReminderTime.ToString("HH:mm") };
            }
            else
            {
                Data = new { WeekDay = 0, ReminderTime = "00:00" };
            }
            return Ok(Data);

        }

        [HttpGet]
        [Route("GetMeetingsTemplates")]
        [Authorize(Policy = Policies.User)]
        public IActionResult GetMeetingsTemplates()
        {
            var UserName = User.FindFirst("fullName")?.Value;
            if (UserName != null)
            {
                var user = _context.CompleteMeetingTemplateFromMember().First(q => q.UserName.ToLower() == UserName);
                //var user = _context.Members.Include(x => x.RegisteredToMeetingTemplates).ThenInclude(y => y.RegisteredMember).Include(x => x.).First(q => q.UserName.ToLower() == UserName);
                List<dynamic> MeetingTemplateJSON = new List<dynamic>();
                foreach (MeetingTemplate template in user.RegisteredToMeetingTemplates.Select(x => x.MeetingTemplate))
                {
                    var isAuthor = template.Members.Where(x => x.IsAuthor && x.RegisteredMember.UserName == user.UserName).Count() == 1;
                    MeetingTemplateJSON.Add(new {ID = template.ID, Name = template.Name, IsAuthor = isAuthor});
                }


                return Ok(new { MeetingTemplates = MeetingTemplateJSON });
            }

            return NotFound();

        }
        [HttpPost]
        [Route("SaveMeetingDates")]
        [Authorize(Policy = Policies.User)]
        public IActionResult SaveMeetingDates([FromBody] MeetingDateTemplateJSONData data)
        {
            //TODO Update/Remove/Add
            
            var MeetingTemplate = _context.MeetingTemplates.AsQueryable().Include(x => x.Members).ThenInclude(y => y.RegisteredMember).Include(x => x.Dates).First(x => x.ID == data.MeetingID);
            if (MeetingTemplate != null)
            {
                //Update
                var Date = MeetingTemplate.Dates.FirstOrDefault(x => data.Templates.Any(y => { return y.id == x.ID; }));
                if(Date != null)
                {
                    Date.Update(data.Templates.First(x => x.id == Date.ID));
                }
                //Remove
                Date = MeetingTemplate.Dates.FirstOrDefault(x => data.Templates.All(y => { return y.id != x.ID; }));
                if (Date != null)
                {
                    MeetingTemplate.Dates.Remove(Date);
                }
                //Add
                foreach(var newDate in data.Templates.Where(x => MeetingTemplate.Dates.All(y => { return y.ID != x.id; })))
                {
                    MeetingDateTemplate newTemplate = new MeetingDateTemplate();
                    newTemplate.Update(newDate);
                    MeetingTemplate.Dates.Add(newTemplate);
                }

                _context.SaveChanges();

                return Ok();
            }
            return NotFound();

        }
        [HttpPost]
        [Route("AddStaticMeeting")]
        [Authorize(Policy = Policies.User)]
        public IActionResult AddStaticMeeting([FromBody] StaticMeetingData data)
        {
            //TODO Update/Remove/Add

            var MeetingTemplate = _context.MeetingTemplates.AsQueryable().Include(x => x.Members).ThenInclude(y => y.RegisteredMember).Include(x => x.Dates).First(x => x.ID == data.MeetingID);
            if (MeetingTemplate != null)
            {
                MeetingTemplate.AddStaticMeeting(data);
                _context.SaveChanges();

                return Ok();
            }
            return NotFound();

        }
        [HttpPost]
        [Route("SaveMeetingReminder")]
        [Authorize(Policy = Policies.User)]
        public IActionResult SaveMeetingReminder([FromBody] MeetingReminderJSONData data)
        {

            var Meeting = _context.MeetingTemplates.AsQueryable().Include(x => x.Members).ThenInclude(y => y.RegisteredMember).Include(x => x.Dates).Include(x => x.ReminderTemplate).First(x => x.ID == data.MeetingID);
            if (Meeting != null)
            {
                if(Meeting.ReminderTemplate == null)
                {
                    Meeting.ReminderTemplate = new ReminderTemplate();
                }
                Meeting.ReminderTemplate.UpdateData(data);
                _context.SaveChanges();

                return Ok();
            }
            return NotFound();

        }
        [HttpPost]
        [Route("SaveMeetingGeneral")]
        [Authorize(Policy = Policies.User)]
        public IActionResult SaveMeetingGeneral([FromBody] MeetingGeneralData data)
        {
            var Meeting = _context.MeetingTemplates.AsQueryable().Include(x => x.Members).ThenInclude(y => y.RegisteredMember).First(x => x.ID == data.MeetingID);
            if(Meeting != null)
            {
                if (data.Name != null)
                {
                    Meeting.Name = data.Name;
                }
                foreach(MeetingGeneralMember member in data.Members)
                {
                    if(member.registered == false && Meeting.Members.Any(x => x.RegisteredMember.UserName.ToLower() == member.userName.ToLower()))
                    {
                        MeetingTemplate_Member remove = Meeting.Members.First(x => x.RegisteredMember.UserName.ToLower() == member.userName.ToLower());
                        _context.MeetingTemplates_Members.Remove(remove);
                        return Ok();
                    }
                    else if(member.registered && !Meeting.Members.Any(x => x.RegisteredMember.UserName.ToLower() == member.userName.ToLower()))
                    {
                        Member NewMember = _context.Members.First(x => x.UserName.ToLower() == member.userName.ToLower());
                        if(NewMember != null)
                        {
                            MeetingTemplate_Member add = new MeetingTemplate_Member();
                            add.RegisteredMember = NewMember;
                            Meeting.Members.Add(add);
                        }
                    }
                }
                _context.SaveChanges();
                return Ok();
            }
            
            return NotFound();

        }

        [HttpPost]
        [Route("UpdateParticipation")]
        [Authorize(Policy = Policies.User)]
        public IActionResult UpdateParticipation([FromBody] ParticipationData data)
        {
            MeetingParticipation part = _context.MeetingSubscriptions.AsQueryable().First(x => x.ID == data.ParticipationID);

            if(part != null)
            {
                part.State = data.State;
                part.Message = data.Info;
                _context.SaveChanges();
                return Ok();
            }

            return NotFound();
        }

        [HttpGet]
        [Route("GetMeetings")]
        [Authorize(Policy = Policies.User)]
        public IActionResult GetMeetings(int MeetingID)
        {
            var UserName = User.FindFirst("fullName")?.Value;
            if (UserName != null)
            {
                var MeetingTemplate = _context.MeetingTemplates.Include(x => x.LiveMeetings).ThenInclude(x => x.Subscriber).ThenInclude( y => y.Subscriber).FirstOrDefault(x => x.ID == MeetingID);
                if(MeetingTemplate != null)
                {
                    List<dynamic> LiveMeetings = new List<dynamic>();
                    foreach(Meeting liveMeeting in MeetingTemplate.LiveMeetings)
                    {
                        //var user = _context.Members.Include(x => x.RegisteredToMeetingTemplates).ThenInclude(y => y.RegisteredMember).Include(x => x.).First(q => q.UserName.ToLower() == UserName);
                        List<dynamic> MeetingParticipations = new List<dynamic>();
                        foreach (MeetingParticipation participant in liveMeeting.Subscriber)
                        {
                            bool isMe = participant.Subscriber.UserName == UserName;
                            MeetingParticipations.Add(new { ID = participant.ID, isMe = isMe, State = participant.State,UserName = participant.Subscriber.UserName, Info = participant.Message });
                        }
                        LiveMeetings.Add(new
                        {
                            MeetingID = liveMeeting.ID,
                            MeetingParticipations = MeetingParticipations,
                            MeetingDate = liveMeeting.MeetingDateStart.ToString("dd.MM.yyyy"),
                            MeetingStart = liveMeeting.MeetingDateStart.ToString("HH:mm"),
                            MeetingEnd = liveMeeting.MeetingDateEnd.ToString("HH:mm"),
                            IsSingle = liveMeeting.IsSingleEvent
                        });
                    }
                    return Ok(LiveMeetings);
                    
                }
               
            }

            return NotFound();

        }
    }
}
