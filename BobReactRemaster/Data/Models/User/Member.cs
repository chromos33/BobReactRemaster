using BobReactRemaster.Data.Models.Stream;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Meetings;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using TwitchLib.Client.Models;
using BobReactRemaster.Data.Models.GiveAways;

namespace BobReactRemaster.Data.Models.User
{
    public class Member
    {
        public List<StreamSubscription> StreamSubscriptions { get; private set; }

        [Key]
        public string UserName { get; private set; }
        public string? DiscordUserName { get; set; }
        public string? DiscordDiscriminator { get; set; }
        public string? PasswordHash { get; private set; }
        public string UserRole { get; set; }

        public List<MeetingTemplate_Member> RegisteredToMeetingTemplates { get; set; }

        public List<MeetingParticipation> MeetingSubscriptions { get; set; }

        //GiveAway Admin Relation
        public List<GiveAways.GiveAway_Member> GiveAways { get; set; } = new List<GiveAway_Member>();

        public List<Gift> GivenGifts { get; set; } = new List<Gift>();

        public List<Gift> WonGifts { get; set; } = new List<Gift>();
        public int TestWonGiftsCount { get; set; } = 0;
        public int GetWonGiftsCount()
        {
            if(TestWonGiftsCount > 0)
            {
                return TestWonGiftsCount;
            }
            return WonGifts.Count;
        }
        private Member()
        {

        }

        public bool canBeFoundOnDiscord()
        {
            return !DiscordDiscriminator.IsNullOrEmpty() && !DiscordUserName.IsNullOrEmpty();
        }
        public Member(string username,string password, UserRole userRole)
        {
            UserName = username;
            PasswordHash = encryptPassword(password);
            UserRole = userRole.ToString();
            StreamSubscriptions = new List<StreamSubscription>();
            RegisteredToMeetingTemplates = new List<MeetingTemplate_Member>();
            MeetingSubscriptions = new List<MeetingParticipation>();
        }

        public Member(string username,string discriminator)
        {
            UserName = username;
            UserRole = User.UserRole.User.ToString();
            DiscordDiscriminator = discriminator;
            DiscordUserName = username;
            StreamSubscriptions = new List<StreamSubscription>();
            RegisteredToMeetingTemplates = new List<MeetingTemplate_Member>();
            MeetingSubscriptions = new List<MeetingParticipation>();
        }

        public string ResetPassword()
        {
            string password = GeneratePassword();
            PasswordHash = encryptPassword(password);
            return password;
        }

        public string GeneratePassword()
        {
            byte[] salt = new byte[128 / 8];
            byte[] pwd = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
                rng.GetBytes(pwd);
            }
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: pwd.ToString(),
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

            return hashed;
        }
        public void SetPassword(string password)
        {
            PasswordHash = encryptPassword(password);
        }

        private string encryptPassword(string password)
        {
           return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool checkPassword(string passwordtocheck)
        {
            return BCrypt.Net.BCrypt.Verify(passwordtocheck, PasswordHash);
        }

        public bool HasSubscriptions()
        {
            return SubscriptionCount() > 0;
        }
        public bool HasSubscription(LiveStream stream)
        {
            if(stream == null)
            {
                throw new ArgumentNullException("stream must not be null");
            }
            return StreamSubscriptions.Where(x => x.LiveStream == stream).Count() > 0;
        }



        public void AddStreamSubscription(LiveStream stream,bool subscribe = true)
        {
            StreamSubscription newSub = new StreamSubscription(stream,this,subscribe);
            StreamSubscriptions.Add(newSub);
        }

        public int SubscriptionCount()
        {
            return StreamSubscriptions.Count();
        }

        public bool? IsSubscribed(LiveStream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream must not be null");
            }
            return StreamSubscriptions.Where(x => x.LiveStream == stream && x.isSubscribed).Count() == 1;
        }

        public void UnSubscribeStream(LiveStream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream must not be null");
            }
            if (StreamSubscriptions.Where(x => x.LiveStream == stream).FirstOrDefault() == null)
            {
                throw new NullReferenceException("Streamsubscription does not exist for this Stream and User");
            }
            StreamSubscriptions.Where(x => x.LiveStream == stream).FirstOrDefault().UnSubscribe();

        }
        public void SubscribeStream(LiveStream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream must not be null");
            }
            if(StreamSubscriptions.Where(x => x.LiveStream == stream).FirstOrDefault() == null)
            {
                throw new NullReferenceException("Streamsubscription does not exist for this Stream and User");
            }
            StreamSubscriptions.Where(x => x.LiveStream == stream).FirstOrDefault().Subscribe();

        }
    }
}
