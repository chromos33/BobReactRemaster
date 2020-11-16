using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Services.Chat.Commands.Base;
using BobReactRemaster.Services.Chat.Commands.Interfaces;

namespace BobReactRemaster.Services.Chat.Commands
{
    //Punny
    public class CommandCenter
    {
        private List<ICommand> Commands;

        public void HandleCommandMessage(CommandMessage msg)
        {

        }
    }
}
