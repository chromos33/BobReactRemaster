using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.Data.Models.User;
using BobReactRemaster.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BobReactRemaster.Data.Models.GiveAways
{
    public class GiveAway
    {
        [Key]
        public int ID { get; private set; }

        public string Name { get; private set; }
        public TextChannel TextChannel { get; private set; }

        public List<GiveAway_Member> Admins { get; private set; }

        public List<Gift> Gifts { get; private set; }

        private List<User.Member> Participants { get; set; } = new List<User.Member>();

        public void AddGift(Gift gift)
        {
            throw new NotImplementedException();
        }

        public void UpdateGift(Gift gift)
        {
            throw new NotImplementedException();
        }

        public void RemoveGift(Gift gift)
        {
            throw new NotImplementedException();
        }

        public void AddParticipant(User.Member member)
        {
            throw new NotImplementedException();
        }

        public List<User.Member> GetParticipants()
        {
            throw new NotImplementedException();
        }

        public void RemoveParticipant(User.Member member)
        {
            throw new NotImplementedException();
        }

        public Gift NextGift(IRandomGenerator random)
        {
            throw new NotImplementedException();
        }

        public List<User.Member> Raffle(IRandomGenerator rng)
        {
            throw new NotImplementedException();
        }
        public List<User.Member> RaffleFFA(IRandomGenerator rng)
        {
            throw new NotImplementedException();
        }
        public List<User.Member> RaffleWeightedFFA(IRandomGenerator rng)
        {
            throw new NotImplementedException();
        }
    }
}
