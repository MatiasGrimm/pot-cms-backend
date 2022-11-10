using PotShop;
using PotShop.API.Helpers;
using PotShop.API.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotShop.API.Data
{
    public static class DefaultData
    {
        public static async Task PopulateAccounts(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApiDbContext>();
            var userMgr = serviceProvider.GetRequiredService<UserManager<ApiUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!context.Users.Any())
            {
                var user = new ApiUser()
                {
                    UserName = "mgc@kodeministeriet.dk",
                    Email = "mgc@kodeministeriet.dk",
                    Name = "Matias Grimm",
                    Location = new Location()
                    {
                        Name = "Kodeministeriet",
                        Address = "Den der vej",
                        Type = 1,
                    }
                };

                await userMgr.CreateAsync(user, "p@ssw0rd");

                if (!await roleManager.RoleExistsAsync(AuthRoles.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole() { Name = AuthRoles.Admin });
                }

                if (!await roleManager.RoleExistsAsync(AuthRoles.Manager))
                {
                    await roleManager.CreateAsync(new IdentityRole() { Name = AuthRoles.Manager });
                }

                await userMgr.AddToRolesAsync(user, new string[] { AuthRoles.Admin, AuthRoles.Manager } );
            }
        }
    }
}
