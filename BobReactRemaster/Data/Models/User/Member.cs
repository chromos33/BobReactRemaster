using BobReactRemaster.Data.Models.Stream;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Data.Models.User
{
    public class Member
    {
        public List<StreamSubscription> StreamSubscriptions { get; private set; }

        [Key]
        public string UserName { get; private set; }
        public string PasswordHash { get; private set; }
        public string UserRole { get; set; }

        private Member()
        {

        }
        public Member(string username,string password, UserRole userRole)
        {
            UserName = username;
            PasswordHash = encryptPassword(password);
            UserRole = userRole.ToString();
            StreamSubscriptions = new List<StreamSubscription>();
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
        public bool HasSubscription(iLiveStream stream)
        {
            if(stream == null)
            {
                throw new ArgumentNullException("stream must not be null");
            }
            return StreamSubscriptions.Where(x => x.LiveStream == stream).Count() > 0;
        }



        public void AddStreamSubscription(iLiveStream stream)
        {
            StreamSubscription newSub = new StreamSubscription(stream,this);
            StreamSubscriptions.Add(newSub);
        }

        public int SubscriptionCount()
        {
            return StreamSubscriptions.Count();
        }

        public bool? IsSubscribed(iLiveStream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream must not be null");
            }
            return StreamSubscriptions.Where(x => x.LiveStream == stream && x.isSubscribed).Count() == 1;
        }

        public void UnSubscribeStream(iLiveStream stream)
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
        public void SubscribeStream(iLiveStream stream)
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
