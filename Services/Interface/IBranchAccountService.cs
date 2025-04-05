using InventiCloud.Data;
using InventiCloud.Entities;
using System.Threading.Tasks;

namespace InventiCloud.Services.Interface
{
    public interface IBranchAccountService 
    {
        Task AddBranchAccountAsync(string username, string email, string password, int branchId);
        Task DeleteBranchAccountAsync(string userId);
        Task UpdateBranchAccountAsync(string userId, string? newUsername = null, string? newEmail = null, string? newPassword = null, int? newBranchId = null);
        Task<ApplicationUser> GetBranchAccountByUserName(string username);
        Task<IEnumerable<ApplicationUser>> GetAllBranchAccountsAsync();
        Task<ApplicationUser> GetBranchAccountByBranchIdAsync(int branchId);

        Task DisposeAsync();
    }

}
