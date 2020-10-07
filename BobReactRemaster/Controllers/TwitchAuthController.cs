using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Auth;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.Data.Models.Stream.Twitch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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

        [HttpGet]
        [Route("TwitchOAuthReturn")]
        [AllowAnonymous]
        public async Task<IActionResult> TwitchOAuthReturn(string code, string scope, string state)
        {
            //TODO get TwitchCredentials where validationKey is equal to state and store continue if true
            /* old code
            var savedtoken = _context.SecurityTokens.Where(st => st.service == TokenType.Twitch).FirstOrDefault();
            savedtoken.code = code;
            var client = new HttpClient();
            string baseUrl = _configuration.GetValue<string>("WebServerWebAddress");
            string url = $"https://id.twitch.tv/oauth2/token?client_id={savedtoken.ClientID}&client_secret={savedtoken.secret}&code={code}&grant_type=authorization_code&redirect_uri={baseUrl}/Admin/TwitchReturnUrlAction";
            var response = await client.PostAsync(url, new StringContent("", System.Text.Encoding.UTF8, "text/plain"));
            var responsestring = await response.Content.ReadAsStringAsync();
            JSONObjects.TwitchAuthToken authtoken = JsonConvert.DeserializeObject<JSONObjects.TwitchAuthToken>(responsestring);
            savedtoken.token = authtoken.access_token;
            savedtoken.RefreshToken = authtoken.refresh_token;
            await _context.SaveChangesAsync();

            */
            return Ok();
        }

    }
}
