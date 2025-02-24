using InventiCloud.Models;

namespace InventiCloud.Services.Interface
{
    public interface IPurchaseOrderService
    {
        Task AddAttributeSet(PurchaseOrder purchaseorder);
        Task DeleteAttributeSet(PurchaseOrder purchaseorder);

        Task<IEnumerable<PurchaseOrder>> GetAllAttributeSetAsync();

        Task DisposeAsync();
    }
}
