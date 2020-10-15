using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.EventBus.MessageDataTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BobReactRemaster.Controllers
{
    [ApiController]
    [Route("Test")]
    public class TestController : Controller
    {
        private IMessageBus _bus;
        public TestController(IMessageBus bus)
        {
            _bus = bus;
        }
        [HttpGet]
        [Route("EmitTwitchRelayPulse")]
        [AllowAnonymous]
        public IActionResult EmitTwitchRelayPulse()
        {
            _bus.Publish(new TwitchRelayPulseMessageData(){StreamName = "chromos33"});
            return Ok();
        }
        [HttpGet]
        [Route("EmitTwitchRelayStop")]
        [AllowAnonymous]
        public IActionResult EmitTwitchRelayStop()
        {
            _bus.Publish(new TwitchStreamStopMessageData() { StreamName = "chromos33" });
            return Ok();
        }


    }
}
