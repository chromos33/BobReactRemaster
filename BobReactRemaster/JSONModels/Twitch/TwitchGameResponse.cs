using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.JSONModels.Twitch
{
    public class TwitchGameResponse
    {
        public List<TwitchGameResponseData> data { get; set; }
    }

    public class TwitchGameResponseData
    {
        public string id { get; set; }
        public string box_art_url { get; set; }
        
        public string name { get; set; }

    }
}
