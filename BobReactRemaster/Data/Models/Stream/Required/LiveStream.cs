using BobReactRemaster.Data.Models.Discord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Data.Models.Stream
{
    public abstract class LiveStream
    {
        [Key] public int Id { get; set; }
        public List<StreamSubscription> Subscriptions { get; protected set; }
        public string URL { get; protected set; }
        public DateTime Started { get; protected set; }
        public DateTime Stopped { get; protected set; }
        public StreamState State { get; protected set; }
        public TextChannel RelayChannel { get; protected set; }
        public string StreamName { get; set; }

        public abstract void SetURL(string URL);
        public abstract void StartStream();
        public abstract void SetStreamStarted(DateTime date);
        public abstract void StopStream();

        //add Value Getter for default data for Streams
    }
}