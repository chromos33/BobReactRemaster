using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.JSONModels.Stream;

namespace BobReactRemaster.Data.Models.Commands
{
    public class ManualCommand : ChatCommand
    {
        public string Trigger { get; set; }

        private ManualCommand()
        {

        }

        public ManualCommand(ManualCommandSaveData data)
        {
            LiveStreamId = data.StreamID;
            UpdateData(data);
        }
        public void UpdateData(ManualCommandSaveData data)
        {
            Trigger = data.Trigger;
            Name = data.Name;
            Response = data.Response;
            //Dunno might actually remove this
            Active = true;
        }
    }
}
