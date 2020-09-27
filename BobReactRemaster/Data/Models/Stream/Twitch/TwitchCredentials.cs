using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Data.Models.Stream.Twitch
{
    public class TwitchCredentials
    {
        [Key]
        public int id { get; set; }
        public string ClientID { get; set; }
        public string Token { get; set; }
        public string Code { get; set; }
        public string Secret { get; set; }
        public string RefreshToken { get; set; }

        //May only be Changed in Setup/Admin
        public bool isTwitchCheckerClient { get; set; }

        public void startReauth()
        {
            //Send Request to Twitch that Reauthes these Credentials
            throw new NotImplementedException();
        }

    }
}
