using BobReactRemaster.Data.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Data
{
    public class ApplicationDBInitializer
    {
        public async static Task<bool> SeedUsers(IServiceProvider serviceProvider)
        {
            var _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Member>>();
            
            string[] roles = new string[] { "Manager", "Admin","User"};
            foreach(string rolename in roles)
            {
                if (await _roleManager.RoleExistsAsync(rolename))
                {
                    var role = new IdentityRole();
                    role.Name = rolename;
                    await _roleManager.CreateAsync(role);
                }
            }
            
            if(userManager.FindByNameAsync("Master").Result == null)
            {
                Member user = new Member("Master");
                IdentityResult result = userManager.CreateAsync(user, "setuppw").Result;
                if(result.Succeeded)
                {
                    await userManager.AddToRolesAsync(user,new string[] { "Manager", "Admin", "User" });
                }
            }
            return true;
        }
    }
}
