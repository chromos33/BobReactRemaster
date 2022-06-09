using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using BobReactRemaster.Auth;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.User;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.Services.Chat.Discord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BobReactRemaster.Controllers
{
    [ApiController]
    [Route("User")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMessageBus MessageBus;
        private readonly ApplicationDbContext _context;
        public LoginController(IConfiguration config, ApplicationDbContext context, IMessageBus messageBus)
        {
            _config = config;
            _context = context;
            MessageBus = messageBus;
        }
        [HttpPost]
        [Route("PasswordChange")]
        [AllowAnonymous]
        public IActionResult PasswordChange([FromBody] PasswordChangeData changeData)
        {
            var UserName = User.FindFirst("fullName")?.Value;
            if (UserName != null)
            {
                Member user = _context.Members.SingleOrDefault(x => x.UserName == UserName);
                if (user != null && user.checkPassword(changeData.OldPassword) && changeData.NewPasswordsMatch())
                {
                    user.SetPassword(changeData.NewPassword);
                    _context.SaveChanges();
                    return Ok();
                }
            }
            

            return Unauthorized();
        }
        [HttpPost]
        [Route("RequestPassword")]
        [AllowAnonymous]
        public IActionResult RequestPassword([FromBody] PasswordRequestData authData)
        {
            Member user = _context.Members.SingleOrDefault(x => x.UserName == authData.UserName);
            if (user != null)
            {
                string pw = user.ResetPassword();
                _context.SaveChanges();
                var data = new DiscordWhisperData(user.DiscordUserName, pw);
                MessageBus.Publish(data);
                return Ok();
            }

            return Unauthorized();
        }
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] AuthData authData)
        {
            IActionResult response = Unauthorized();

            if (authData != null)
            {
                Member? user = AuthenticateUser(authData);
                if(user != null)
                {
                    string tokenString = GenerateJWTToken(user);
                    response = Ok(new{token = tokenString,userName = user.UserName});
                }
            }

            return response;
        }

        [HttpGet]
        [Route("Setup")]
        [Authorize(Policy = Policies.User)]
        public IActionResult Setup()
        {
            return Ok(new{Response= "This is a response from admin method"});
        }

        private string GenerateJWTToken(Member user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim("fullName", user.UserName),
                new Claim("role",user.UserRole),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                
            };
            var token = new JwtSecurityToken
                (issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Member? AuthenticateUser(AuthData authData)
        {
             Member user = _context.Members.SingleOrDefault(x => x.UserName == authData.UserName);
             if (user != null && user.checkPassword(authData.Password))
             {
                return user;
             }
             return null;
        }

    }
}
