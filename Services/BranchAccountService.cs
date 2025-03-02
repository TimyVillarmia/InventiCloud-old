using InventiCloud.Data;
using InventiCloud.Entities;
using InventiCloud.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Services
{
    public class BranchAccountService(ILogger<BranchAccountService> _logger, UserManager<ApplicationUser> _userManager, IDbContextFactory<InventiCloud.Data.ApplicationDbContext> DbFactory) : IBranchAccountService
    {

        public async Task AddBranchAccountAsync(string username, string email, string password, Branch branch)
        {
            var user = new ApplicationUser { UserName = username, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                // Handle errors
                string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create user: {errors}");
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
