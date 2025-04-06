using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface ISalesOrderService
    {
        Task AddSalesOrderAsync(SalesOrder salesOrder, ICollection<SalesOrderItem> salesorderItems);
        Task DeleteSalesOrderAsync(SalesOrder salesOrder);
        Task UpdateSalesOrderAsync(SalesOrder salesOrder);
        Task AddSalesOrderItemAsync(SalesOrderItem item);
        Task DeleteSalesOrderItemAsync(SalesOrderItem item);
        Task<IEnumerable<SalesOrder>> GetAllSalesOrderAsync();
        Task<IEnumerable<SalesOrderItem>> GetAllSalesOrderItemByIdAsync(int? salesOrderId);
        Task<SalesOrder> GetSalesOrderByReferenceNumberAsync(string referenceNumber);
        Task<SalesOrder> GetSalesOrderByIdAsync(int? salesOrderId);

        Task DisposeAsync();
    }
}
