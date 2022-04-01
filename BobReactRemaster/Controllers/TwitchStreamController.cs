using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Auth;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.EventBus.MessageDataTypes;
using BobReactRemaster.JSONModels.Twitch;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TwitchLib.Api;

namespace BobReactRemaster.Controllers
{
    [ApiController]
    [Route("TwitchStream")]
    public class TwitchStreamController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMessageBus _MessageBus;
        public TwitchStreamController(ApplicationDbContext context, IConfiguration configuration, IMessageBus messageBus)
        {
            _context = context;
            _configuration = configuration;
            _MessageBus = messageBus;
        }
        [HttpPost]
        [Route("GetTwitchStreams")]
        [Authorize(Policy = Policies.User)]
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
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> SaveTwitchGeneral([FromBody] TwitchGeneralData data)
        {
            var stream = _context.TwitchStreams.FirstOrDefault(x => x.Id == data.StreamID);
            if (!String.Equals(stream.StreamName, data.StreamName, StringComparison.CurrentCultureIgnoreCase))
            {
                stream.StreamName = data.StreamName;
                if (stream.StreamID.IsNullOrEmpty())
                {
#pragma warning disable CS8601 // Possible null reference assignment.
                    stream.StreamID = await RequestTwitchClientID(stream.StreamName);
#pragma warning restore CS8601 // Possible null reference assignment.
                }
                _context.SaveChanges();
            }

            if (stream.StreamID.IsNullOrEmpty())
            {
#pragma warning disable CS8601 // Possible null reference assignment.
                stream.StreamID = await RequestTwitchClientID(stream.StreamName);
#pragma warning restore CS8601 // Possible null reference assignment.
                _context.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Route("CreateTwitchStream")]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> CreateTwitchStream([FromBody] TwitchGeneralData data)
        {
            var stream = _context.TwitchStreams.FirstOrDefault(x => String.Equals(x.StreamName, data.StreamName, StringComparison.CurrentCultureIgnoreCase));
            if (stream == null)
            {
                stream = new TwitchStream(data.StreamName);
#pragma warning disable CS8601 // Possible null reference assignment.
                stream.StreamID = await RequestTwitchClientID(stream.StreamName);
#pragma warning restore CS8601 // Possible null reference assignment.
                _context.TwitchStreams.Add(stream);
                _context.SaveChanges();
                _MessageBus.Publish(new StreamCreatedMessageData(stream));
                
            }
            
            return Ok(new { StreamID = stream.Id});
        }

        private async Task<string?> RequestTwitchClientID(string StreamName)
        {
            var api = new TwitchAPI();
            var cred = _context.TwitchCredentials.FirstOrDefault(x => x.isMainAccount);
            api.Settings.AccessToken = cred.Token;
            api.Settings.ClientId = cred.ClientID;
            var result = await api.Helix.Users.GetUsersAsync(logins: new List<string>() {StreamName});
            return result.Users.FirstOrDefault()?.Id;
        }
        [HttpPost]
        [Route("DeleteTwitchStream")]
        [Authorize(Policy = Policies.User)]
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
