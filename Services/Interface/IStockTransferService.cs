using InventiCloud.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventiCloud.Services.Interface
{
    public interface IStockTransferService
    {
        Task<IEnumerable<StockTransfer>> GetAllStockTransferAsync();
        Task<IEnumerable<StockTransferItem>> GetAllStockTransferItemByIdAsync(int? stockTransferId);


        Task AddStockTransferAsync(StockTransfer stockTransfer, ICollection<StockTransferItem> stockTransferItems);
        Task<StockTransfer> GetStockTransferByReferenceNumberAsync(string referenceNumber);
        Task<StockTransfer> GetStockTransferByIdAsync(int? stockTransferId);
        Task UpdateStockTransferAsync(string referenceNumber, StockTransfer newStockTransfer);
        Task DeleteStockTransferAsync(string referenceNumber);

        Task UpdateStockTransferItemsAsync(string referenceNumber, ICollection<StockTransferItem> updatedStockTransferItems);

        Task StockTransferToInTransitAsync(string referenceNumber);
        Task StockTransferToCompleteAsync(string referenceNumber);
        Task StockTransferCancelledAsync(string referenceNumber);
        Task DisposeAsync();
    }
}