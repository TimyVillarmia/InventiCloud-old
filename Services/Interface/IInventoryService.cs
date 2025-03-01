using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface IInventoryService
    {
        Task AddInventoryAsync(Product product);
        Task DeleteInventoryAsync(Inventory inventory);

        Task<IEnumerable<Inventory>> GetAllInventoryAsync();

        Task DisposeAsync();
    }
}
