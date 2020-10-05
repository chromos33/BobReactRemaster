using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.JSONModels.Setup
{
    public class DiscordTokenData
    {
        [Required] public string ClientID { get; set; }  
        [Required] public string Token { get; set; }  
    }
}
