using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Stream;

namespace BobReactRemaster.Services.Stream
{
    interface IStreamChecker
    {
        public Task CheckStreams();
    }
}
