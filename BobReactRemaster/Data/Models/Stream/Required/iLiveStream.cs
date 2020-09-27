using BobReactRemaster.Data.Models.Discord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Data.Models.Stream
{
    public interface iLiveStream
    {
        public List<StreamSubscription> GetStreamSubscriptions();
        [Key]
        public int Id { get; set; }
        public List<StreamSubscription> Subscriptions { get; }
        public string URL { get; }
        public DateTime Started { get; }
        public DateTime Stopped { get; }
        public StreamState State { get; }
        public TextChannel RelayChannel { get;}
        public string StreamName { get; }

        public void SetURL(string URL);
        public void StartStream();
        public void SetStreamStarted(DateTime date);
        public void StopStream();

        //add Value Getter for default data for Streams
    }
}
