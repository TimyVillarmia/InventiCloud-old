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
        Task DeleteStockTransferAsync(string referenceNumber);
        Task StockTransferToApprovedAsync(string referenceNumber);
        Task StockTransferToCompletedAsync(string referenceNumber);
        Task StockTransferToRejectedAsync(string referenceNumber);
        Task DisposeAsync();
    }
}