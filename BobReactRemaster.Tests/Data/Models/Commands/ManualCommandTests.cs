using System;
using System.Collections.Generic;
using System.Text;
using BobReactRemaster.Controllers;
using BobReactRemaster.Data.Models.Commands;
using BobReactRemaster.JSONModels.Stream;
using NUnit.Framework;
using NUnit.Framework.Legacy;

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
            ClassicAssert.AreEqual(interval,command.AutoInverval);
            ClassicAssert.AreEqual(name,command.Name);
            ClassicAssert.AreEqual(response,command.Response);
            ClassicAssert.AreEqual(true,command.Active);
            ClassicAssert.AreEqual(streamid,command.LiveStreamId);
            ClassicAssert.AreEqual(0,command.ID);
            ClassicAssert.AreEqual(null,command.LiveStream);
        }
    }
}
