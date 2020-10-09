namespace BobReactRemaster.JSONModels.Twitch
{
    public class TwitchAuthToken
    {
        public string access_token { get; set; }
        public string id_token { get; set; }
        public string refresh_token { get; set; }
        public string[] scope { get; set; }

        public int expires_in { get; set; }
    }
}
