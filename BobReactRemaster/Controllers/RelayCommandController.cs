using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Auth;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Commands;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.EventBus.MessageDataTypes;
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
        private readonly IMessageBus bus;

        public RelayCommandController(ApplicationDbContext context, IMessageBus bus)
        {
            _context = context;
            this.bus = bus;
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
                    IntervalCommands.Add(new  {ID= command.ID, Name= command.Name, Response= command.Response, Interval= command.AutoInverval, Open = false});
                }
                foreach (var command in stream.RelayManualCommands)
                {
                    ManualCommands.Add(new { ID = command.ID, Name = command.Name, Response = command.Response, Trigger= command.Trigger, Open = false });
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
            var command = _context.ManualCommands.FirstOrDefault(x => x.ID == data.CommandID);
            if (command != null)
            {
                command.UpdateData(data);
                _context.SaveChanges();
                bus.Publish(new RefreshManualRelayCommands());
                return Ok();
            }
            return NotFound();
        }
        [HttpPost]
        [Route("CreateManualCommand")]
        [Authorize(Policy = Policies.User)]
        public IActionResult CreateManualCommand([FromBody] ManualCommandSaveData data)
        {
            var command = _context.ManualCommands.FirstOrDefault(x => x.Trigger == data.Trigger);
            if (command == null)
            {
                command = new ManualCommand();
                command.InitData(data);
                _context.ManualCommands.Add(command);
                _context.SaveChanges();
                bus.Publish(new RefreshManualRelayCommands());
                return Ok(new {ID= command.ID});
            }
            return NotFound();
        }
        [HttpPost]
        [Route("DeleteManualCommand")]
        [Authorize(Policy = Policies.User)]
        public IActionResult DeleteManualCommand([FromBody] CommandDeleteData data)
        {
            var command = _context.ManualCommands.FirstOrDefault(x => x.ID == data.CommandId);
            if (command != null)
            {
                _context.ManualCommands.Remove(command);
                _context.SaveChanges();
                bus.Publish(new RefreshManualRelayCommands());
                return Ok();
            }
            return NotFound();
        }
        [HttpPost]
        [Route("SaveIntervalCommand")]
        [Authorize(Policy = Policies.User)]
        public IActionResult SaveIntervalCommand([FromBody] IntervalCommandSaveData data)
        {
            var command = _context.IntervalCommands.FirstOrDefault(x => x.ID == data.CommandID);
            if (command != null)
            {
                command.UpdateData(data);
                _context.SaveChanges();
                bus.Publish(new RefreshIntervalRelayCommands());
                return Ok();
            }
            return NotFound();
        }
        [HttpPost]
        [Route("CreateIntervalCommand")]
        [Authorize(Policy = Policies.User)]
        public IActionResult CreateIntervalCommand([FromBody] IntervalCommandSaveData data)
        {
            var command = _context.IntervalCommands.FirstOrDefault(x => x.Name == data.Name);
            if (command == null)
            {
                command = new IntervalCommand();
                command.InitData(data);
                _context.IntervalCommands.Add(command);
                _context.SaveChanges();
                bus.Publish(new RefreshIntervalRelayCommands());
                return Ok(new { ID = command.ID });
            }
            return NotFound();
        }
        [HttpPost]
        [Route("DeleteIntervalCommand")]
        [Authorize(Policy = Policies.User)]
        public IActionResult DeleteIntervalCommand([FromBody] CommandDeleteData data)
        {
            var command = _context.IntervalCommands.FirstOrDefault(x => x.ID == data.CommandId);
            if (command != null)
            {
                _context.IntervalCommands.Remove(command);
                _context.SaveChanges();
                bus.Publish(new RefreshIntervalRelayCommands());
                return Ok();
            }
            return NotFound();
        }
    }
}
