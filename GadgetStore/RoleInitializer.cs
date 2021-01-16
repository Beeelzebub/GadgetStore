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
            string adminLogin = "admin";
            string password = "admin";

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
            if (await userManager.FindByNameAsync(adminLogin) == null)
            {
                User admin = new User { UserName = adminLogin, FirstName = adminLogin, SecondName = adminLogin, Balance = 0};
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)   
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
