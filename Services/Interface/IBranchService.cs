using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface IBranchService
    {
        Task AddBranch(Branch branch);
        Task DeleteBranch(Branch branch);

        Task<IEnumerable<Branch>> GetAllBranchAsync();

        Task DisposeAsync();
    }
}
