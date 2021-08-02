using BobReactRemaster.EventBus.BaseClasses;
using System;

namespace BobReactRemaster.Services.Chat.Discord
{
    public class DiscordWhisperData : BaseMessageData
    {
        public string MemberName { get; set; }
        public string Message { get; set; }

        public DiscordWhisperData(string MemberName,string Mesage)
        {
            this.MemberName = MemberName;
            this.Message = Message;
        }
        public void AddToMessage(string add)
        {
            Message += add + Environment.NewLine;
        }
    }
}