using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.Data.Models.Stream.Twitch;
using BobReactRemaster.EventBus;
using BobReactRemaster.EventBus.MessageDataTypes;
using BobReactRemaster.Services.Chat.Command.Commands.Twitch;
using BobReactRemaster.Services.Chat.Command.Messages;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace BobReactRemaster.Tests.Services.Chat.Command.Twitch
{
    class TwitchGameChangeCommandTests
    {
        [Test]
        public void IsTriggerable_validMessage_ReturnsTrue()
        {
            var Stream = new TwitchStream("test");
            Stream.SetTwitchCredential(new TwitchCredential(){  Token = "adsf", ClientID = "test", isMainAccount = false});
            var Command = new TwitchGameChangeCommand(null,Stream);
            Assert.IsTrue(Command.IsTriggerable(new TwitchCommandMessage("!game test","","",false)));
        }
        [Test]
        public void IsTriggerable_invalidMessage_ReturnsFalse()
        {
            var Stream = new TwitchStream("test");
            Stream.SetTwitchCredential(new TwitchCredential() { Token = "adsf", ClientID = "test", isMainAccount = false });
            var Command = new TwitchGameChangeCommand(null, Stream);
            Assert.IsFalse(Command.IsTriggerable(new TwitchCommandMessage("! game test", "", "",false)));
        }
    }
}
