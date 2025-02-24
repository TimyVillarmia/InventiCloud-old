using InventiCloud.Models;

namespace InventiCloud.Services.Interface
{
    public interface IPurchaseOrderService
    {
        Task AddPurchaseOrder(PurchaseOrder purchaseorder);
        Task DeletePurchaseOrder(PurchaseOrder purchaseorder);

        Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrderAsync();

        Task DisposeAsync();
    }
}
