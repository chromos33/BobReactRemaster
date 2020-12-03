using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using BobReactRemaster.JSONModels.Twitch;
using Newtonsoft.Json;

namespace BobReactRemaster.APIs
{
    public class TwitchCustomAPI
    {
        public static async Task<string> GetTwitchGameIDFromName(string Name,HttpClient Client)
        {
            string uri = $"https://api.twitch.tv/helix/games?Name={HttpUtility.UrlEncode(Name)}";
            var response = await Client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var resultdata = JsonConvert.DeserializeObject<TwitchGameResponse>(result);
                if (resultdata.data.Any())
                {
                    return resultdata.data.First().id;
                }
            }
            return null;
        }

        public static async Task<bool> TryToSetTwitchGame(string StreamID,string GameID, HttpClient Client)
        {
            string uri = $"https://api.twitch.tv/helix/channels?broadcaster_id={StreamID}";
            var data = JsonConvert.SerializeObject(new { game_id = GameID });
            var Response = await Client.PatchAsync(uri, new StringContent(data, System.Text.Encoding.UTF8, "application/json"));
            return Response.IsSuccessStatusCode;
        }
        public static async Task<bool> TryToSetTwitchTitle(string StreamID, string Title, HttpClient Client)
        {
            string uri = $"https://api.twitch.tv/helix/channels?broadcaster_id={StreamID}";
            var data = JsonConvert.SerializeObject(new {title = Title});
            var Response = await Client.PatchAsync(uri, new StringContent(data, System.Text.Encoding.UTF8, "application/json"));
            return Response.IsSuccessStatusCode;
        }


    }
}
