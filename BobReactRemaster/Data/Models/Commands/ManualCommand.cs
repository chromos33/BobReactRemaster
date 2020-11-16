using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Data.Models.Commands
{
    public class ManualCommand : ChatCommand
    {
        public string Trigger { get; set; }
    }
}
