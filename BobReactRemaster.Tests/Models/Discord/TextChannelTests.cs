using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using BobReactRemaster.Data.Models.Discord;
using Discord.WebSocket;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace BobRemastered.Tests.Models.Discord
{
    class TextChannelTests
    {

        [Test]
        public void Update_ValidData_CorrectUpdatedState()
        {
            var textchat = new TextChannel(54,"test","");
            var ChannelName = "Channel";
            var GuildName = "Guild";

           
            textchat.Update(ChannelName,GuildName);
            ClassicAssert.AreEqual(ChannelName,textchat.Name);
            ClassicAssert.AreEqual(GuildName,textchat.Guild);
        }
        [Test]
        public void Constructor_ValidData_CorrectInitialState()
        {

            var textchat = new TextChannel(45,"test","");
            var ChannelName = "Channel";
            var GuildName = "Guild";


            textchat.Update(ChannelName, GuildName);
            ClassicAssert.AreEqual(ChannelName, textchat.Name);
            ClassicAssert.AreEqual(GuildName, textchat.Guild);
        }
    }
}
