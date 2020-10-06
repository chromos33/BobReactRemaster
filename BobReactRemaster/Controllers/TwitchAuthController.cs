using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BobReactRemaster.Controllers
{
    [ApiController]
    [Route("Twitch")]
    public class TwitchAuthController : Controller
    {
        [HttpPost]
        [Route("TwitchOAuthStart")]
        [Authorize(Policy = Policies.Admin)]
        public IActionResult TwitchOAuthStart()
        {
            return Ok();
        }
    }
}
