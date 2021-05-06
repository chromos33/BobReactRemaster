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
using Microsoft.AspNetCore.Authorization;

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
            MeetingTemplate tmp = new MeetingTemplate();
            _context.MeetingTemplates.Add(tmp);


            return Ok(new { MeetingID = tmp.ID});
        }
        [HttpGet]
        [Route("LoadGeneralMeetingData")]
        [Authorize(Policy = Policies.User)]
        [Authorize(Policy = Policies.Admin)]
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
    }
}
