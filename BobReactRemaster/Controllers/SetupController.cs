using System.Collections.Generic;
using System.Linq;
using BobReactRemaster.Auth;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.JSONModels.Setup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BobReactRemaster.Controllers
{
    [ApiController]
    [Route("Setup")]
    public class SetupController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SetupController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetDiscordTokenData")]
        [Authorize(Policy = Policies.User)]
        public IActionResult GetDiscordTokenData()
        {
            var Credentials = _context.DiscordCredentials.FirstOrDefault();
            if (Credentials != null)
            {
                return Ok(new { ClientID= Credentials.ClientID, Token= Credentials.Token });
            }
            return NotFound();
        }
        [HttpPost]
        [Route("SetDiscordTokenData")]
        [Authorize(Policy = Policies.User)]
        public IActionResult SetDiscordTokenData([FromBody] DiscordTokenData Data)
        {

            var Credentials = _context.DiscordCredentials.FirstOrDefault();
            if (Credentials == null)
            {
                Credentials = new DiscordCredentials();
                _context.DiscordCredentials.Add(Credentials);
            }
            Credentials.setFromDiscordTokenData(Data);
            _context.SaveChanges();
            return Ok();
        }
        [HttpPost]
        [Route("ImportFile")]
        [Authorize(Policy = Policies.User)]
        [Consumes("text/plain")]
        public IActionResult ImportFile([FromBody] string data)
        {
            var test = JsonConvert.DeserializeObject(data);
            return Ok();
        }
    }
}
