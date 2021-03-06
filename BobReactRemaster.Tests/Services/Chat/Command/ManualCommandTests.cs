﻿using System;
using System.Collections.Generic;
using System.Text;
using BobReactRemaster.EventBus;
using BobReactRemaster.Services.Chat.Command.Commands;
using BobReactRemaster.Services.Chat.Command.Messages;
using NUnit.Framework;

namespace BobReactRemaster.Tests.Services.Chat.Command
{
    class ManualCommandTests
    {
        [Test]
        public void IsTriggerable_ValidCommandTrigger_ReturnsResponse()
        {
            var Trigger = "!command";
            var Response = "response";
            var Command = new ManualRelayCommand(Trigger,Response, new MessageBus(),null);
            var CommandMessage = new TwitchCommandMessage(Trigger,"","",false);
            Assert.IsTrue(Command.IsTriggerable(CommandMessage));
        }
        [Test]
        public void IsTriggerable_InValidCommandTrigger_ReturnsResponse()
        {
            var Trigger = "!command";
            var Response = "response";
            var Command = new ManualRelayCommand(Trigger, Response, new MessageBus(),null);
            var CommandMessage = new TwitchCommandMessage("!other","","",false);
            Assert.IsFalse(Command.IsTriggerable(CommandMessage));
        }
    }
}
