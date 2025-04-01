using InventiCloud.Entities;
using InventiCloud.Services.Interface;
using InventiCloud.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Services
{
    public class StockAdjustmentService : IStockAdjustmentService
    {

        private readonly ILogger<StockAdjustmentService> _logger;
        private readonly NavigationManager _navigationManager;
        private readonly IInventoryService _inventoryService;
        private readonly IDbContextFactory<InventiCloud.Data.ApplicationDbContext> _dbFactory;

        public StockAdjustmentService(ILogger<StockAdjustmentService> logger,
                                    NavigationManager navigationManager,
                                    IInventoryService inventoryService,
                                    IDbContextFactory<InventiCloud.Data.ApplicationDbContext> dbFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        public async Task<IEnumerable<StockAdjustment>> GetAllStockAdjustmentAsync()
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.StockAdjustments
                .Include(sa => sa.StockAdjustmentStatus)
                .Include(sa => sa.StockAdjustmentReason)
                .Include(sa => sa.SourceBranch)
                .Include(sa => sa.ApplicationUser)
                .Include(sa => sa.StockAdjustmentItems)
                .AsNoTracking()
                .ToListAsync();
        }

        public  async Task<IEnumerable<StockAdjustmentItem>> GetAllStockAdjustmentItemByIdAsync(int? stockAdjustmentId)
        {
            if (!stockAdjustmentId.HasValue) throw new ArgumentNullException(nameof(stockAdjustmentId), "Stock Adjustment ID cannot be null.");

            using var context = _dbFactory.CreateDbContext();

            try
            {
                var stockAdjustmentItems = await context.StockAdjustmentItems
                    .Where(sai => sai.StockAdjustmentId == stockAdjustmentId)
                    .Include(sai => sai.Product)
                    .Include(sai => sai.StockAdjustment)
                    .ToListAsync();

                if (stockAdjustmentItems == null || !stockAdjustmentItems.Any())
                {
                    _logger.LogWarning("No Stock Adjustment Items found for Stock Transfer ID: {stockAdjustmentId}.", stockAdjustmentId);
                    return new List<StockAdjustmentItem>();
                }

                return stockAdjustmentItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Stock Adjustment Items for Stock Adjustment ID: {stockAdjustmentId}.", stockAdjustmentId);
                throw;
            }
        }


        public async Task AddStockAdjustmentAsync(StockAdjustment stockAdjustment, ICollection<StockAdjustmentItem> stockAdjustmentItems)
        {
            if (stockAdjustment == null) throw new ArgumentNullException(nameof(stockAdjustment), "Stock Adjustment cannot be null.");
            if (stockAdjustmentItems == null || !stockAdjustmentItems.Any()) throw new InvalidOperationException("Stock Adjustment must have at least one item.");

            using var context = _dbFactory.CreateDbContext();

            //try
            //{
            //    stockAdjustment.StatusId = 1; // Draft
            //    context.StockAdjustments.Add(stockAdjustment);
            //    await context.SaveChangesAsync(); // Save to get StockAdjustmentId

            //    stockAdjustment.ReferenceNumber = ReferenceNumberGenerator.GenerateStockAdjustmentReference(stockAdjustment.StockAdjustmentId);
            //    context.StockAdjustments.Update(stockAdjustment);

            //    foreach (var item in stockAdjustmentItems)
            //    {
            //        item.StockAdjustmentId = stockAdjustment.StockAdjustmentId;
            //        var sourceInventory = await _inventoryService.GetInventoryByProductIdAndBranchIdAsync(item.Inventory.ProductId, stockAdjustment.SourceBranchId);

            //        if (sourceInventory == null || sourceInventory.OnHandquantity < item.NewQuantity)
            //        {
            //            throw new InvalidOperationException($"Insufficient inventory for ProductId: {item.ProductId} in SourceBranchId: {stockTransfer.SourceBranchId}");
            //        }

            //        sourceInventory.Allocated += item.TransferQuantity;
            //        sourceInventory.AvailableQuantity = sourceInventory.OnHandquantity - sourceInventory.Allocated;
            //        await _inventoryService.UpdateInventoryAsync(sourceInventory);
            //    }

            //    context.StockTransferItems.AddRange(stockTransferItems);
            //    await context.SaveChangesAsync();


            //    _navigationManager.NavigateTo($"/inventory/stock-transfers/{stockTransfer.ReferenceNumber}");
            //    _logger.LogInformation("Stock Transfer {StockTransferId} added successfully.", stockTransfer.StockTransferId);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error adding stock transfer {StockTransferId}.", stockTransfer.StockTransferId);
            //    throw;
            //}
        }

        public Task DeleteStockAdjustmentAsync(string referenceNumber)
        {
            throw new NotImplementedException();
        }

        public Task DisposeAsync()
        {
            throw new NotImplementedException();
        }

        

        
        public Task<StockAdjustment> GetAllStockAdjustmentItemByIdAsync(string stockAdjustmentId)
        {
            throw new NotImplementedException();
        }

        public Task<StockAdjustment> GetStockAdjustmentByReferenceNumberAsync(string referenceNumber)
        {
            throw new NotImplementedException();
        }

        public Task StockTransferToCompleteAsync(string referenceNumber)
        {
            throw new NotImplementedException();
        }

        public Task StockTransferToDraftAsync(string referenceNumber)
        {
            throw new NotImplementedException();
        }

        public Task UpdateStockAdjustmentAsync(string referenceNumber, StockAdjustment newStockAdjustment)
        {
            throw new NotImplementedException();
        }

        public Task UpdateStockAdjustmentItemsAsync(string referenceNumber, ICollection<StockAdjustmentItem> newStockAdjustmentItems)
        {
            throw new NotImplementedException();
        }
    }
}
