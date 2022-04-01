using BobReactRemaster.Auth;
using BobReactRemaster.Data;
using BobReactRemaster.JSONModels.Stream;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Controllers
{
    [ApiController]
    [Route("StreamRelayChannel")]
    public class StreamRelayChannelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StreamRelayChannelController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("GetTwitchRelayData")]
        [Authorize(Policy = Policies.User)]
        public IActionResult GetTwitchRelayData([FromBody] StreamRelayQueryInitData data)
        {
            var stream = _context.TwitchStreams.FirstOrDefault(x => x.Id == data.StreamID);
            if (stream != null)
            {
                var permanentchannels = _context.DiscordTextChannels.AsQueryable().Where(x => 
                    x.IsPermanentRelayChannel &&
                    x.LiveStreamID == data.StreamID ||
                    x.LiveStreamID == null
                );
                List<dynamic> returnChannels = new List<dynamic>();
                int index = 0;
                int selectedChannelIndex = 0;
                foreach (var channel in permanentchannels)
                {
                    index++;
                    if (channel.LiveStreamID != null && channel.LiveStreamID == data.StreamID)
                    {
                        selectedChannelIndex = index;
                        returnChannels.Add(new {ChannelName= channel.Name, ChannelID=channel.id,Active=true});
                    }
                    else
                    {
                        returnChannels.Add(new {ChannelName= channel.Name, ChannelID=channel.id,Active=false});
                    }
                    
                }
                return Ok(new {selectedChannelIndex = selectedChannelIndex, Channels = returnChannels, RelayEnabled = stream.RelayEnabled, RandomRelayEnabled = stream.VariableRelayChannel, UpTimeInterval = stream.UpTimeInterval });
            }

            return NotFound();
        }
        [HttpPost]
        [Route("SaveTwitchRelayData")]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> SaveTwitchRelayData([FromBody] StreamRelayQuerySaveData data)
        {
            var stream = _context.TwitchStreams.FirstOrDefault(x => x.Id == data.StreamID);
            if (stream != null)
            {
                stream.RelayEnabled = data.RelayEnabled;
                stream.VariableRelayChannel = data.RandomRelayEnabled;
                stream.UpTimeInterval = data.AutoInterval;
                var channel = _context.DiscordTextChannels.AsQueryable().FirstOrDefault(x =>
                   x.IsPermanentRelayChannel &&
                   (x.LiveStreamID == data.StreamID ||
                   x.LiveStreamID == null) &&
                   x.id == data.RelayChannelID
                );
                stream.UnsetRelayChannel();
                if (channel != null)
                {
                    stream.SetRelayChannel(channel);
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
    }
}
