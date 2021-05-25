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
            MeetingTemplate meetingTemplate = _context.MeetingTemplates.FirstOrDefault(x => x.ID == ID);
            if (meetingTemplate == null)
            {
                return NotFound();
            }

            var Members = _context.Members.AsQueryable();
            List<dynamic> AvailableMembers = new List<dynamic>();
            List<dynamic> RegisteredMembers = new List<dynamic>();
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
                    AvailableMembers.Add(new { UserName= Member.UserName});
                }
                else
                {
                    RegisteredMembers.Add(new { UserName= Member.UserName});
                }
            }


            return Ok(new {AvailableMembers = AvailableMembers,RegisteredMember = RegisteredMembers,Name= meetingTemplate.Name});
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
    }
}
