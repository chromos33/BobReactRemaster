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
    public class TwitchStream : LiveStream, IRelayableStream
    {
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

        public override void SetURL(string url)
        {
            URL = url;
        }

        public override void StartStream()
        {
            State = StreamState.Running;
            SetStreamStarted(DateTime.Now);
        }

        public override void SetStreamStarted(DateTime date)
        {
            Started = date;
        }

        public override void StopStream()
        {
            Stopped = DateTime.Now;
            State = StreamState.Stopped;
        }

        public void SetRelayChannel(TextChannel channel)
        {
            RelayChannel = channel;
        }
    }
}