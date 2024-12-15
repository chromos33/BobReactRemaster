using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BobReactRemaster.Auth;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.Data.Models.Stream.Twitch;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.EventBus.MessageDataTypes;
using BobReactRemaster.JSONModels.Twitch;
using BobReactRemaster.Services.Chat.Twitch;
using BobReactRemaster.Services.Scheduler;
using BobReactRemaster.Services.Scheduler.Tasks;
using BobReactRemaster.SettingsOptions;
using Discord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BobReactRemaster.Controllers
{
    [ApiController]
    [Route("Twitch")]
    public class TwitchAuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceScopeFactory _scopeFactory;
        public TwitchAuthController(ApplicationDbContext context, IServiceScopeFactory scopeFactory)
        {
            _context = context;
            _scopeFactory = scopeFactory;
        }

        [HttpGet]
        [Route("GetTwitchMainTokenData")]
        [Authorize(Policy = Policies.Admin)]
        public IActionResult GetTwitchMainTokenData()
        {
            var Credential = _context.TwitchCredentials.FirstOrDefault(x => x.isMainAccount);
            if (Credential != null)
            {
                return Ok(new { ClientID = Credential.ClientID, Secret = Credential.Secret, ChatUserName= Credential.ChatUserName });
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
            var scope = _scopeFactory.CreateScope();
            var Option = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<WebServerSettingsOptions>>();
            string WebserverAddress = Option.Value.Address;
            var guid = Guid.NewGuid().ToString();
            string link = Credential.getTwitchAuthLink(Data,WebserverAddress,guid);
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
                    var scope = _scopeFactory.CreateScope();
                    var Option = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<WebServerSettingsOptions>>();
                    string WebserverAddress = Option.Value.Address;
                    var guid = Guid.NewGuid().ToString();
                    string link = Stream.APICredential.getTwitchAuthLink(Data, WebserverAddress,guid);
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
                var internalScope = _scopeFactory.CreateScope();
                var Option = internalScope.ServiceProvider.GetRequiredService<IOptionsSnapshot<WebServerSettingsOptions>>();
                string WebserverAddress = Option.Value.Address;
                string returnurl = Credential.getTwitchReturnURL(WebserverAddress);
                string url = $"https://id.twitch.tv/oauth2/token?client_id={Credential.ClientID}&client_secret={Credential.Secret}&code={code}&grant_type=authorization_code&redirect_uri={returnurl}";
                var response = await client.PostAsync(url, new StringContent("", System.Text.Encoding.UTF8, "text/plain"));
                var responsestring = await response.Content.ReadAsStringAsync();
                TwitchAuthToken? authtoken = JsonConvert.DeserializeObject<TwitchAuthToken>(responsestring);
                if (authtoken != null)
                {
                    Credential.Token = authtoken.access_token;
                    Credential.ExpireDate = DateTime.Now.AddSeconds(authtoken.expires_in);
                    Credential.RefreshToken = authtoken.refresh_token;
                }

                await _context.SaveChangesAsync();
                //Add Task to Scheduler for Refresh;
                var schedulerservice = internalScope.ServiceProvider.GetServices<IHostedService>()
                    .FirstOrDefault(x => x.GetType() == typeof(SchedulerService));
                if (schedulerservice != null)
                {
                    try
                    {
                        var scheduler = (SchedulerService)schedulerservice;
                        scheduler.AddTask(new TwitchOAuthRefreshTask(Credential.ExpireDate,Credential.id));
                    }
                    catch (InvalidCastException)
                    {
                    }
                }
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
