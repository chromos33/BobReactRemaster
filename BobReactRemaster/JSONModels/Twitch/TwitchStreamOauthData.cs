using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Controllers;

namespace BobReactRemaster.JSONModels.Twitch
{
    public class TwitchStreamOauthData: TwitchOauthStoreData
    {
        [Required] public string StreamName { get; set; }
    }
}
