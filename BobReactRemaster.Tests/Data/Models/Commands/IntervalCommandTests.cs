using System;
using System.Collections.Generic;
using System.Text;
using BobReactRemaster.Data.Models.Commands;
using BobReactRemaster.JSONModels.Stream;
using NUnit.Framework;
using NUnit.Framework.Legacy;

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
            ClassicAssert.AreEqual(trigger,command.Trigger);
            ClassicAssert.AreEqual(name,command.Name);
            ClassicAssert.AreEqual(response,command.Response);
            ClassicAssert.AreEqual(true,command.Active);
            ClassicAssert.AreEqual(streamid,command.LiveStreamId);
            ClassicAssert.AreEqual(0, command.ID);
            ClassicAssert.AreEqual(null, command.LiveStream);
        }
    }
}
