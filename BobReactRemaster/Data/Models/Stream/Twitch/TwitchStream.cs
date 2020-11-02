using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.Data.Models.Stream.Required;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Stream.Twitch;
using BobReactRemaster.JSONModels.Twitch;

namespace BobReactRemaster.Data.Models.Stream
{
    public class TwitchStream : iLiveStream, IRelayableStream
    {
        public int Id { get; set; }

        public List<StreamSubscription> Subscriptions { get; private set; }

        public string URL { get; private set; }

        public DateTime Started { get; private set; }

        public DateTime Stopped { get; private set; }

        public StreamState State { get; private set; }

        public TextChannel RelayChannel { get; private set; }

        public string StreamName { get; set; }
        public string StreamID { get; set; }
        public int? APICredentialId { get; set; }
        public TwitchCredential APICredential { get; private set; }

        public TwitchStream(string StreamName)
        {
            this.StreamName = StreamName;
            Subscriptions = new List<StreamSubscription>();
            State = StreamState.Stopped;
        }

        public void SetTwitchCredential(TwitchCredential cred)
        {
            if (cred.isMainAccount)
            {
                throw new InvalidDataException("Credential cannot be the MainAccount use TwitchCredential.StreamClone()");
            }

            APICredential = cred;

        }

        public TwitchStreamListData GetStreamListData()
        {
            return new TwitchStreamListData(){ID = Id,Name = StreamName,StreamState = State};
        }

        public List<StreamSubscription> GetStreamSubscriptions()
        {
            return Subscriptions;
        }

        public void SetURL(string URL)
        {
            this.URL = URL;
        }

        public void StartStream()
        {
            State = StreamState.Running;
            SetStreamStarted(DateTime.Now);
        }

        public void SetStreamStarted(DateTime date)
        {
            Started = date;
        }

        public void StopStream()
        {
            Stopped = DateTime.Now;
            State = StreamState.Stopped;
        }

        public void SetRelayChannel(TextChannel channel)
        {
            RelayChannel = channel;
        }

        List<StreamSubscription> iLiveStream.GetStreamSubscriptions()
        {
            throw new NotImplementedException();
        }

        void iLiveStream.SetURL(string URL)
        {
            throw new NotImplementedException();
        }

        void iLiveStream.StartStream()
        {
            throw new NotImplementedException();
        }

        void iLiveStream.SetStreamStarted(DateTime date)
        {
            throw new NotImplementedException();
        }

        void iLiveStream.StopStream()
        {
            throw new NotImplementedException();
        }
    }
}