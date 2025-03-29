using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface IStockTransferService
    {
        Task AddStockTransferAsync(StockTransfer stockTransfer, ICollection<StockTransferItem> stockTransferItems);
        Task DeleteStockTransferAsync(string referenceNumber);
        Task UpdateStockTransferAsync(string referenceNumber, StockTransfer newStockTransfer);
        Task StockTransferToAllocatedAsync(string referenceNumber);
        Task StockTransferToInTransitAsync(string referenceNumber);
        Task StockTransferToCompleteAsync(string referenceNumber);
        Task StockTransferCancelledAsync(string referenceNumber);

        Task UpdateStockTransferItemsAsync(string referenceNumber, ICollection<StockTransferItem> updatedStockTransferItems);
        Task UpdateStockTransferStatusAsync(StockTransfer stockTransfer, int statusId, string statusName);
        Task<IEnumerable<StockTransfer>> GetAllStockTransferAsync();
        Task<IEnumerable<StockTransferItem>> GetAllStockTransferItemByIdAsync(int? stockTransferId);
        Task<StockTransfer> GetStockTransferByReferenceNumberAsync(string referenceNumber);
        Task<StockTransfer> GetStockTransferByIdAsync(int? stockTransferId);
        Task DisposeAsync();


    }
}
