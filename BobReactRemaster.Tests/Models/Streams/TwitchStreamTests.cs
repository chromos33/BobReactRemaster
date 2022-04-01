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

        [Test]
        public void GetStreamListData_ValidTwitchStream_ValidReturn()
        {
            var streamname = "Stream";
            var stream = new TwitchStream(streamname);
            stream.Id = 5;
            var result = stream.GetStreamListData();
            Assert.AreEqual(5,result.ID);
            Assert.AreEqual(streamname,result.Name);
            Assert.AreEqual(StreamState.Stopped,result.StreamState);
        }

        [Test]
        public void GetUpTimeTask_ValidTask()
        {
            var streamname = "Stream";
            var stream = new TwitchStream(streamname);
            
            stream.StartStream();
            stream.UpTimeInterval = 15;
            var Started = stream.Started;
            var Task = stream.GetUpTimeTask();
            Assert.IsFalse(Task.Executable());

        }

        [Test]
        public void HasStaticCommands()
        {
            var streamname = "Stream";
            var stream = new TwitchStream(streamname);
            Assert.IsFalse(stream.HasStaticCommands());
            stream.SetTwitchCredential(new TwitchCredential());
            Assert.IsTrue(stream.HasStaticCommands());
        }

        [Test]
        public void UnsetRelayChannel_ValidStates()
        {
            var streamname = "Stream";
            var stream = new TwitchStream(streamname);
            stream.SetRelayChannel(new TextChannel(UInt64.MinValue,"test","test" ));
            Assert.IsNotNull(stream.RelayChannel);
            stream.UnsetRelayChannel();
            Assert.IsNull(stream.RelayChannel);
        }

        [Test]
        public void GetStreamStartedMessage_ValidResponse()
        {
            var streamname = "Stream";
            var stream = new TwitchStream(streamname);
            var titel = "test";
            var result = stream.GetStreamStartedMessage(titel);
            var expected = $"{streamname} hat angefangen {titel} zu streamen.";
            Assert.AreEqual(expected,expected);

        }

    }
}
