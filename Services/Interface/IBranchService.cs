using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface IBranchService
    {
        Task AddBranch(Branch branch);
        Task DeleteBranch(Branch branch);
        Task<Branch> UpdateBranch(Branch branch);
        Task<bool> IsBranchExist(string branchName);
        Task<bool> IsBranchNumberExist(string branchNumber);
        Task<Branch> GetBranchByNameAsync(string branchName);

        Task<IEnumerable<Branch>> GetAllBranchAsync();

        Task DisposeAsync();
    }
}
