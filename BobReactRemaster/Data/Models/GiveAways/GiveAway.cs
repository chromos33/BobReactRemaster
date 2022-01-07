using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.Data.Models.User;
using BobReactRemaster.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BobReactRemaster.Exceptions;
using IdentityServer4.Extensions;
using Microsoft.EntityFrameworkCore.Internal;
using Member = BobReactRemaster.Data.Models.User.Member;

namespace BobReactRemaster.Data.Models.GiveAways
{
    public class GiveAway
    {
        [Key]
        public int ID { get; private set; }

        public string Name { get; private set; }
        public TextChannel TextChannel { get; private set; }

        public List<GiveAway_Member> Admins { get; private set; } = new List<GiveAway_Member>();

        public List<Gift> Gifts { get; private set; } = new List<Gift>();

        private List<User.Member> Participants { get; set; } = new List<User.Member>();

        public void AddGift(Gift gift)
        {
            if (gift.ID > 0 && Gifts.Any(x => x.ID == gift.ID))
            {
                throw new DuplicateKeyException("There is already a gift with that ID run UpdateGift instead");
            }
            Gifts.Add(gift);
        }

        public void UpdateGift(Gift gift)
        {
            if (gift.ID == 0 || Gifts.All(x => x.ID != gift.ID))
            {
                throw new NotFoundException("There is no gift with that ID run AddGift to add");
            }

            Gift update = Gifts.FirstOrDefault(x => x.ID == gift.ID);
            if (update != null)
            {
                update = gift;
            }
        }

        public void RemoveGift(Gift gift)
        {
            if (gift.ID == 0 || Gifts.All(x => x.ID != gift.ID))
            {
                throw new NotFoundException("There is no gift with that ID run AddGift to add");
            }

            Gifts.RemoveAll(x => x.ID == gift.ID);

        }

        public void AddParticipant(User.Member member)
        {
            if (!member.UserName.IsNullOrEmpty() && Participants.Any(x => x.UserName == member.UserName))
            {
                throw new DuplicateKeyException("There Member is already in the List");
            }
            Participants.Add(member);
        }

        public List<User.Member> GetParticipants()
        {
            return Participants;
        }

        public void RemoveParticipant(User.Member member)
        {
            if (member.UserName.IsNullOrEmpty() || Participants.All(x => x.UserName != member.UserName))
            {
                throw new NotFoundException("This user is not in the List thus cannot be removed");
            }

            Participants.RemoveAll(x => x.UserName == member.UserName);
        }
        //TODO add algorithm to increment Turn on IdenticalGifts
        public Gift NextGift(IRandomGenerator random)
        {
            if (Gifts.Count == 0)
            {
                throw new EmptyListExeption("The gift list is empty.");
            }
            int minTurns = Gifts.Min(x => x.Turn);
            var TurnFilteredGifts = Gifts.Where(x => x.Turn == minTurns);
            if (TurnFilteredGifts.Count() == 1)
            {
                return TurnFilteredGifts.FirstOrDefault();
            }
            return TurnFilteredGifts.ElementAt(random.Generate(0, TurnFilteredGifts.Count() - 1));
        }

        private IEnumerable<Gift> getIdenticalFreeGifts(Gift gift)
        {
            return Gifts.Where(x => x.Winner == null && x.CleanedLink() == gift.CleanedLink());
        }

        public List<User.Member> Raffle(IRandomGenerator rng)
        {
            if (Participants.Count == 0)
            {
                throw new EmptyListExeption("No users applied to this Raffle");
            }
            var mainGift = Gifts.FirstOrDefault(x => x.IsCurrent);
            //Performance path if only 1 participant thus no logic applied to safe execution time
            if (Participants.Count == 1)
            {
                return new List<Member>() { Participants.First() };
            }
            
                
            List<User.Member> Winners = new List<User.Member>();
            while (Participants.Count > 0 && getIdenticalFreeGifts(mainGift).Any())
            {
                Gift nextGift = getIdenticalFreeGifts(mainGift).FirstOrDefault();
                int GiftMinFilter = Participants.Min(x => x.GetWonGiftsCount());
                var RaffleMembers = Participants.Where(x => x.GetWonGiftsCount() == GiftMinFilter);
                var Winner = doRaffle(RaffleMembers,nextGift,rng);
                if (Winner != null)
                {
                    Winners.Add(Winner);
                    Participants.Remove(Winner);
                }
            }

            clearParticipants();
            return Winners;
        }

