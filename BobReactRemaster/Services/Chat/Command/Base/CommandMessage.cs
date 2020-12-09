using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.EventBus.BaseClasses;

namespace BobReactRemaster.Services.Chat.Commands.Base
{
    public abstract class CommandMessage : BaseMessageData
    {
        public string Message { get; protected set; }
        public bool IsElevated { get; protected set; }
    }
}
