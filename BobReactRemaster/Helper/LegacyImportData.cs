using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Helper
{
    public class LegacyImportData
    {
        public List<Member> Members { get; set; }
        public List<Stream> Streams { get; set; }
        public List<Subscription> Subscriptions { get; set; }
        public List<Quote> Quotes { get; set; }
    }
    public class Member
    {
        public string UserName { get; set; }
    }
    public class Stream
    {
        public string StreamName { get; set; }
    }
    public class Subscription
    {
        public bool isSubscribed { get; set; }
        public string UserName { get; set; }
        public string StreamName { get; set; }
    }
    public class Quote
    {
        public string Streamer { get; set; }
        public DateTime Created { get; set; }
        public string Text { get; set; }
    }
}
