using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface IStockAdjustmentService
    {

        Task<IEnumerable<StockAdjustment>> GetAllStockAdjustmentAsync();
        Task<IEnumerable<StockAdjustmentItem>> GetAllStockAdjustmentItemByIdAsync(int? stockAdjustmentId);


        Task AddStockAdjustmentAsync(StockAdjustment stockAdjustment, ICollection<StockAdjustmentItem> stockAdjustmentItems);
        
        Task<StockAdjustment> GetStockAdjustmentByReferenceNumberAsync(string referenceNumber);
        Task<StockAdjustment> GetAllStockAdjustmentItemByIdAsync(string stockAdjustmentId);
        Task UpdateStockAdjustmentAsync(string referenceNumber, StockAdjustment newStockAdjustment);
        Task DeleteStockAdjustmentAsync(string referenceNumber);

        Task UpdateStockAdjustmentItemsAsync(string referenceNumber, ICollection<StockAdjustmentItem> newStockAdjustmentItems);


        Task StockTransferToDraftAsync(string referenceNumber);
        Task StockTransferToCompleteAsync(string referenceNumber);
        Task DisposeAsync();
    }
}
