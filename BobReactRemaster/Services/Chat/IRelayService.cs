using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Services.Chat.GeneralClasses;

namespace BobReactRemaster.Services.Chat
{
    public interface IRelayService
    {
        public void RelayMessage(RelayMessage MessageObject);
    }
}
