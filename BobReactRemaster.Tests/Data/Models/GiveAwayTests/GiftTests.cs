using System;
using System.Collections.Generic;
using System.Text;
using BobReactRemaster.Data.Models.GiveAways;
using BobReactRemaster.Data.Models.User;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace BobReactRemaster.Tests.Data.Models.GiveAwayTests
{
    internal class GiftTests
    {
        //Test
        // Constructor()
        // AssignWinner() need better name but is called to relate the winning Member to the Gift
        [Test]
        [TestCase("https://store.steampowered.com/app/1142710/Total_War_WARHAMMER_III/", "https://store.steampowered.com/app/1142710/Total_War_WARHAMMER_III")]
        [TestCase("https://store.steampowered.com/app/1142710/Total_War_WARHAMMER_III/ ", "https://store.steampowered.com/app/1142710/Total_War_WARHAMMER_III")]
        [TestCase("https://store.steampowered.com/app/1142710/Total_War_WARHAMMER_III", "https://store.steampowered.com/app/1142710/Total_War_WARHAMMER_III")]
        public void CleanedLink_ContainsLink_LinkCorretlyCleaned(string link,string expectedresult)
        {
            Member member = new Member("Test", "test2");
            Gift gift = new Gift(member, link,"Some Name irrelevant for Test");
            ClassicAssert.AreEqual(expectedresult,gift.CleanedLink());
        }
    }
}
