using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Services.Chat.Commands.Base;

namespace BobReactRemaster.Services.Chat.Commands.Interfaces
{
    public interface ICommand
    {
        bool IsTriggerable(CommandMessage msg);

        public void TriggerCommand(CommandMessage msg);
    }


}
