using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Auth;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Stream;
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
        [HttpPost]
        [Route("CreateTwitchStream")]
        [Authorize(Policy = Policies.Admin)]
        public IActionResult CreateTwitchStream([FromBody] TwitchGeneralData data)
        {
            var stream = _context.TwitchStreams.FirstOrDefault(x => String.Equals(x.StreamName, data.StreamName, StringComparison.CurrentCultureIgnoreCase));
            if (stream == null)
            {
                stream = new TwitchStream(data.StreamName);
                _context.TwitchStreams.Add(stream);
                _context.SaveChanges();
            }
            
            return Ok(new { StreamID = stream.Id});
        }
        [HttpPost]
        [Route("DeleteTwitchStream")]
        [Authorize(Policy = Policies.Admin)]
        public IActionResult DeleteTwitchStream([FromBody] TwitchGeneralData data)
        {
            var stream = _context.TwitchStreams.FirstOrDefault(x => x.Id == data.StreamID);
            if (stream != null)
            {
                _context.TwitchStreams.Remove(stream);
                _context.SaveChanges();
                return Ok();
            }

            return NotFound();


        }
    }

    
}
