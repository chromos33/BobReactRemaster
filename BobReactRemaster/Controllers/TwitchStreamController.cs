using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Auth;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Stream.Twitch;
using BobReactRemaster.JSONModels.Twitch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BobReactRemaster.Controllers
{
    [ApiController]
    [Route("TwitchStream")]
    public class TwitchStreamController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public TwitchStreamController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost]
        [Route("GetTwitchStreams")]
        [Authorize(Policy = Policies.Admin)]
        public IActionResult GetTwitchStreams()
        {
            List<TwitchStreamListData> response = new List<TwitchStreamListData>();
            foreach (var Stream in _context.TwitchStreams)
            {
                response.Add(Stream.GetStreamListData());
            }

            return Ok(response);
        }
        [HttpPost]
        [Route("SaveTwitchGeneral")]
        [Authorize(Policy = Policies.Admin)]
        public IActionResult SaveTwitchGeneral([FromBody] TwitchGeneralData data)
        {
            var stream = _context.TwitchStreams.FirstOrDefault(x => x.Id == data.StreamID);
            if (!String.Equals(stream.StreamName, data.StreamName, StringComparison.CurrentCultureIgnoreCase))
            {
                stream.StreamName = data.StreamName;
                _context.SaveChanges();
            }
            return Ok();
        }
    }

    
}
