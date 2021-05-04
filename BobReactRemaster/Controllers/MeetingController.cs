using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Auth;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Meetings;
using BobReactRemaster.EventBus.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace BobReactRemaster.Controllers
{
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
        [Route("GetStreamQuotes")]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> CreateMeeting()
        {
            MeetingTemplate tmp = new MeetingTemplate();
            _context.MeetingTemplates.Add(tmp);


            return Ok(new { MeetingID = tmp.ID});
        }
    }
}
