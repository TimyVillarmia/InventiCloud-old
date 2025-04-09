using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface IInventoryService
    {
        Task AddInventoryAsync(Product product);
        Task DeleteInventoryAsync(Inventory inventory);
        Task UpdateInventoryAsync(Inventory inventory);
        Task<Inventory> GetInventoryByProductIdAndBranchIdAsync(int productId, int branchId);
        Task PopulateNewBranchInventoryAsync(Branch branch);
        
        Task<IEnumerable<Inventory>> GetAllInventoryAsync();
        Task<IEnumerable<Inventory>> GetAllInventoryByBranchAsync(int branchId);

        Task DisposeAsync();
    }
}
