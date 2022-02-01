using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace SMT.Access.Identity
{
    public class IdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(ApplicationUserRoles.Admin));
            await roleManager.CreateAsync(new IdentityRole(ApplicationUserRoles.User));

            var defaultUser = new ApplicationUser { UserName = "user@smt.uz", Email = "user@smt.uz" };
            await userManager.CreateAsync(defaultUser, "Artel2022");

            string adminUserName = "admin@smt.uz";
            var adminUser = new ApplicationUser { UserName = adminUserName, Email = adminUserName };
            await userManager.CreateAsync(adminUser, "Artel2022");
            adminUser = await userManager.FindByNameAsync(adminUserName);
            await userManager.AddToRoleAsync(adminUser, ApplicationUserRoles.Admin);
        }
    }
}
