using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface IPurchaseOrderService
    {
        Task AddPurchaseOrderAsync(PurchaseOrder purchaseorder, ICollection<PurchaseOrderItem> purchaseorderItems);
        Task DeletePurchaseOrderAsync(PurchaseOrder purchaseorder);
        Task UpdatePurchaseOrderAsync(PurchaseOrder purchaseorder);
        Task SetPurchaseOrderStatusAsync(PurchaseOrder purchaseorder, string statusName);

        Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrderAsync();

        Task DisposeAsync();
    }
}
