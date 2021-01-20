using GadgetStore.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GadgetStore
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("customer") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("customer"));
            }
            if (await roleManager.FindByNameAsync("seller") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("seller"));
            }
            if (await userManager.FindByNameAsync("admin") == null)
            {
                User admin = new User { UserName = "admin", FirstName = "admin", SecondName = "admin" };
                IdentityResult result = await userManager.CreateAsync(admin, "admin");
                if (result.Succeeded)   
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }

                
            }
            if (await userManager.FindByNameAsync("unknow") == null)
            {
                User unknow = new User { UserName = "unknow", FirstName = "пользователь", SecondName = "удален" };
                IdentityResult result = await userManager.CreateAsync(unknow, "unknow");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(unknow, "customer");
                }
            }

        }
    }
}
