using InventiCloud.Data;
using InventiCloud.Entities;
using InventiCloud.Migrations;
using InventiCloud.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Services
{
    public class BranchAccountService(ILogger<BranchAccountService> _logger,
        UserManager<ApplicationUser> _userManager,
        RoleManager<IdentityRole> _roleManager,
        IDbContextFactory<InventiCloud.Data.ApplicationDbContext> DbFactory) : IBranchAccountService
    {

        public async Task AddBranchAccountAsync(string username, string email, string password, Branch branch)
        {
            try
            {
                using var context = DbFactory.CreateDbContext();

                //create identity user
                var user = new ApplicationUser { UserName = username, Email = email };
                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    // Handle errors
                    string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create user: {errors}");
                }


                // Create BranchAccount
                var newBranchAccount = new BranchAccount
                {
                    ApplicationUserId = user.Id,
                    BranchId = branch.BranchId,
                };

                // add branch account
                context.BranchAccounts.Add(newBranchAccount);
                await context.SaveChangesAsync();

                // create role
                if (!await _roleManager.RoleExistsAsync("branch"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("branch"));
                }

                //assign role
                await _userManager.AddToRoleAsync(user, "branch");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }




        public Task DeleteBranchAccountAsync(BranchAccount branchAccount)
        {
            throw new NotImplementedException();
        }

        public Task DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BranchAccount>> GetAllBranchAccountAsync()
        {
            throw new NotImplementedException();
        }
    }
}
