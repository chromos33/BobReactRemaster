using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.User;
using BobReactRemaster.EventBus;
using BobReactRemaster.JSONModels;
using BobReactRemaster.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BobReactRemaster.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<Member> _userManager;
        private readonly SignInManager<Member> _signInManager;
        private readonly IMessageBus _eventBus;
        private readonly ApplicationDbContext _dbcontext;
        public LoginController(UserManager<Member> userManager,
            SignInManager<Member> signInManager,
            IMessageBus eventBus,
            ApplicationDbContext dbcontext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _eventBus = eventBus;
            _dbcontext = dbcontext;
        }
        public async Task<string> Login()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                string json = await reader.ReadToEndAsync();
                ReactLoginData data = JsonConvert.DeserializeObject<ReactLoginData>(json);
                if (data.user != null && data.pass != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(data.user, data.pass, true, false);
                    return JsonConvert.SerializeObject(new ReactResponse() { Response = "true" });
                }
            }
            return JsonConvert.SerializeObject(new ReactResponse() { Response = "false" });
        }
    }
}
