using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Controllers;

namespace BobReactRemaster.Data.Models.Commands
{
    public class IntervalCommand : ChatCommand
    {
        public int AutoInverval { get; set; }
        public DateTime? LastExecution { get; set; }

        public void InitData(IntervalCommandSaveData data)
        {
            LiveStreamId = data.StreamID;
            UpdateData(data);
        }

        public void UpdateData(IntervalCommandSaveData data)
        {
            AutoInverval = data.Interval;
            Name = data.Name;
            Response = data.Response;
            //Dunno might actually remove this
            Active = true;
        }
    }
}
