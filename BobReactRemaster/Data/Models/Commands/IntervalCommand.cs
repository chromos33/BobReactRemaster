using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Data.Models.Commands
{
    public class IntervalCommand : ChatCommand
    {
        public int AutoInverval { get; set; }
        public DateTime? LastExecution { get; set; }
    }
}
