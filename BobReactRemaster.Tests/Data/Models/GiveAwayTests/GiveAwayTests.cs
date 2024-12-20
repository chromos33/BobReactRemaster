﻿using BobReactRemaster.Data.Models.GiveAways;
using BobReactRemaster.Data.Models.User;
using BobReactRemaster.Exceptions;
using BobReactRemaster.Tests.Helper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework.Legacy;


namespace BobReactRemaster.Tests.Data.Models.GiveAwayTests
{
    internal class GiveAwayTests
    {
        //Test
        //AddOrUpdateGift(Gift)
        [Test]
        public void AddGift_NewGift_CorrectlyAddedToList()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test","test",UserRole.User);
            Gift gift = new Gift(member,"https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            testCandidate.AddGift(gift);
            ClassicAssert.IsTrue(testCandidate.Gifts.Any(x => (x.ID == 0 && x.Name == "Farming Simulator 22")));
        }
        [Test]
        public void AddGift_SameGift_DuplicateKeyExceptionThrown()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            Gift gift = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift.ID = 1;
            testCandidate.AddGift(gift);
            Assert.Throws<DuplicateKeyException>(() => { testCandidate.AddGift(gift); });
        }
        [Test]
        public void UpdateGift_NewGift_NotFoundExceptionThrown()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            Gift gift = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift.ID = 1;
            Assert.Throws<NotFoundException>(() => { testCandidate.UpdateGift(gift); });
        }
        //TODO later when nextGift is tested and implemented make Gifts private and just get the first gift with the nextGift function
        [Test]
        public void UpdateGift_ChangedGift_GiftUpdated()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            Gift gift = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift.ID = 1;
            testCandidate.AddGift(gift);
            gift.Key = "test";
            gift.Name = "Farming Simulator 23";
            testCandidate.UpdateGift(gift);
            ClassicAssert.IsTrue(testCandidate.Gifts.First().Name == "Farming Simulator 23" && testCandidate.Gifts.First().Key == "test");

        }
        //RemoveGift(Gift)
        [Test]
        public void RemoveGift_NewGift_NotFoundExceptionThrown()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            Gift gift = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift.ID = 1;
            Assert.Throws<NotFoundException>(() => { testCandidate.UpdateGift(gift); });
        }
        [Test]
        public void RemoveGift_ExistantGift_GiftRemoved()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            Gift gift = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift.ID = 1;
            testCandidate.AddGift(gift);
           
            testCandidate.RemoveGift(gift);
            ClassicAssert.IsTrue(testCandidate.Gifts.Count() == 0);
        }
        [Test]
        public void RemoveGift_ExistantGift_CorrectGiftRemoved()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            Gift gift = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift.ID = 1;
            testCandidate.AddGift(gift);
            Gift gift2 = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift.ID = 2;
            testCandidate.AddGift(gift2);

            testCandidate.RemoveGift(gift);
            ClassicAssert.IsTrue(!testCandidate.Gifts.Any(x => x.ID == 1));
        }
        //AddParticipant()
        //RemoveParticipant()
        //GetParticipants() returns all current Participants
        [Test]
        public void AddParticipant_NewParticipant_CorrectlyAdded()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            testCandidate.AddParticipant(member);
            ClassicAssert.IsTrue(testCandidate.GetParticipants().Any(x => x.UserName == member.UserName));
        }
        [Test]
        public void AddParticipant_ExistantParticipant_ThrowsDuplicateKeyException()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);

            testCandidate.AddParticipant(member);
            Assert.Throws<DuplicateKeyException>(() => { testCandidate.AddParticipant(member); });
        }
        [Test]
        public void RemoveParticipant_NewParticipant_NotFoundExceptionThrown()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            
            Assert.Throws<NotFoundException>(() => { testCandidate.RemoveParticipant(member); });
        }
        [Test]
        public void RemoveParticipant_ExistantParticipant_GiftRemoved()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            testCandidate.AddParticipant(member);

            testCandidate.RemoveParticipant(member);
            ClassicAssert.IsTrue(testCandidate.GetParticipants().Count() == 0);
        }
        [Test]
        public void RemoveParticipant_ExistantParticipant_CorrectParticipantRemoved()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            Member member2 = new Member("test2", "test", UserRole.User);
            testCandidate.AddParticipant(member);
            testCandidate.AddParticipant(member2);

            testCandidate.RemoveParticipant(member);
            ClassicAssert.IsTrue(!testCandidate.GetParticipants().Any(x => x.UserName == member.UserName));
        }
        //NextGift() - Randomly gets the next Gift in line for Raffling considering current "turn"
        [Test]
        public void NextGift_EmptyList_ThrowEmptyListExeption()
        {
            GiveAway testCandidate = new GiveAway();
            TestableRandomGenerator rng = new TestableRandomGenerator();
            Assert.Throws<EmptyListExeption>(() => { testCandidate.NextGift(rng); });
        }

        [Test]
        public void NextGift_FilledList_GiftReturned()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            Gift gift = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift.ID = 1;
            testCandidate.AddGift(gift);
            Gift gift2 = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift2.ID = 2;
            testCandidate.AddGift(gift2);
            
            TestableRandomGenerator rng = new TestableRandomGenerator();
            rng.SetResult(0);
            ClassicAssert.IsTrue(testCandidate.NextGift(rng).ID == 1);
        }

        [Test]
        public void Raffle_NoParticipants_ThrowsEmpyListException()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            Gift gift = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift.ID = 1;
            testCandidate.AddGift(gift);
            Gift gift2 = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift2.ID = 2;
            testCandidate.AddGift(gift2);

            TestableRandomGenerator rng = new TestableRandomGenerator();
            rng.SetResult(1);
            Assert.Throws<EmptyListExeption>(() => { testCandidate.Raffle(rng); });
        }

        [Test]
        public void Raffle_Participants_ReturnsWinner()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            member.TestWonGiftsCount = 1;
            Member member2 = new Member("test2", "test", UserRole.User);
            Member member3 = new Member("test3", "test", UserRole.User);
            testCandidate.AddParticipant(member);
            testCandidate.AddParticipant(member2);
            testCandidate.AddParticipant(member3);
            Gift gift = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift.ID = 1;
            testCandidate.AddGift(gift);
            Gift gift2 = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift2.ID = 2;
            gift2.IsCurrent = true;
            testCandidate.AddGift(gift2);

            TestableRandomGenerator rng = new TestableRandomGenerator();
            rng.SetResult(0);
            List<Member> result = testCandidate.Raffle(rng);
            ClassicAssert.IsTrue(result.First().UserName == member2.UserName);
        }
        [Test]
        public void Raffle_ParticipantsAllDifferentWonItemCount_ReturnsWinner()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            member.TestWonGiftsCount = 1;
            Member member2 = new Member("test2", "test", UserRole.User);
            member2.TestWonGiftsCount = 2;
            Member member3 = new Member("test3", "test", UserRole.User);
            member3.TestWonGiftsCount = 3;
            testCandidate.AddParticipant(member);
            testCandidate.AddParticipant(member2);
            testCandidate.AddParticipant(member3);
            Gift gift = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift.ID = 1;
            testCandidate.AddGift(gift);
            Gift gift2 = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift2.ID = 2;
            gift2.IsCurrent = true;
            testCandidate.AddGift(gift2);

            TestableRandomGenerator rng = new TestableRandomGenerator();
            rng.SetResult(0);
            List<Member> result = testCandidate.Raffle(rng);
            ClassicAssert.IsTrue(result.First().UserName == member.UserName);
        }

        [Test]
        public void RaffleFFA_Participants_ReturnsWinner()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            member.TestWonGiftsCount = 1;
            Member member2 = new Member("test2", "test", UserRole.User);
            member2.TestWonGiftsCount = 2;
            Member member3 = new Member("test3", "test", UserRole.User);
            member3.TestWonGiftsCount = 3;
            testCandidate.AddParticipant(member);
            testCandidate.AddParticipant(member2);
            testCandidate.AddParticipant(member3);
            Gift gift = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift.ID = 1;
            testCandidate.AddGift(gift);
            Gift gift2 = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift2.ID = 2;
            gift2.IsCurrent = true;
            testCandidate.AddGift(gift2);

            TestableRandomGenerator rng = new TestableRandomGenerator();
            rng.SetResult(1);
            List<Member> result = testCandidate.RaffleFFA(rng);
            ClassicAssert.IsTrue(result.First().UserName == member2.UserName);
        }

        [Test]
        public void RaffleWeightedFFA_Participants_ReturnsWinner()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            member.TestWonGiftsCount = 1;
            Member member2 = new Member("test2", "test", UserRole.User);
            member2.TestWonGiftsCount = 2;
            Member member3 = new Member("test3", "test", UserRole.User);
            member3.TestWonGiftsCount = 3;
            testCandidate.AddParticipant(member);
            testCandidate.AddParticipant(member2);
            testCandidate.AddParticipant(member3);
            Gift gift = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift.ID = 1;
            testCandidate.AddGift(gift);
            Gift gift2 = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift2.ID = 2;
            gift2.IsCurrent = true;
            testCandidate.AddGift(gift2);

            TestableRandomGenerator rng = new TestableRandomGenerator();
            rng.SetResult(0);
            List<Member> result = testCandidate.RaffleWeightedFFA(rng);
            //Because it being weighted member should have the second slot as well (in the "random table")
            ClassicAssert.IsTrue(result.First().UserName == member.UserName);
        }
        //***

        [Test]
        public void Raffle_Participants_ReturnsMultipleWinners()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            member.TestWonGiftsCount = 1;
            Member member2 = new Member("test2", "test", UserRole.User);
            Member member3 = new Member("test3", "test", UserRole.User);
            testCandidate.AddParticipant(member);
            testCandidate.AddParticipant(member2);
            testCandidate.AddParticipant(member3);
            Gift gift = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift.ID = 1;
            testCandidate.AddGift(gift);
            Gift gift2 = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift2.ID = 2;
            gift2.IsCurrent = true;
            testCandidate.AddGift(gift2);

            TestableRandomGenerator rng = new TestableRandomGenerator();
            rng.SetResult(0);
            List<Member> result = testCandidate.Raffle(rng);
            ClassicAssert.IsTrue(result.First().UserName == member2.UserName);
            ClassicAssert.IsTrue(result[1].UserName == member3.UserName);
        }

        [Test]
        public void RaffleFFA_Participants_ReturnsMultipleWinners()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            member.TestWonGiftsCount = 1;
            Member member2 = new Member("test2", "test", UserRole.User);
            member2.TestWonGiftsCount = 2;
            Member member3 = new Member("test3", "test", UserRole.User);
            member3.TestWonGiftsCount = 3;
            testCandidate.AddParticipant(member);
            testCandidate.AddParticipant(member2);
            testCandidate.AddParticipant(member3);
            Gift gift = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift.ID = 1;
            testCandidate.AddGift(gift);
            Gift gift2 = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift2.ID = 2;
            gift2.IsCurrent = true;
            testCandidate.AddGift(gift2);

            TestableRandomGenerator rng = new TestableRandomGenerator();
            rng.AddResult(1);
            rng.AddResult(1);
            List<Member> result = testCandidate.RaffleFFA(rng);
            ClassicAssert.IsTrue(result.First().UserName == member2.UserName);
            ClassicAssert.IsTrue(result[1].UserName == member3.UserName);
        }

        [Test]
        public void RaffleWeightedFFA_Participants_ReturnsMultipleWinners()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            member.TestWonGiftsCount = 1;
            Member member2 = new Member("test2", "test", UserRole.User);
            member2.TestWonGiftsCount = 2;
            Member member3 = new Member("test3", "test", UserRole.User);
            member3.TestWonGiftsCount = 3;
            testCandidate.AddParticipant(member);
            testCandidate.AddParticipant(member2);
            testCandidate.AddParticipant(member3);
            Gift gift = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift.ID = 1;
            testCandidate.AddGift(gift);
            Gift gift2 = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift2.ID = 2;
            gift2.IsCurrent = true;
            testCandidate.AddGift(gift2);

            TestableRandomGenerator rng = new TestableRandomGenerator();
            rng.SetResult(2);
            List<Member> result = testCandidate.RaffleWeightedFFA(rng);
            //Because it being weighted member should have the second slot as well (in the "random table")
            ClassicAssert.IsTrue(result.First().UserName == member.UserName);
            ClassicAssert.IsTrue(result[1].UserName == member2.UserName);
        }

        //***

        [Test]
        public void Raffle_Participants_ParticipantsListCleared()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            member.TestWonGiftsCount = 1;
            Member member2 = new Member("test2", "test", UserRole.User);
            Member member3 = new Member("test3", "test", UserRole.User);
            testCandidate.AddParticipant(member);
            testCandidate.AddParticipant(member2);
            testCandidate.AddParticipant(member3);
            Gift gift = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift.ID = 1;
            testCandidate.AddGift(gift);
            Gift gift2 = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift2.ID = 2;
            gift2.IsCurrent = true;
            testCandidate.AddGift(gift2);

            TestableRandomGenerator rng = new TestableRandomGenerator();
            rng.AddResult(1);
            rng.AddResult(0);
            List<Member> result = testCandidate.Raffle(rng);
            ClassicAssert.IsTrue(testCandidate.GetParticipants().Count == 0);
        }

        [Test]
        public void RaffleFFA_Participants_ParticipantsListCleared()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            member.TestWonGiftsCount = 1;
            Member member2 = new Member("test2", "test", UserRole.User);
            member2.TestWonGiftsCount = 2;
            Member member3 = new Member("test3", "test", UserRole.User);
            member3.TestWonGiftsCount = 3;
            testCandidate.AddParticipant(member);
            testCandidate.AddParticipant(member2);
            testCandidate.AddParticipant(member3);
            Gift gift = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift.ID = 1;
            testCandidate.AddGift(gift);
            Gift gift2 = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift2.ID = 2;
            gift2.IsCurrent = true;
            testCandidate.AddGift(gift2);

            TestableRandomGenerator rng = new TestableRandomGenerator();
            rng.SetResult(0);
            List<Member> result = testCandidate.RaffleFFA(rng);
            ClassicAssert.IsTrue(testCandidate.GetParticipants().Count == 0);
        }

        [Test]
        public void RaffleWeightedFFA_Participants_ParticipantsListCleared()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            member.TestWonGiftsCount = 1;
            Member member2 = new Member("test2", "test", UserRole.User);
            member2.TestWonGiftsCount = 2;
            Member member3 = new Member("test3", "test", UserRole.User);
            member3.TestWonGiftsCount = 3;
            testCandidate.AddParticipant(member);
            testCandidate.AddParticipant(member2);
            testCandidate.AddParticipant(member3);
            Gift gift = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift.ID = 1;
            testCandidate.AddGift(gift);
            Gift gift2 = new Gift(member, "https://store.steampowered.com/app/1248130/Farming_Simulator_22/", "Farming Simulator 22");
            gift2.ID = 2;
            gift2.IsCurrent = true;
            testCandidate.AddGift(gift2);

            TestableRandomGenerator rng = new TestableRandomGenerator();
            rng.SetResult(2);
            List<Member> result = testCandidate.RaffleWeightedFFA(rng);
            //Because it being weighted member should have the second slot as well (in the "random table")
            ClassicAssert.IsTrue(testCandidate.GetParticipants().Count == 0);
        }

        //Increment "Turn" on all Gifts with the same Link (trim and clean for parameters)
        // Raffle() - Randomly choose a Winner from the List of Particpants excluding all but the participants with the least amount of won gifts
        // RaffleFFA() - Randomly choose a winner from the List of Particpants
        // RaffleWeightedFFA() - Randomly Choose a winner from the List of Participants Weighting people better with less Gifts won

        //Clear current Participants and or just use 1 List of Participants that gets cleared on raffle/NextGift
        [Test]
        public void AddAdmin_NewAdmin_CorrectlyAddedToList()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            testCandidate.AddAdmin(member);
            ClassicAssert.IsTrue(testCandidate.Admins.Any(x => x.Member.UserName == "test"));
        }
        [Test]
        public void AddAdmin_AlreadyAdmin_DuplicateKeyExceptionThrown()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            testCandidate.AddAdmin(member);
            Assert.Throws<DuplicateKeyException>(() => { testCandidate.AddAdmin(member); });
        }
        [Test]
        public void RemoveAdmin_NewAdmin_NotFoundExceptionThrown()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            
            Assert.Throws<NotFoundException>(() => { testCandidate.RemoveAdmin(member); });
        }
        [Test]
        public void RemoveAdmin_ExistantAdmin_AdminRemoved()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            testCandidate.AddAdmin(member);

            testCandidate.RemoveAdmin(member);
            ClassicAssert.IsTrue(!testCandidate.Admins.Any());
        }
        [Test]
        public void IsAdmin_CorrectAdminState()
        {
            GiveAway testCandidate = new GiveAway();
            Member member = new Member("test", "test", UserRole.User);
            ClassicAssert.IsFalse(testCandidate.IsAdmin(member));
            testCandidate.AddAdmin(member);
            ClassicAssert.IsTrue(testCandidate.IsAdmin(member));
            testCandidate.RemoveAdmin(member);
            ClassicAssert.IsFalse(testCandidate.IsAdmin(member));
        }
    }
}
