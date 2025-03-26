using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface IStockTransferService
    {
        Task AddStockTransferAsync(StockTransfer stockTransfer, ICollection<StockTransferItem> stockTransferItems);
        Task DeleteStockTransferAsync(StockTransfer stockTransfer);
        Task UpdateStockTransferAsync(StockTransfer stockTransfer);
        Task StockTransferToDraftAsync(StockTransfer stockTransfer);
        Task StockTransferToPendingAsync(StockTransfer stockTransfer);
        Task StockTransferToCompleteAsync(StockTransferItem stockTransfer);
        Task AddStockTransferItemAsync(StockTransferItem stockTransferItem);
        Task DeleteStockTransferItemAsync(StockTransferItem stockTransferItem);
        Task UpdateStockTransferStatusAsync(StockTransfer stockTransfer, int statusId, string statusName);
        Task<IEnumerable<StockTransfer>> GetAllStockTransferAsync();
        Task<IEnumerable<StockTransferItem>> GetAllStockTransferItemByIdAsync(int? stockTransferId);
        Task<StockTransfer> GetStockTransferByReferenceNumberAsync(string referenceNumber);
        Task<StockTransfer> GetStockTransferByIdAsync(int? stockTransferId);
        Task DisposeAsync();


    }
}
