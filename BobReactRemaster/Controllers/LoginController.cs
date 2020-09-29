using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BobReactRemaster.Auth;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.User;
using BobReactRemaster.EventBus;
using BobReactRemaster.JSONModels;
using BobReactRemaster.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace BobReactRemaster.Controllers
{
    [ApiController]
    [Route("User")]
    public class LoginController : ControllerBase
    {
        private readonly IMessageBus _eventBus;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        public LoginController(IMessageBus eventBus, IConfiguration config, ApplicationDbContext context)
        {
            _eventBus = eventBus;
            _config = config;
            _context = context;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] AuthData authData)
        {
            IActionResult response = Unauthorized();

            Member user = AuthenticateUser(authData);
            if(user != null)
            {
                string tokenString = GenerateJWTToken(user);
                response = Ok(new{token = tokenString,userName = user.UserName});
            }

            return response;
        }

        [HttpGet]
        [Route("Setup")]
        [Authorize(Policy = Policies.Admin)]
        public IActionResult Setup()
        {
            return Ok("This is a response from admin method");
        }

        private string GenerateJWTToken(Member user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken
                (issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Member AuthenticateUser(AuthData authData)
        {
             Member user = _context.Members.SingleOrDefault(x => x.UserName == authData.UserName && x.Password == authData.Password);
             return user;
        }

    }
}
