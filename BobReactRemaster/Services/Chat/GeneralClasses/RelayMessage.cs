using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.EventBus.BaseClasses;

namespace BobReactRemaster.Services.Chat.GeneralClasses
{
    public abstract class RelayMessage
    {
        public string Message { get; set; }

        public abstract List<BaseMessageData> GetMessageBusMessages(ApplicationDbContext context);
    }
}
