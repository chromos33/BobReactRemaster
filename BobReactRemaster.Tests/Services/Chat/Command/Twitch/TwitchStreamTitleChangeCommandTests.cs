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
    class TwitchStreamTitleChangeCommandTests
    {
        [Test]
        public void IsTriggerable_validMessage_ReturnsTrue()
        {
            var Stream = new TwitchStream("test");
            Stream.SetTwitchCredential(new TwitchCredential(){  Token = "adsf", ClientID = "test", isMainAccount = false});
            var Command = new TwitchStreamTitleChangeCommand(null,Stream);
            Assert.IsTrue(Command.IsTriggerable(new TwitchCommandMessage("!title test","","")));
        }
        [Test]
        public void IsTriggerable_invalidMessage_ReturnsTrue()
        {
            var Stream = new TwitchStream("test");
            Stream.SetTwitchCredential(new TwitchCredential() { Token = "adsf", ClientID = "test", isMainAccount = false });
            var Command = new TwitchStreamTitleChangeCommand(null, Stream);
            Assert.IsFalse(Command.IsTriggerable(new TwitchCommandMessage("!game test", "", "")));
        }

        [Test]
        public void TriggerCommand_validMessage_TriggersCorrectBusMessage()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                }).Verifiable();
            var Stream = new TwitchStream("test");
            Stream.SetTwitchCredential(new TwitchCredential() { Token = "adsf", ClientID = "test", isMainAccount = false });
            var bus = new MessageBus();
            var Command = new TwitchStreamTitleChangeCommand(bus, Stream, new HttpClient(handlerMock.Object));
            bus.RegisterToEvent<TwitchRelayMessageData>((x) =>
            {
                Assert.AreEqual(x.Message,Command.UpdatedMessage);
            });
            Command.TriggerCommand(new TwitchCommandMessage("!title test","",""));  
        }
        [Test]
        public void TriggerCommand_invalidMessage_TriggersCorrectBusMessage()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                }).Verifiable();
            var Stream = new TwitchStream("test");
            Stream.SetTwitchCredential(new TwitchCredential() { Token = "adsf", ClientID = "test", isMainAccount = false });
            var bus = new MessageBus();
            var Command = new TwitchStreamTitleChangeCommand(bus, Stream, new HttpClient(handlerMock.Object));
            bus.RegisterToEvent<TwitchRelayMessageData>((x) =>
            {
                Assert.AreEqual(x.Message, Command.HelpMessage);
            });
            Command.TriggerCommand(new TwitchCommandMessage("!title", "", ""));
        }
    }
}
