using InventiCloud.Data;
using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface IBranchAccountService
    {
        Task AddBranchAccountAsync(string username, string email, string password, Branch branch);
        Task DeleteBranchAccountAsync(BranchAccount branchAccount);

        Task<IEnumerable<BranchAccount>> GetAllBranchAccountAsync();

        Task DisposeAsync();
    }
}
