using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace SMT.Access.Identity
{
    public class IdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(ApplicationUserRoles.Admin));
            await roleManager.CreateAsync(new IdentityRole(ApplicationUserRoles.User));

            var defaultUser = new ApplicationUser { UserName = "user@smt.uz", Telegram = "user@smt.uz" };
            var result = await userManager.CreateAsync(defaultUser, "User2022!");

            if (!result.Succeeded)
                throw new InvalidOperationException();

            await userManager.AddToRoleAsync(defaultUser, ApplicationUserRoles.User);

            string adminUserName = "admin@smt.uz";
            var adminUser = new ApplicationUser { UserName = adminUserName, Telegram = adminUserName };
            result = await userManager.CreateAsync(adminUser, "Admin2022!");

            if (!result.Succeeded)
                throw new InvalidOperationException();

            await userManager.AddToRoleAsync(adminUser, ApplicationUserRoles.Admin);
        }
    }
}
