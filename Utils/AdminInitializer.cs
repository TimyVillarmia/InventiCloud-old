
using InventiCloud.Data;
using Microsoft.AspNetCore.Identity;

namespace InventiCloud.Utils
{
    public class AdminInitializer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AdminInitializer> _logger;
        private readonly IConfiguration _configuration;


        public AdminInitializer(IServiceProvider serviceProvider, ILogger<AdminInitializer> logger, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _configuration = configuration;

        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Define roles
            const string adminRoleName = "Administrator";
            const string branchRoleName = "Branch";
            if (!await roleManager.RoleExistsAsync(adminRoleName))
            {
                var adminRoleResult = await roleManager.CreateAsync(new IdentityRole(adminRoleName));
                if (adminRoleResult.Succeeded)
                {
                    _logger.LogInformation($"Admin role '{adminRoleName}' created successfully.");
                }
                else
                {
                    _logger.LogError($"Error creating admin role '{adminRoleName}': {string.Join(", ", adminRoleResult.Errors.Select(e => e.Description))}");
                    return; // Stop if role creation fails
                }
            }

            // Create Branch role if it doesn't exist
            if (!await roleManager.RoleExistsAsync(branchRoleName))
            {
                var branchRoleResult = await roleManager.CreateAsync(new IdentityRole(branchRoleName));
                if (branchRoleResult.Succeeded)
                {
                    _logger.LogInformation($"Role '{branchRoleName}' created successfully.");
                }
                else
                {
                    _logger.LogError($"Error creating role '{branchRoleName}': {string.Join(", ", branchRoleResult.Errors.Select(e => e.Description))}");
                    return;
                }
            }

            // Define the admin user

            var adminSettings = _configuration.GetSection("InitialAdminSettings");
            var adminUserName = adminSettings["UserName"];
            var adminPassword = adminSettings["Password"];

            if (string.IsNullOrEmpty(adminUserName) || string.IsNullOrEmpty(adminPassword))
            {
                _logger.LogError("Initial admin username or password not configured in appsettings.");
                return;
            }



            var adminUser = await userManager.FindByNameAsync(adminUserName);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser { UserName = adminUserName, Email = adminUserName, EmailConfirmed = true };
                var createUserResult = await userManager.CreateAsync(adminUser, adminPassword);

                if (createUserResult.Succeeded)
                {
                    _logger.LogInformation($"Admin user '{adminUserName}' created successfully.");

                    // Assign the admin role to the user
                    if (!await userManager.IsInRoleAsync(adminUser, adminRoleName))
                    {
                        var addToRoleResult = await userManager.AddToRoleAsync(adminUser, adminRoleName);
                        if (addToRoleResult.Succeeded)
                        {
                            _logger.LogInformation($"Admin user '{adminUserName}' added to role '{adminRoleName}'.");
                        }
                        else
                        {
                            _logger.LogError($"Error adding admin user '{adminUserName}' to role '{adminRoleName}': {string.Join(", ", addToRoleResult.Errors.Select(e => e.Description))}");
                        }
                    }
                }
                else
                {
                    _logger.LogError($"Error creating admin user '{adminUserName}': {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                _logger.LogInformation($"Admin user '{adminUserName}' already exists.");
                // Ensure the admin user is in the admin role (in case it was removed)
                if (!await userManager.IsInRoleAsync(adminUser, adminRoleName))
                {
                    var addToRoleResult = await userManager.AddToRoleAsync(adminUser, adminRoleName);
                    if (addToRoleResult.Succeeded)
                    {
                        _logger.LogInformation($"Admin user '{adminUserName}' re-added to role '{adminRoleName}'.");
                    }
                    else
                    {
                        _logger.LogError($"Error re-adding admin user '{adminUserName}' to role '{adminRoleName}': {string.Join(", ", addToRoleResult.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    }
}
