using InventiCloud.Data;
using Microsoft.AspNetCore.Identity;

namespace InventiCloud.Utils
{
    public class PasswordHasherUtility
    {
        public static string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            return passwordHasher.HashPassword(null, password); // Pass null as the user instance
        }
    }
}
