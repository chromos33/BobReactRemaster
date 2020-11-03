using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.User;
using BobReactRemaster.Services.Chat.GeneralClasses;
using Microsoft.Extensions.DependencyInjection;

namespace BobReactRemaster.Services
{
    public class UserRegistrationService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public UserRegistrationService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public string RegisterUser(string nickname)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            if (context.Members.FirstOrDefault(x => x.UserName.ToLower() == nickname.ToLower()) == null)
            {
                Member newMember = new Member(nickname);
                string Password = newMember.ResetPassword();

                context.Members.Add(newMember);
                context.SaveChanges();
                return Password;
            }
            return null;

        }
    }
}
