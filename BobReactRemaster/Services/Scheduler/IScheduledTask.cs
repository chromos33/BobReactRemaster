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
        public void setScopeFactory(IServiceScopeFactory factory);
        public bool isThisTask(int ID);
        public bool isThisTask(IScheduledTask Task);
        public int? GetID();
        public bool InitializeID(int ID);
    }
}
