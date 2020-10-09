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
            ConnectionCredentials testcred = cred.GetRelayConnectionCredentials();
            Assert.AreEqual("bobreacttest",testcred.TwitchUsername);
            Assert.AreEqual("oauth:Test",testcred.TwitchOAuth);
        }
    }
}
