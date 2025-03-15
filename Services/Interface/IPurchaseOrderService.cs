using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface IPurchaseOrderService
    {
        Task AddPurchaseOrderAsync(PurchaseOrder purchaseOrder, ICollection<PurchaseOrderItem> purchaseorderItems);
        Task DeletePurchaseOrderAsync(PurchaseOrder purchaseOrder);
        Task UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder);
        Task PurchaseOrderToOrderedAsync(PurchaseOrder purchaseOrder);
        Task PurchaseOrderToCompleteAsync(PurchaseOrder purchaseOrder);
        Task PurchaseOrderToCancelAsync(PurchaseOrder purchaseOrder);
        Task AddPurchaseOrderItemAsync(PurchaseOrderItem item);
        Task DeletePurchaseOrderItemAsync(PurchaseOrderItem item);
        Task UpdatePurchaseOrderStatusAsync(PurchaseOrder purchaseOrder, int statusId, string statusName);
        Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrderAsync();
        Task<IEnumerable<PurchaseOrderItem>> GetAllPurchaseOrderItemByIdAsync(int? purchaseOrderID);
        Task<PurchaseOrder> GetPurchaseOrderByReferenceNumberAsync(string referenceNumber);
        Task<PurchaseOrder> GetPurchaseOrderByIdAsync(int? purchaseOrderID);

        Task DisposeAsync();
    }
}
