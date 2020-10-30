using System;
using System.Collections.Generic;
using System.Text;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.Data.Models.Stream.Twitch;
using NUnit.Framework;
using TwitchLib.Client.Models;

namespace BobReactRemaster.Tests.Models.Streams
{
    class TwitchCredentialTests
    {
        [Test]
        public void GetRelayConnectionCredentials_ValidObjectState_CorrectCredentialsObject()
        {
            TwitchCredential cred = new TwitchCredential();
            cred.Token = "Test";
            cred.ChatUserName = "bobreacttest";
            ConnectionCredentials testcred = cred.GetRelayConnectionCredentials();
            Assert.AreEqual("bobreacttest",testcred.TwitchUsername);
            Assert.AreEqual("oauth:Test",testcred.TwitchOAuth);
        }
        [Test]
        public void StreamClone_ValidObjectState_CorrectClonedObject()
        {
            TwitchCredential cred = new TwitchCredential();
            cred.Token = "Test";
            cred.isMainAccount = true;
            var clone = cred.StreamClone();
            Assert.AreNotSame(clone,cred);
        }
    }
}
