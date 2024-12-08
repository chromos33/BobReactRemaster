using System;
using System.Collections.Generic;
using System.Text;
using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus;
using BobReactRemaster.EventBus.MessageDataTypes;
using BobReactRemaster.Services.Chat;
using BobReactRemaster.Services.Chat.Discord;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace BobReactRemaster.Tests.Services.Chat
{
    class RelayServiceTests
    {
        [Test]
        public void RelayMessage_ValidState_TriggersEvent()
        {
            var bus = new MessageBus();
            var testGuild = "Guild";
            var testChannel = "Channel";
            var testMessage = "Message";
            var StreamName = "TestStream";
            bus.RegisterToEvent<TwitchRelayMessageData>((x) =>
            {
                ClassicAssert.AreEqual(StreamName.ToLower(),x.StreamName.ToLower());
            });
            

            var RelayService = new RelayService(bus);
            var RelayChannel = new TextChannel(54,testChannel,testGuild);
            var LiveStream = new TwitchStream(StreamName);
            LiveStream.SetRelayChannel(RelayChannel);
            var LiveStreamList = new List<LiveStream>() {{LiveStream}};
            RelayService.RelayMessage(new RelayMessageFromDiscord(testGuild,testChannel,testMessage),LiveStreamList );
        }
    }
}
