using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BobReactRemaster.Auth;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.Data.Models.Stream.Twitch;
using BobReactRemaster.JSONModels.Twitch;
using Discord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BobReactRemaster.Controllers
{
    [ApiController]
    [Route("Twitch")]
    public class TwitchAuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public TwitchAuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetTwitchMainTokenData")]
        [Authorize(Policy = Policies.Admin)]
        public IActionResult GetTwitchMainTokenData()
        {
            var Credential = _context.TwitchCredentials.FirstOrDefault(x => x.isMainAccount);
            if (Credential != null)
            {
                return Ok(new { ClientID = Credential.ClientID, Secret = Credential.Secret });
            }
            return NotFound();
        }


        [HttpPost]
        [Route("TwitchOAuthStartAdmin")]
        [Authorize(Policy = Policies.Admin)]
        public IActionResult TwitchOAuthStartAdmin([FromBody] TwitchOauthStoreData Data)
        {

            var Credential = _context.TwitchCredentials.FirstOrDefault(x => x.isMainAccount);
            if (Credential == null)
            {
                Credential = new TwitchCredential();
                Credential.isMainAccount = true;
                _context.TwitchCredentials.Add(Credential);
            }
            Credential.setFromTwitchOauthStoreData(Data);
            string WebserverAddress = _configuration.GetValue<string>("WebServerWebAddress");
            string link = Credential.getTwitchAuthLink(Data,WebserverAddress);
            _context.SaveChanges();

            return Ok(new {Link= link});
        }
        [HttpPost]
        [Route("TwitchOAuthStartUser")]
        [Authorize(Policy = Policies.User)]
        public IActionResult TwitchOAuthStartUser([FromBody] TwitchStreamOauthData Data)
        {
            var Stream = _context.TwitchStreams.FirstOrDefault(x => String.Equals(x.StreamName, Data.StreamName, StringComparison.CurrentCultureIgnoreCase));

            if (Stream != null)
            {
                if (Stream.APICredential == null)
                {
                    var MainCredential = _context.TwitchCredentials.FirstOrDefault(x => x.isMainAccount);
                    if (MainCredential != null)
                    {
                        Stream.SetTwitchCredential(MainCredential.StreamClone());
                        _context.SaveChanges();
                    }
                }
                if (Stream.APICredential != null)
                {
                    string WebserverAddress = _configuration.GetValue<string>("WebServerWebAddress");
                    string link = Stream.APICredential.getTwitchAuthLink(Data, WebserverAddress);
                    _context.SaveChanges();
                    return Ok(new {Link = link});
                }
            }
            return NotFound();
        }

        [HttpGet]
        [Route("TwitchOAuthReturn")]
        [AllowAnonymous]
        public async Task<IActionResult> TwitchOAuthReturn(string code, string scope, string state)
        {
            var Credential = _context.TwitchCredentials.FirstOrDefault(x => x.validationKey == state);

            if (Credential != null && code != null)
            {
                var client = new HttpClient();
                string WebserverAddress = _configuration.GetValue<string>("WebServerWebAddress");
                string returnurl = Credential.getTwitchReturnURL(WebserverAddress);
                string url = $"https://id.twitch.tv/oauth2/token?client_id={Credential.ClientID}&client_secret={Credential.Secret}&code={code}&grant_type=authorization_code&redirect_uri={returnurl}";
                var response = await client.PostAsync(url, new StringContent("", System.Text.Encoding.UTF8, "text/plain"));
                var responsestring = await response.Content.ReadAsStringAsync();
                TwitchAuthToken authtoken = JsonConvert.DeserializeObject<TwitchAuthToken>(responsestring);
                Credential.Token = authtoken.access_token;
                Credential.ExpireDate = DateTime.Now.AddSeconds(authtoken.expires_in);
                Credential.RefreshToken = authtoken.refresh_token;
                await _context.SaveChangesAsync();

                if (Credential.isMainAccount)
                {
                    return Redirect("/SetupView");
                }
                //replace Redirect with Link to Stream overview
                return Redirect("/");
            }
            //replace Redirect with Link to Stream overview
            return Redirect("/");

        }

    }
}
