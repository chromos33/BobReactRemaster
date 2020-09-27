using BobReactRemaster.Data.Models.Stream;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Data.Models.User
{
    public class Member : IdentityUser
    {
        public List<StreamSubscription> StreamSubscriptions { get; private set; }
        private Member ()
        {

        }
        public Member(string UserName)
        {
            this.UserName = UserName;
            StreamSubscriptions = new List<StreamSubscription>();
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
