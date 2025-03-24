using InventiCloud.Entities;
using InventiCloud.Services.Interface;

namespace InventiCloud.Services
{
    public class StockTransferService : IStockTransferService
    {
        public Task AddStockTransferAsync(StockTransfer stockTransfer, ICollection<StockTransferItem> stockTransferItems)
        {
            throw new NotImplementedException();
        }

        public Task AddStockTransferItemAsync(StockTransferItem stockTransferItem)
        {
            throw new NotImplementedException();
        }

        public Task DeleteStockTransferAsync(StockTransfer stockTransfer)
        {
            throw new NotImplementedException();
        }

        public Task DeleteStockTransferItemAsync(StockTransferItem stockTransferItem)
        {
            throw new NotImplementedException();
        }

        public Task DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StockTransfer>> GetAllStockTransferAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StockTransferItem>> GetAllStockTransferItemByIdAsync(int? stockTransferId)
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseOrder> GetPurchaseOrderByIdAsync(int? stockTransferId)
        {
            throw new NotImplementedException();
        }

        public Task<StockTransfer> GetPurchaseOrderByReferenceNumberAsync(string referenceNumber)
        {
            throw new NotImplementedException();
        }

        public Task StockTransferToCompleteAsync(StockTransferItem stockTransfer)
        {
            throw new NotImplementedException();
        }

        public Task StockTransferToDraftAsync(StockTransfer stockTransfer)
        {
            throw new NotImplementedException();
        }

        public Task StockTransferToPendingAsync(StockTransfer stockTransfer)
        {
            throw new NotImplementedException();
        }

        public Task UpdateStockTransferAsync(StockTransfer stockTransfer)
        {
            throw new NotImplementedException();
        }

        public Task UpdateStockTransferStatusAsync(StockTransfer stockTransfer, int statusId, string statusName)
        {
            throw new NotImplementedException();
        }
    }
}
