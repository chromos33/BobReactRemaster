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
        public static async Task<bool> SeedUsers(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            
            if (dbContext.Members.Count(x => x.UserName == "Master") == 0)
            {
                Member user = new Member("Master","setuppw",UserRole.Admin);
                await dbContext.Members.AddAsync(user);
                await dbContext.SaveChangesAsync();
            }
            return true;
        }
    }
}
