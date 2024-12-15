using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Auth;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.Helper;
using BobReactRemaster.JSONModels.Setup;
using BobReactRemaster.Services.Chat;
using BobReactRemaster.Services.Chat.Discord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace BobReactRemaster.Controllers
{
    [ApiController]
    [Route("Setup")]
    public class SetupController : Controller
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ApplicationDbContext _context;
        public SetupController(ApplicationDbContext context, IServiceScopeFactory scopeFactory)
        {
            _context = context;
            _scopeFactory = scopeFactory;
        }
        [HttpGet]
        [Route("GetDiscordTokenData")]
        [Authorize(Policy = Policies.Admin)]
        public IActionResult GetDiscordTokenData()
        {
            var Credentials = _context.DiscordCredentials.FirstOrDefault();
            if (Credentials != null)
            {
                return Ok(new { ClientID= Credentials.ClientID, Token= Credentials.Token });
            }
            return NotFound();
        }
        [HttpPost]
        [Route("SetDiscordTokenData")]
        [Authorize(Policy = Policies.Admin)]
        public IActionResult SetDiscordTokenData([FromBody] DiscordTokenData Data)
        {

            var Credentials = _context.DiscordCredentials.FirstOrDefault();
            if (Credentials == null)
            {
                Credentials = new DiscordCredentials();
                _context.DiscordCredentials.Add(Credentials);
            }
            Credentials.setFromDiscordTokenData(Data);
            _context.SaveChanges();
            return Ok();
        }
        [HttpPost]
        [Route("ImportFile")]
        [Authorize(Policy = Policies.Admin)]
        [Consumes("text/plain")]
        public async Task<IActionResult> ImportFile([FromBody] string data)
        {
            //DiscordChat
            var Data = JsonConvert.DeserializeObject<LegacyImportData>(data);
            //Member
            var scope = _scopeFactory.CreateScope();
            var Services = scope.ServiceProvider.GetServices<IHostedService>();
            DiscordChat? Discord = null;
            foreach(var service in Services)
            {
                if(service.GetType() == typeof(DiscordChat))
                {
                    Discord = (DiscordChat) service;
                }
            }

            if (Data != null)
            {
                foreach (var Member in Data.Members)
                {
                    if (_context.Members.AsEnumerable()
                            .Where(x => x.DiscordUserName?.ToLower() == Member.UserName.ToLower()).Count() > 0)
                    {
                        continue;
                    }

                    if (Discord != null)
                    {
                        var DiscordMember = await Discord.GetMemberByName(Member.UserName);
                        if (DiscordMember != null)
                        {
                            if (DiscordMember.Username.Contains("kyu"))
                            {
                                Console.WriteLine("test");
                            }
                            BobReactRemaster.Data.Models.User.Member tmp =
                                new Data.Models.User.Member(DiscordMember.Username, DiscordMember.Id);
                            tmp.ResetPassword();
                            _context.Members.Add(tmp);
                        }
                        else
                        {
                            Console.WriteLine(Member.UserName);
                        }
                    }
                }

                foreach (var Stream in Data.Streams)
                {
                    if (_context.TwitchStreams.AsEnumerable()
                            .Where(x => x.StreamName.ToLower() == Stream.StreamName.ToLower()).Count() > 0)
                    {
                        continue;
                    }

                    TwitchStream tmp = new TwitchStream(Stream.StreamName);
                    _context.TwitchStreams.Add(tmp);
                }
                _context.SaveChanges();

                foreach (var Quote in Data.Quotes)
                {
                    if (_context.Quotes.AsEnumerable().Count(x => x.Text.Equals(Quote.Text, StringComparison.CurrentCultureIgnoreCase)) > 0)
                    {
                        continue;
                    }

                    var stream = _context.TwitchStreams.AsQueryable()
                        .FirstOrDefault(x => x.StreamName.Equals(Quote.Streamer, StringComparison.CurrentCultureIgnoreCase));
                    if (stream != null)
                    {
                        Data.Models.Stream.Quote tmp = new Data.Models.Stream.Quote();
                        tmp.Text = Quote.Text;
                        tmp.Created = Quote.Created;
                        tmp.stream = stream;

                        _context.Quotes.Add(tmp);
                    }
                }

                foreach (var Subscription in Data.Subscriptions)
                {
                    var stream = _context.TwitchStreams.AsQueryable()
                        .FirstOrDefault(x => x.StreamName.ToLower() == Subscription.StreamName.ToLower());
                    var submember = _context.Members.AsQueryable().FirstOrDefault(x =>
                        x.DiscordUserName.ToLower() == Subscription.UserName.ToLower());
                    if (stream != null && submember != null)
                    {
                        if (_context.StreamSubscriptions.AsEnumerable()
                                .Where(x => x.LiveStream == stream && x.Member == submember).Count() > 0)
                        {
                            continue;
                        }

                        var tmp = new StreamSubscription(stream, submember, Subscription.isSubscribed);
                        _context.StreamSubscriptions.Add(tmp);
                    }
                }

                _context.SaveChanges();
            }

            
            return Ok();
        }
    }
}
