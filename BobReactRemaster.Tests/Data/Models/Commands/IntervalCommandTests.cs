using System;
using System.Collections.Generic;
using System.Text;
using BobReactRemaster.Data.Models.Commands;
using BobReactRemaster.JSONModels.Stream;
using NUnit.Framework;

namespace BobReactRemaster.Tests.Data.Models.Commands
{
    class ManualCommandTests
    {
        [Test]
        public void Constructor_ValidInput_CorrectState()
        {
            
            int commandID = 5;
            string name = "test";
            string response = "Hier Test";
            string trigger = "!test";
            int streamid = 10;
            var data = new ManualCommandSaveData()
            {
                CommandID = commandID,
                Name = name,
                Response = response,
                Trigger = trigger,
                StreamID = streamid

            };
            ManualCommand command = new ManualCommand(data);
            Assert.AreEqual(trigger,command.Trigger);
            Assert.AreEqual(name,command.Name);
            Assert.AreEqual(response,command.Response);
            Assert.AreEqual(true,command.Active);
            Assert.AreEqual(streamid,command.LiveStreamId);
            Assert.AreEqual(0, command.ID);
            Assert.AreEqual(null, command.LiveStream);
        }
    }
}
