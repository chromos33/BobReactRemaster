using BobReactRemaster.Data.Models.Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Data.Models.Stream.Required
{
    public interface IRelayableStream
    {
        public void SetRelayChannel(TextChannel channel);
    }
}