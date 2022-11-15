﻿using PotShop;
using PotShop.API.Helpers;
using PotShop.API.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PotShop.API.Models.Entities.Enums;

namespace PotShop.API.Data
{
    public static class DefaultData
    {
        private static async Task AddStaff(UserManager<ApiUser> userMgr, string username, string name, Location location)
        {
            var user = new ApiUser()
            {
                UserName = username,
                Email = username,
                Name = name,
                Location = location,
            };

            var staffAccess = new StaffAccess()
            {
                Location = location,
                Staff = user,
                Access = Access.GetInventory
            };

            user.StaffAccess = staffAccess;

            await userMgr.CreateAsync(user, "P@ssw0rd");
        }
        private static async Task AddStaffWithAllPerms(UserManager<ApiUser> userMgr, string username, string name, Location location)
        {
            var user = new ApiUser()
            {
                UserName = username,
                Email = username,
                Name = name,
                Location = location,
            };

            var staffAccess = new StaffAccess()
            {
                Location = location,
                Staff = user,
                Access = Access.All
            };

            user.StaffAccess = staffAccess;

            await userMgr.CreateAsync(user, "P@ssw0rd");

            await userMgr.AddToRolesAsync(user, new string[] { AuthRoles.Admin, AuthRoles.Manager });
        }

        public static async Task PopulateAccounts(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApiDbContext>();
            var userMgr = serviceProvider.GetRequiredService<UserManager<ApiUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Location defaultLocation = new Location();

            if (!context.Locations.Any())
            {
                defaultLocation = new Location()
                {
                    Name = "Kodeministeriet",
                    Address = "Den der vej",
                    Type = 1,
                };
            }


            if (!await roleManager.RoleExistsAsync(AuthRoles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = AuthRoles.Admin });
            }

            if (!await roleManager.RoleExistsAsync(AuthRoles.Manager))
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = AuthRoles.Manager });
            }

            if (!context.Users.Any())
            {
                await AddStaffWithAllPerms(userMgr, "mgc@kodeministeriet.dk", "Matias Grimm", defaultLocation);

                //var mgc = new ApiUser()
                //{
                //    UserName = "mgc@kodeministeriet.dk",
                //    Email = "mgc@kodeministeriet.dk",
                //    Name = "Matias Grimm",
                //    Location = defaultLocation
                //};


                //var testManager = new ApiUser()
                //{
                //    UserName = "testManager@testing.dk",
                //    Email = "testManager@testing.dk",
                //    Name = "Test Manager",
                //    Location = defaultLocation
                //};

                //var testEmployee = new ApiUser()
                //{
                //    UserName = "TestEmployee@testing.dk",
                //    Email = "TestEmployee@testing.dk",
                //    Name = "Test Employee",
                //    Location = defaultLocation
                //};

                //await userMgr.CreateAsync(mgc, "p@ssw0rd");
                //await userMgr.CreateAsync(testEmployee, "p@ssw0rd");
                //await userMgr.CreateAsync(testManager, "p@ssw0rd");


                //await userMgr.AddToRolesAsync(mgc, new string[] { AuthRoles.Admin, AuthRoles.Manager } );
                //await userMgr.AddToRolesAsync(testManager, new string[] {AuthRoles.Manager});
            }
            if (!context.Products.Any())
            {
                Product prod1 = new Product()
                {
                    Name = "1 🥷🤖",
                    Description = "1 Måske kommer der en 🥷"
                };
                Product prod2 = new Product()
                {
                    Name = "2 🥷🤖",
                    Description = "3 Måske kommer der en 🥷"
                };
                Product prod3 = new Product()
                {
                    Name = "3 🥷🤖",
                    Description = "3 Jeg er disabled",
                    IsDisabled = true
                };

                await context.Products.AddAsync(prod1);
                await context.Products.AddAsync(prod2);
                await context.Products.AddAsync(prod3);

                await context.SaveChangesAsync();
            }
        }
    }
}
