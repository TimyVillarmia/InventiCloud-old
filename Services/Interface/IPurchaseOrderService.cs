using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface IPurchaseOrderService
    {
        Task AddPurchaseOrderAsync(PurchaseOrder purchaseorder, ICollection<PurchaseOrderItem> purchaseorderItems);
        Task DeletePurchaseOrderAsync(PurchaseOrder purchaseorder);
        Task UpdatePurchaseOrderAsync(PurchaseOrder purchaseorder);
        Task PurchaseOrderToOrderedAsync(PurchaseOrder purchaseorder);
        Task PurchaseOrderToCompleteAsync(PurchaseOrder purchaseorder);
        Task PurchaseOrderToCancelAsync(PurchaseOrder purchaseorder);

        Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrderAsync();

        Task DisposeAsync();
    }
}
