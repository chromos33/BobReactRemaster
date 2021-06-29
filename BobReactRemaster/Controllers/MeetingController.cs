using Microsoft.AspNetCore.Mvc;
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
                var user = _context.Members.Include(x => x.RegisteredToMeetingTemplates).ThenInclude(y => y.RegisteredMember).First(q => q.UserName.ToLower() == UserName);
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
                Dates.Add(new { ID= date.ID, Day = date.DayOfWeek, Start = date.Start.ToString("HH:mm"), End = date.End.ToString("HH:mm") });
            }
            return Ok(Dates);
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
                    MeetingTemplateJSON.Add(new {ID = template.ID, Name = template.Name});
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
    }
}
