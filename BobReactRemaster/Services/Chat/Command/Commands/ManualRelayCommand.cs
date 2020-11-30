using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.Services.Chat.Command.Messages;
using BobReactRemaster.Services.Chat.Commands.Base;
using BobReactRemaster.Services.Chat.Commands.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BobReactRemaster.Services.Chat.Command.Commands
{
    public class ManualRelayCommand: ICommand
    {
        private string Trigger;
        private string Response;
        private readonly IMessageBus Bus;
        private readonly LiveStream _livestream;

        public ManualRelayCommand(string trigger, string response,IMessageBus bus,LiveStream livestream)
        {
            Trigger = trigger;
            Response = response;
            Bus = bus;
            _livestream = livestream;
        }
        public bool IsTriggerable(CommandMessage msg)
        {
            return msg.Message.StartsWith(Trigger);
        }

        public void TriggerCommand(CommandMessage msg)
        {
            Bus.Publish(_livestream.getRelayMessageData(Response));
        }
    }
}
