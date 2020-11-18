using System.ComponentModel.DataAnnotations;

namespace BobReactRemaster.Controllers
{
    public class TwitchOauthStoreData
    {
        [Required] public string ClientID { get; set; }
        [Required] public string Secret { get; set; }
        [Required] public string ChatUserName { get; set; }

        public string Scopes { get; set; }
    }
}