
using NUnit.Framework;
using System;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.Data.Models.User;

namespace BobRemastered.Tests
{
    public class UserTests
    {
        private Member user;
        [SetUp]
        public void Setup()
        {
            user = new Member("chromos33");
        }

        [Test]
        public void Constructor_ValidUserName_UserNameSet()
        {
            

            Assert.AreEqual(user.UserName,"chromos33");
        }
        [Test]
        public void Constructor_ValidUserName_EmptySubscriptionList()
        {
            Assert.AreEqual(0,user.SubscriptionCount());

        }
        [Test]
        public void AddStreamSubscription_StreamObject_SubscriptionListCountEqualsOne()
        {
            iLiveStream stream = new TwitchStream("Stream1");
            user.AddStreamSubscription(stream);
            Assert.AreEqual(1, user.SubscriptionCount());
        }
        [Test]
        public void HasSubscriptions_EmptyList_ReturnsFalse()
        {
            Assert.IsFalse(user.HasSubscriptions());
        }
        [Test]
        public void HasSubscriptions_FilledList_ReturnsFalse()
        {
            iLiveStream stream = new TwitchStream("Stream1");
            user.AddStreamSubscription(stream);
            Assert.IsTrue(user.HasSubscriptions());
        }
        [Test]
        public void HasSubscription_NullGiven_ThrowsException()
        {
            Assert.Throws(typeof(ArgumentNullException),delegate { user.HasSubscription(null); });
        }
        [Test]
        public void HasSubscription_LiveStreamNotInListGiven_ReturnsFalse()
        {
            iLiveStream stream = new TwitchStream("Stream1");

            Assert.IsFalse(user.HasSubscription(stream));
        }
        [Test]
        public void HasSubscription_LiveStreamInListGiven_ReturnsTrue()
        {
            iLiveStream stream = new TwitchStream("Stream1");
            user.AddStreamSubscription(stream);

            Assert.IsTrue(user.HasSubscription(stream));
        }
        [Test]
        public void IsSubscribed_LiveStreamInListGiven_ReturnsTrue()
        {
            iLiveStream stream = new TwitchStream("Stream1");
            user.AddStreamSubscription(stream);

            Assert.IsTrue(user.IsSubscribed(stream));
        }
        [Test]
        public void IsSubscribed_LiveStreamNotInListGiven_ReturnsFalse()
        {
            iLiveStream stream = new TwitchStream("Stream1");

            Assert.IsFalse(user.IsSubscribed(stream));
        }
        [Test]
        public void IsSubscribed_NullGiven_ThrowsException()
        {
            Assert.Throws(typeof(ArgumentNullException), delegate { user.IsSubscribed(null); });
        }

        [Test]
        public void UnSubscribeStream_NullGiven_ThrowsException()
        {
            Assert.Throws(typeof(ArgumentNullException), delegate { user.UnSubscribeStream(null); });
        }
        [Test]
        public void UnSubscribeStream_StreamWithNoSubscriptionObject_ThrowsException()
        {
            iLiveStream stream = new TwitchStream("Stream1");
            Assert.Throws(typeof(NullReferenceException), delegate { user.UnSubscribeStream(stream); });
        }
        [Test]
        public void UnSubscribeStream_SubscriptionActive_DisablesSubscription()
        {
            iLiveStream stream = new TwitchStream("Stream1");
            user.AddStreamSubscription(stream);

            user.UnSubscribeStream(stream);
            Assert.IsFalse(user.IsSubscribed(stream));
        }
        [Test]
        public void SubscribeStream_NullGiven_ThrowsException()
        {
            Assert.Throws(typeof(ArgumentNullException), delegate { user.UnSubscribeStream(null); });
        }
        [Test]
        public void SubscribeStream_StreamWithNoSubscriptionObject_ThrowsException()
        {
            iLiveStream stream = new TwitchStream("Stream1");
            Assert.Throws(typeof(NullReferenceException), delegate { user.UnSubscribeStream(stream); });
        }
        [Test]
        public void SubscribeStream_SubscriptionActive_DisablesSubscription()
        {
            iLiveStream stream = new TwitchStream("Stream1");
            user.AddStreamSubscription(stream);

            user.SubscribeStream(stream);
            Assert.IsTrue(user.IsSubscribed(stream));
            user.UnSubscribeStream(stream);
            Assert.IsFalse(user.IsSubscribed(stream));
            user.SubscribeStream(stream);
            Assert.IsTrue(user.IsSubscribed(stream));
        }
    }
}