        private void clearParticipants()
        {
            Participants = new List<Member>();
        }
        private User.Member doRaffle(IEnumerable<Member> RaffleMembers, Gift gift,IRandomGenerator rng)
        {
            int rngnum = rng.Generate(0, RaffleMembers.Count() - 1);
            var Winner = RaffleMembers.ElementAt(rngnum);
            gift.Winner = Winner;
            return Winner;
        }

        public List<User.Member> RaffleFFA(IRandomGenerator rng)
        {
            if (Participants.Count == 0)
            {
                throw new EmptyListExeption("No users applied to this Raffle");
            }
            var mainGift = Gifts.FirstOrDefault(x => x.IsCurrent);
            //Performance path if only 1 participant thus no logic applied to safe execution time
            if (Participants.Count == 1)
            {
                return new List<Member>() { Participants.First() };
            }


            List<User.Member> Winners = new List<User.Member>();
            while (Participants.Count > 0 && getIdenticalFreeGifts(mainGift).Any())
            {
                Gift nextGift = getIdenticalFreeGifts(mainGift).FirstOrDefault();
                var Winner = doRaffle(Participants, nextGift, rng);
                if (Winner != null)
                {
                    Winners.Add(Winner);
                    Participants.Remove(Winner);
                }
            }

            clearParticipants();
            return Winners;
        }
        public List<User.Member> RaffleWeightedFFA(IRandomGenerator rng)
        {
            if (Participants.Count == 0)
            {
                throw new EmptyListExeption("No users applied to this Raffle");
            }
            var mainGift = Gifts.FirstOrDefault(x => x.IsCurrent);
            //Performance path if only 1 participant thus no logic applied to safe execution time
            if (Participants.Count == 1)
            {
                return new List<Member>() { Participants.First() };
            }


            List<User.Member> Winners = new List<User.Member>();
            while (Participants.Count > 0 && getIdenticalFreeGifts(mainGift).Any())
            {
                Gift nextGift = getIdenticalFreeGifts(mainGift).FirstOrDefault();
                int TotalChances = 0;
                foreach (Member Member in Participants)
                {
                    //+1 value to count 0 gifts as 1 to not have infinite chance
                    if (TotalChances == 0)
                    {
                        TotalChances = Member.GetWonGiftsCount() + 1;
                    }
                    else
                    {

                        TotalChances *= (Member.GetWonGiftsCount() + 1);
                    }
                }
                List<Tuple<Member, int,int>> ChanceMapping = new List<Tuple<Member, int,int>>();
                //Test start at index 0 "-1" to compensate for initial increment
                int currentChanceStart = -1;
                foreach (Member Member in Participants)
                {
                    int min = currentChanceStart + 1;
                    int max = currentChanceStart + (TotalChances / (Member.GetWonGiftsCount() + 1));
                    currentChanceStart = max;
                    ChanceMapping.Add(new Tuple<Member, int, int>(Member,min,max));
                }

                var test = ChanceMapping;
                TotalChances += Participants.Count - 1;
                var rngresult = rng.Generate(0, TotalChances);
                var Winner = ChanceMapping.FirstOrDefault(x => rngresult >= x.Item2 && rngresult < x.Item3)?.Item1;
                if (Winner != null)
                {
                    Winners.Add(Winner);
                    nextGift.Winner = Winner;
                    Participants.Remove(Winner);
                }
            }

            clearParticipants();
            return Winners;
        }

        public GiveAway_Member AddAdmin(Member member)
        {
            if (Admins.Any(x => x.Member.UserName == member.UserName))
            {
                throw new DuplicateKeyException("This user is already an admin for this giveaway");
            }

            GiveAway_Member connection = new GiveAway_Member();
            connection.Member = member;
            connection.GiveAway = this;
            Admins.Add(connection);
            return connection;
        }

        public void RemoveAdmin(Member member)
        {
            if (Admins.All(x => x.Member.UserName != member.UserName))
            {
                throw new NotFoundException("This user is not an admin for this giveaway");
            }

            Admins.RemoveAll(x => x.Member.UserName == member.UserName);
        }

        public bool IsAdmin(Member member)
        {
            return Admins.Any(x => x.Member.UserName == member.UserName);
        }
    }
}
