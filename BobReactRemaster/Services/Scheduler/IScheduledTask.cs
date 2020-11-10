using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BobReactRemaster.Services.Scheduler
{
    public interface IScheduledTask
    {
        public bool Executable();
        public void Execute();
        public bool Removeable();
    }
}
