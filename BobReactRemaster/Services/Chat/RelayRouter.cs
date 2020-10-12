using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BobReactRemaster.Services.Chat
{
    public class RelayRouter
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public RelayRouter(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public void RelayMessage(RelayMessage MessageObject)
        {
            Console.WriteLine("Here Router");
        }

    }

    public class RelayMessage
    {
        public string channel { get; set; }
        public string server { get; set; }
        public string message { get; set; }

        public SourceType SourceType { get; set; }
    }
    public enum SourceType
    {
        Discord,
        Twitch
    }
}
