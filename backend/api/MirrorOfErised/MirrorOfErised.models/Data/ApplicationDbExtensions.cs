using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirrorOfErised.models.Data
{
    public static class ApplicationDbExtensions
    {


        public async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {



            await roleManager.CreateAsync(new IdentityRole("Admin"));  //Admin over alles

        }

        public async static Task SeedUsers(UserManager<IdentityUser> userMgr, RoleManager<IdentityRole> roleManager)
        {
            //1. Admin aanmaken ---------------------------------------------------
            if (await userMgr.FindByNameAsync("MirrorOfErised") == null)  //controleer de UserName
            {
                var user = new IdentityUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "mirror@mirror.ow",
                    Email = "mirror@mirror.ow",
                    EmailConfirmed = true
                };

                var userResult = await userMgr.CreateAsync(user, "D@s1re");
                var role = (await roleManager.FindByNameAsync("Admin"));
                var roleResult = await userMgr.AddToRoleAsync(user, role.Name);
                // var claimResult = await userMgr.AddClaimAsync(user, new Claim("DocentWeb", "True"));

                if (!userResult.Succeeded || !roleResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }

            }
            
        }



       
    }
}