using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface IBranchService
    {
        Task AddBranch(Branch branch);
        Task DeleteBranch(Branch branch);
        Task UpdateBranch(Branch branch);
        Task<Branch> GetBranchByNameAsync(string branchName);

        Task<IEnumerable<Branch>> GetAllBranchAsync();

        Task DisposeAsync();
    }
}
