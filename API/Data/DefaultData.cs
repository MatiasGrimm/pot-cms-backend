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
                    UserName = "mum@kodeministeriet.dk",
                    Email = "mum@kodeministeriet.dk",
                    Name = "Magnus Madsen",
                    Company = new Company()
                    {
                        Name = "Kodeministeriet",
                        CVR = "41728086",
                        Phone = "53540855",
                    }
                };

                await userMgr.CreateAsync(user, "312Kodemini");

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
