using InventiCloud.Data;
using Microsoft.AspNetCore.Identity;

namespace InventiCloud.Utils
{
    public static class SeedAdminUser
{
    public static async Task CreateAdmin(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        var adminSettings = configuration.GetSection("InitialAdminSettings");
        var adminUserName = adminSettings["UserName"];
        var adminPassword = adminSettings["Password"];

        if (string.IsNullOrEmpty(adminUserName) || string.IsNullOrEmpty(adminPassword))
        {
            return; // Or log a warning
        }

        var adminUser = await userManager.FindByNameAsync(adminUserName);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser { UserName = adminUserName, Email = adminUserName, EmailConfirmed = true };
            var createUserResult = await userManager.CreateAsync(adminUser, adminPassword);

            if (createUserResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Administrator");
            }
            else
            {
                // Log user creation errors
                foreach (var error in createUserResult.Errors)
                {
                    Console.Write($"Error creating admin user: {error.Description}");
                }
            }
        }
        else if (!await userManager.IsInRoleAsync(adminUser, "Administrator"))
        {
            await userManager.AddToRoleAsync(adminUser, "Administrator");
        }
    }
}
}
