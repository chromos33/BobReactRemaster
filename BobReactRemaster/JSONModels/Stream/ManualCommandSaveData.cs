using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.JSONModels.Stream
{
    public class ManualCommandSaveData
    {
        public int CommandID { get; set; }
        public string Name { get; set; }
        public string Response { get; set; }
        public string Trigger { get; set; }

    }
}
