using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.EventBus.BaseClasses;
using Microsoft.Extensions.DependencyInjection;

namespace BobReactRemaster.EventBus.MessageDataTypes
{
    public class TwitchOAuthedMessageData: BaseMessageData
    {
        public DateTime ExpireTime { get; set; }
        public int ID { get; set; }
        public IServiceScopeFactory ServiceScopeFactory { get; set; }

    }
}
