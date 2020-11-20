using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.Services.Chat.Command.Messages;
using BobReactRemaster.Services.Chat.Commands.Base;
using BobReactRemaster.Services.Chat.Commands.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BobReactRemaster.Services.Chat.Command.Commands
{
    public class ManualCommand: ICommand
    {
        private string Trigger;
        private string Response;
        private readonly IMessageBus Bus;

        public ManualCommand(string trigger, string response,IMessageBus bus)
        {
            Trigger = trigger;
            Response = response;
            Bus = bus;
        }
        public bool IsTriggerable(CommandMessage msg)
        {
            return msg.Message.StartsWith(Trigger);
        }

        public void TriggerCommand()
        {
            throw new NotImplementedException();
        }
    }
}
