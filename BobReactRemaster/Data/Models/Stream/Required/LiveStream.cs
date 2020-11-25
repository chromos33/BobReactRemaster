using BobReactRemaster.Data.Models.Discord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Commands;
using BobReactRemaster.EventBus.BaseClasses;

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

        public bool RelayEnabled { get; set; }

        public string StreamName { get; set; }
        
        public bool VariableRelayChannel { get; set; }

        public List<IntervalCommand> RelayIntervalCommands { get; set; }
        public List<ManualCommand> RelayManualCommands { get; set; }

        public abstract void SetURL(string URL);
        public abstract void StartStream();
        public abstract void SetStreamStarted(DateTime date);
        public abstract void StopStream();

        public abstract BaseMessageData getRelayMessageData(string message);

        public string GetUptimeMessage(DateTime StartTime)
        {
            TimeSpan duration = DateTime.Now.Subtract(StartTime);
            string DurationText = "";
            if (duration.Days > 0)
            {
                DurationText += $"{duration.Days} ";
                if (duration.Days == 1)
                {
                    DurationText += "Tag";
                }
                else
                {
                    DurationText += "Tagen";
                }
            }

            if (duration.Hours > 0)
            {
                DurationText += $"{duration.Hours} Stunden";
            }
            if (duration.Minutes > 0)
            {
                DurationText += $"{duration.Minutes} Minuten";
            }
            return $"Stream läuft seit {DurationText}";
        }
        

        //add Value Getter for default data for Streams
        public string GetSubscriptionCreatedMessage()
        {
            return $"Der Stream {StreamName} wurde hinzugefügt. Du kannst ihn im Interface abonnieren";
        }
    }
}