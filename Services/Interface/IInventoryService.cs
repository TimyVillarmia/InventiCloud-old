using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface IInventoryService
    {
        Task AddInventoryAsync(Product product);
        Task DeleteInventoryAsync(Inventory inventory);

        Task<IEnumerable<Inventory>> GetAllInventoryAsync();
        Task<IEnumerable<Inventory>> GetAllInventoryByBranchAsync(int branchId);

        Task DisposeAsync();
    }
}
