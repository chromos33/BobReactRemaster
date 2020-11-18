using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Auth;
using BobReactRemaster.Data;
using BobReactRemaster.JSONModels.Stream;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BobReactRemaster.Controllers
{
    [ApiController]
    [Route("RelayCommands")]
    public class RelayCommandController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RelayCommandController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("GetTwitchCommandData")]
        [Authorize(Policy = Policies.User)]
        public IActionResult GetTwitchCommandData([FromBody] StreamRelayQueryInitData data)
        {
            var stream = _context.TwitchStreams.Include(x => x.RelayIntervalCommands).Include(x => x.RelayManualCommands).FirstOrDefault(x => x.Id == data.StreamID);
            if (stream != null)
            {
                List<dynamic> IntervalCommands = new List<dynamic>();
                List<dynamic> ManualCommands = new List<dynamic>();
                foreach (var command in stream.RelayIntervalCommands)
                {
                    IntervalCommands.Add(new  {ID= command.ID, Name= command.Name, Response= command.Response, Interval= command.AutoInverval});
                }
                foreach (var command in stream.RelayManualCommands)
                {
                    ManualCommands.Add(new { ID = command.ID, Name = command.Name, Response = command.Response, Trigger= command.Trigger });
                }
                return Ok(new {IntervalCommands = IntervalCommands, ManualCommands = ManualCommands});
            }

            return NotFound();
        }
        [HttpPost]
        [Route("SaveManualCommand")]
        [Authorize(Policy = Policies.User)]
        public IActionResult SaveManualCommand([FromBody] ManualCommandSaveData data)
        {
            return NotFound();
        }
        [HttpPost]
        [Route("CreateManualCommand")]
        [Authorize(Policy = Policies.User)]
        public IActionResult CreateManualCommand([FromBody] ManualCommandSaveData data)
        {
            return NotFound();
        }

        private int TryPersistManualCommand(ManualCommandSaveData data)
        {

            return 0;
        }
        [HttpPost]
        [Route("DeleteManualCommand")]
        [Authorize(Policy = Policies.User)]
        public IActionResult DeleteManualCommand([FromBody] CommandDeleteData data)
        {
            return NotFound();
        }

    }
}
