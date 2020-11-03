using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Extensions;

namespace BobReactRemaster.Services.Chat.Twitch
{
    public class MessageQueue
    {
        private List<string> Messages { get; set; }
        public bool isModerator { get; private set; }
        private DateTime LastMessageSent { get; set; }
        private TimeSpan MessageRate { get; set; }

        public MessageQueue( bool isModerator, TimeSpan messageRate)
        {
            Messages = new List<string>();
            this.isModerator = isModerator;
            MessageRate = messageRate;
        }

        public void EnableModeratorMode()
        {
            isModerator = true;
        }

        public string NextQueuedMessage()
        {
            if (!Messages.IsNullOrEmpty())
            {
                if (NextMessageDue())
                {
                    string tmp = Messages.First();
                    Messages.RemoveAt(0);
                    LastMessageSent = DateTime.Now;
                    return tmp;
                }
            }

            return null;
        }

        private bool NextMessageDue()
        {
            if (isModerator)
            {
                return true;
            }
            if (DateTime.Now.Subtract(LastMessageSent) > MessageRate)
            {
                return true;
            }

            return false;
        }

        public void AddMessage(string Message)
        {
            Messages.Add(Message);
        }
    }
}
