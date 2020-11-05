using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BobReactRemaster.Auth;
using BobReactRemaster.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BobReactRemaster.Controllers
{
    [ApiController]
    [Route("StreamSubscriptions")]
    public class StreamSubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StreamSubscriptionController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetUserSubscriptions")]
        [Authorize(Policy = Policies.User)]
        public IActionResult GetUserSubscriptions()
        {

            var UserName = User.FindFirst("fullName")?.Value;
            if (UserName != null)
            {
                var user = _context.Members.Include(x => x.StreamSubscriptions).ThenInclude(x => x.LiveStream).FirstOrDefault(x =>
                    String.Equals(x.UserName, UserName, StringComparison.CurrentCultureIgnoreCase));
                Console.WriteLine("test");
            }
            return NotFound();
        }
    }
}
