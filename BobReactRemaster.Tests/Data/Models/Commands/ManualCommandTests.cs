using System;
using System.Collections.Generic;
using System.Text;
using BobReactRemaster.Controllers;
using BobReactRemaster.Data.Models.Commands;
using BobReactRemaster.JSONModels.Stream;
using NUnit.Framework;

namespace BobReactRemaster.Tests.Data.Models.Commands
{
    class IntervalCommandTests
    {
        [Test]
        public void InitData_ValidInput_CorrectState()
        {
            
            int commandID = 5;
            string name = "test";
            string response = "Hier Test";
            int streamid = 10;
            int interval = 5;
            var data = new IntervalCommandSaveData()
            {
                CommandID = commandID,
                Name = name,
                Response = response,
                StreamID = streamid,
                Interval = interval

            };
            IntervalCommand command = new IntervalCommand(data);
            Assert.AreEqual(interval,command.AutoInverval);
            Assert.AreEqual(name,command.Name);
            Assert.AreEqual(response,command.Response);
            Assert.AreEqual(true,command.Active);
            Assert.AreEqual(streamid,command.LiveStreamId);
            Assert.AreEqual(0,command.ID);
            Assert.AreEqual(null,command.LiveStream);
        }
    }
}
