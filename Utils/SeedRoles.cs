using Microsoft.AspNetCore.Identity;

namespace InventiCloud.Utils
{
    public class SeedRoles
    {
        public static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                await roleManager.CreateAsync(new IdentityRole("Administrator"));
            }
            if (!await roleManager.RoleExistsAsync("Branch"))
            {
                await roleManager.CreateAsync(new IdentityRole("Branch"));
            }
            // Add other roles as needed
        }
    }
}
