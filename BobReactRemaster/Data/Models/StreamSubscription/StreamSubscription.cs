﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.User;

namespace BobReactRemaster.Data.Models.Stream
{
    public class StreamSubscription
    {
        [Key] public int Id { get; set; }
        public LiveStream LiveStream { get; private set; }
        public bool isSubscribed { get; private set; }

        public Member Member { get; set; }

        private StreamSubscription()
        {
        }

        public StreamSubscription(LiveStream Stream, Member Member)
        {
            LiveStream = Stream;
            this.Member = Member;
            isSubscribed = true;
        }

        public void Subscribe()
        {
            isSubscribed = true;
        }

        public void UnSubscribe()
        {
            isSubscribed = false;
        }
    }
}