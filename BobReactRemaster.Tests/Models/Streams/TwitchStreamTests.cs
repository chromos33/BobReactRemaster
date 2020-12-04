using NUnit.Framework;
using System;
using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.Data.Models.Stream.Twitch;

namespace BobRemastered.Tests.Models.Streams
{
    class TwitchStreamTests
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void Constructor_ValidStreamName_StreamNameSet()
        {
            TwitchStream stream = new TwitchStream("SomeStream");

            Assert.AreEqual(stream.StreamName, "SomeStream");

        }
        [Test]
        public void Constructor_ValidStreamName_SubscriptionsNotNull()
        {
            TwitchStream stream = new TwitchStream("SomeStream");

            Assert.NotNull(stream.Subscriptions);

        }
        [Test]
        public void SetURL_ValidURL_URLSet()
        {
            TwitchStream stream = new TwitchStream("SomeStream");
            string streamurl = "http://twitch.tv/somestream";
            stream.SetURL(streamurl);

            Assert.AreEqual(stream.URL, streamurl);

        }
        [Test]
        public void StartStream_ValidDateTime_ValidStreamState()
        {
            TwitchStream stream = new TwitchStream("SomeStream");
            stream.StartStream();

            Assert.AreEqual(stream.State, StreamState.Running);

        }
        [Test]
        public void SetStreamStarted_DateTimeGiven_DateTimeSet()
        {
            TwitchStream stream = new TwitchStream("SomeStream");
            DateTime time = new DateTime(2020, 8, 17, 15, 30, 15);
            stream.SetStreamStarted(time);

            Assert.IsTrue(stream.Started.Equals(time));

        }
        [Test]
        public void StopStream_Nothing_ValidStreamState()
        {
            TwitchStream stream = new TwitchStream("SomeStream");
            Assert.AreEqual(stream.State, StreamState.Stopped);
            stream.StartStream();
            Assert.AreEqual(stream.State, StreamState.Running);
            stream.StopStream();
            Assert.AreEqual(stream.State, StreamState.Stopped);

        }
        [Test]
        public void SetRelayChannel_TextChannel_ValidRelayChannelSet()
        {
            TwitchStream stream = new TwitchStream("SomeStream");
            TextChannel test = new TextChannel(56,"Test","");
            stream.SetRelayChannel(test);
            Assert.AreEqual(stream.RelayChannel, test);

        }
    }
}
