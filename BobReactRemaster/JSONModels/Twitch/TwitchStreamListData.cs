using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Stream;

namespace BobReactRemaster.JSONModels.Twitch
{
    public class TwitchStreamListData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public StreamState StreamState { get; set; }
    }
}
