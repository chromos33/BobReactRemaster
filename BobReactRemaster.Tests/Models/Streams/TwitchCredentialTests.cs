﻿using System;
using System.Collections.Generic;
using System.Text;
using BobReactRemaster.Controllers;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.Data.Models.Stream.Twitch;
using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.AreEqual("bobreacttest",testcred.TwitchUsername);
            ClassicAssert.AreEqual("oauth:Test",testcred.TwitchOAuth);
        }
        [Test]
        public void StreamClone_ValidObjectState_CorrectClonedObject()
        {
            TwitchCredential cred = new TwitchCredential();
            cred.Token = "Test";
            cred.isMainAccount = true;
            var clone = cred.StreamClone();
            ClassicAssert.AreNotSame(clone,cred);
        }

        [Test]
        public void setFromTwitchOauthStoreData_ValidParameter()
        {
            var Credential = new TwitchCredential();
            var data = new TwitchOauthStoreData();
            data.ClientID = "ClientID";
            data.Secret = "Secret";
            data.ChatUserName = "ChatUserName";
            Credential.setFromTwitchOauthStoreData(data);
            ClassicAssert.AreEqual(data.ClientID,Credential.ClientID);
            ClassicAssert.AreEqual(data.Secret,Credential.Secret);
            ClassicAssert.AreEqual(data.ChatUserName,Credential.ChatUserName);
        }

        [Test]
        public void getTwitchReturnURL_ValidParameterString()
        {
            var Credential = new TwitchCredential();
            string webserverAddress = "test.local";
            string expected = webserverAddress + "/Twitch/TwitchOAuthReturn";
            ClassicAssert.AreEqual(expected,Credential.getTwitchReturnURL(webserverAddress));
            ClassicAssert.AreEqual(expected,Credential.getTwitchReturnURL(webserverAddress+"/"));
        }

        [Test]
        public void getTwitchAuthLink_ValidParameters()
        {
            string webserverAddress = "test.local";
            var Credential = new TwitchCredential();
            var data = new TwitchOauthStoreData();
            data.ClientID = "ClientID";
            data.Secret = "Secret";
            data.ChatUserName = "ChatUserName";
            data.Scopes = "scope1|scope2|scope3";
            Credential.setFromTwitchOauthStoreData(data);
            var guid = Guid.NewGuid().ToString();
            var result = Credential.getTwitchAuthLink(data, webserverAddress,guid);
            var returnurl = Credential.getTwitchReturnURL(webserverAddress);
            var expected = $"https://id.twitch.tv/oauth2/authorize?response_type=code&client_id={data.ClientID}&redirect_uri={returnurl}&force_verify=true&scope=scope1+scope2+scope3&state={guid}";
            ClassicAssert.AreEqual(expected,result);
        }

        [Test]
        public void GetRelayConnectionCredentials_ValidConnectionCredentialReturned()
        {
            var ChatUsername = "UserName";
            var Token = "Token";
            var Credential = new TwitchCredential();
            Credential.ChatUserName = ChatUsername;
            Credential.Token = Token;
            ConnectionCredentials result = Credential.GetRelayConnectionCredentials();
            ClassicAssert.AreNotEqual(Credential.ChatUserName,result.TwitchUsername);
            ClassicAssert.AreEqual(Credential.ChatUserName.ToLower(),result.TwitchUsername);
            ClassicAssert.AreEqual("oauth:"+Credential.Token,result.TwitchOAuth);
        }

    }
}
