using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.Data.Models.Stream.Required;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Commands;
using BobReactRemaster.Data.Models.Stream.Twitch;
using BobReactRemaster.EventBus.BaseClasses;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.EventBus.MessageDataTypes;
using BobReactRemaster.JSONModels.Twitch;
using BobReactRemaster.Services.Chat.Command.Commands.Twitch;
using BobReactRemaster.Services.Chat.Commands.Interfaces;
using BobReactRemaster.Services.Scheduler;
using BobReactRemaster.Services.Scheduler.Tasks;

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
            RelayIntervalCommands = new List<IntervalCommand>();
            RelayManualCommands = new List<ManualCommand>();
            Quotes = new List<Quote>();
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

        public override void SetURL(string url)
        {
            URL = url;
        }

        public override void StartStream()
        {
            State = StreamState.Running;
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

        public override BaseMessageData getRelayMessageData(string message)
        {
            TwitchRelayMessageData TwitchMessageData = new TwitchRelayMessageData();
            TwitchMessageData.Message = message;
            TwitchMessageData.StreamName = StreamName;
            return TwitchMessageData;
        }

        public override IScheduledTask GetUpTimeTask()
        {
            return new StreamUptimeRelayTask(this,Started,UpTimeInterval);
        }

        public override bool HasStaticCommands()
        {
            return APICredential != null;
        }

        public override IEnumerable<ICommand> GetStaticCommands(IMessageBus bus)
        {
            List<ICommand> Commands = new List<ICommand>();

            Commands.Add(new TwitchStreamTitleChangeCommand(bus,this));
            Commands.Add(new TwitchGameChangeCommand(bus,this));
            return Commands;
        }

        public void SetRelayChannel(TextChannel channel)
        {
            RelayChannel = channel;
        }

        public void UnsetRelayChannel()
        {
            RelayChannel = null;
        }
        public string GetStreamStartedMessage(string Title = "")
        {
            string message = $"{StreamName} hat angefangen {Title} zu streamen.";

            if (RelayChannel != null)
            {
                message += $"Relay befindet sich in Channel {RelayChannel.Name}";
            }

            return message;
        }
    }
}