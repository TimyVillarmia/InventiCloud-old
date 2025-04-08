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

        public async Task<IEnumerable<StockAdjustment>> GetAllStockAdjustmentAsync(int? userBranchId)
        {
            using var context = _dbFactory.CreateDbContext();
            IQueryable<StockAdjustment> query = context.StockAdjustments
                .Include(sa => sa.StockAdjustmentStatus)
                .Include(sa => sa.StockAdjustmentReason)
                .Include(sa => sa.SourceBranch)
                .Include(sa => sa.ApplicationUser)
                .Include(sa => sa.StockAdjustmentItems)
                .AsNoTracking();

            if (userBranchId.HasValue)
            {
                query = query.Where(po => po.SourceBranchId == userBranchId);
            }

            return await query.ToListAsync();

        }

        public async Task<IEnumerable<StockAdjustmentReason>> GetAllStockAdjustmentReasonAsync()
        {
            using var context = _dbFactory.CreateDbContext();

            return await context.StockAdjustmentReasons
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
            if (stockAdjustment == null)
            {
                throw new ArgumentNullException(nameof(stockAdjustment), "Stock Adjustment cannot be null.");
            }

            if (stockAdjustmentItems == null || !stockAdjustmentItems.Any())
            {
                throw new InvalidOperationException("Stock Adjustment must have at least one item.");
            }

            using var context = _dbFactory.CreateDbContext();

            try
            {
                stockAdjustment.StatusId = 1; // Draft
                context.StockAdjustments.Add(stockAdjustment);
                await context.SaveChangesAsync(); // Save to get StockAdjustmentId

                stockAdjustment.ReferenceNumber = ReferenceNumberGenerator.GenerateStockAdjustmentReference(stockAdjustment.StockAdjustmentId);
                context.StockAdjustments.Update(stockAdjustment);


                foreach (var item in stockAdjustmentItems)
                {
                    item.StockAdjustmentId = stockAdjustment.StockAdjustmentId;

                    // Retrieve the inventory item based on ProductId and BranchId
                    var inventory = await _inventoryService.GetInventoryByProductIdAndBranchIdAsync(item.ProductId, stockAdjustment.SourceBranchId);

                    if (inventory == null)
                    {
                        throw new InvalidOperationException($"Inventory not found for ProductId: {item.ProductId} in BranchId: {stockAdjustment.SourceBranchId}.");
                    }

                    // Check if NewQuantity is the same as PreviousQuantity
                    if (item.NewQuantity == inventory.AvailableQuantity)
                    {
                        throw new InvalidOperationException($"New quantity cannot be the same as current quantity for ProductId: {item.ProductId}.");
                    }


                }

                context.StockAdjustmentItems.AddRange(stockAdjustmentItems);
                await context.SaveChangesAsync();

                _navigationManager.NavigateTo($"/inventory/stock-adjustments/{stockAdjustment.ReferenceNumber}");
                _logger.LogInformation("Stock Adjustment {StockAdjustmentId} added successfully.", stockAdjustment.StockAdjustmentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding stock adjustment {StockAdjustmentId}.", stockAdjustment.StockAdjustmentId);
                throw;
            }
        }

        public async Task DeleteStockAdjustmentAsync(string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
                throw new ArgumentNullException(nameof(referenceNumber), "Reference number cannot be null or empty.");
            }

            using var context = _dbFactory.CreateDbContext();

            try
            {
                // Find the StockAdjustment by ReferenceNumber
                var stockAdjustment = await context.StockAdjustments
                    .Include(sa => sa.StockAdjustmentStatus) // Include Status
                    .FirstOrDefaultAsync(sa => sa.ReferenceNumber == referenceNumber);

                if (stockAdjustment == null)
                {
                    throw new InvalidOperationException($"Stock Adjustment with reference number '{referenceNumber}' not found.");
                }

                // Check if status is Draft
                if (stockAdjustment.StockAdjustmentStatus.StatusName != "Draft")
                {
                    throw new InvalidOperationException($"Stock Adjustment with reference number '{referenceNumber}' cannot be deleted as its status is '{stockAdjustment.StockAdjustmentStatus.StatusName}'.");
                }

                // Remove StockAdjustmentItems first (if any)
                var adjustmentItems = await context.StockAdjustmentItems
                    .Where(sai => sai.StockAdjustmentId == stockAdjustment.StockAdjustmentId)
                    .ToListAsync();

                if (adjustmentItems.Any())
                {
                    context.StockAdjustmentItems.RemoveRange(adjustmentItems);
                    await context.SaveChangesAsync();
                }

                // Remove the StockAdjustment
                context.StockAdjustments.Remove(stockAdjustment);
                await context.SaveChangesAsync();

                _logger.LogInformation("Stock Adjustment {ReferenceNumber} deleted successfully.", referenceNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Stock Adjustment {ReferenceNumber}.", referenceNumber);
                throw;
            }
        }

        public async Task DisposeAsync()
        {
            using var context = _dbFactory.CreateDbContext();
            await context.DisposeAsync();
        }

        public async Task<StockAdjustment> GetStockAdjustmentByReferenceNumberAsync(string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
                return null;
            }

            using var context = _dbFactory.CreateDbContext();

            return await context.StockAdjustments
                .Include(sa => sa.StockAdjustmentStatus)
                .Include(sa => sa.SourceBranch)
                .Include(sa => sa.ApplicationUser)
                .Include(sa => sa.StockAdjustmentReason)
                .Include(sa => sa.StockAdjustmentItems)
                    .ThenInclude(sai => sai.Product)
                .FirstOrDefaultAsync(sa => sa.ReferenceNumber == referenceNumber);
        }

        public async Task<StockAdjustment> GetStockAdjustmentByIdAsync(int? stockAdjustmentId)
        {
            if (!stockAdjustmentId.HasValue)
            {
                return null; // Or throw ArgumentNullException, depending on your needs
            }

            using var context = _dbFactory.CreateDbContext();

            return await context.StockAdjustments
                .Include(sa => sa.StockAdjustmentStatus)
                .Include(sa => sa.SourceBranch)
                .Include(sa => sa.ApplicationUser)
                .Include(sa => sa.StockAdjustmentReason)
                .Include(sa => sa.StockAdjustmentItems)
                    .ThenInclude(sai => sai.Product)
                .FirstOrDefaultAsync(sa => sa.StockAdjustmentId == stockAdjustmentId.Value);
        }

        public async Task UpdateStockAdjustmentAsync(string referenceNumber, StockAdjustment newStockAdjustment)
        {
            if (newStockAdjustment == null)
            {
                throw new ArgumentNullException(nameof(newStockAdjustment), "Stock Adjustment cannot be null.");
            }

            using var context = _dbFactory.CreateDbContext();

            try
            {
                var existingStockAdjustment = await context.StockAdjustments
                    .Include(sa => sa.StockAdjustmentStatus)
                    .FirstOrDefaultAsync(sa => sa.ReferenceNumber == referenceNumber);

                if (existingStockAdjustment == null)
                {
                    throw new InvalidOperationException($"Stock Adjustment with Reference Number {referenceNumber} not found.");
                }

                if (existingStockAdjustment.StockAdjustmentStatus.StatusName != "Draft")
                {
                    throw new InvalidOperationException("Cannot update Stock Adjustment. It is not in 'Draft' status.");
                }

                // Update Stock Adjustment Details
                existingStockAdjustment.SourceBranchId = newStockAdjustment.SourceBranchId;
                existingStockAdjustment.ReasonId = newStockAdjustment.ReasonId;

                context.StockAdjustments.Update(existingStockAdjustment);
                await context.SaveChangesAsync();

                _logger.LogInformation("Stock Adjustment {ReferenceNumber} updated.", referenceNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating stock adjustment {ReferenceNumber}.", referenceNumber);
                throw;
            }
        }


        public async Task UpdateStockAdjustmentItemsAsync(string referenceNumber, ICollection<StockAdjustmentItem> newStockAdjustmentItems)
        {
            using var context = _dbFactory.CreateDbContext();

            try
            {
                var existingAdjustment = await context.StockAdjustments
                    .Include(sa => sa.StockAdjustmentItems)
                    .FirstOrDefaultAsync(sa => sa.ReferenceNumber == referenceNumber);

                if (existingAdjustment == null)
                {
                    throw new InvalidOperationException($"Stock Adjustment with Reference Number {referenceNumber} not found.");
                }

                var existingItems = existingAdjustment.StockAdjustmentItems.ToList();

                // Delete items not in the new list
                var itemsToDelete = existingItems.Where(existingItem => !newStockAdjustmentItems.Any(newItem => newItem.StockAdjustmentItemId == existingItem.StockAdjustmentItemId)).ToList();
                context.StockAdjustmentItems.RemoveRange(itemsToDelete);

                foreach (var newItem in newStockAdjustmentItems)
                {
                    var existingItem = existingItems.FirstOrDefault(item => item.StockAdjustmentItemId == newItem.StockAdjustmentItemId);

                    if (existingItem != null)
                    {
                        // Update existing item
                        if (newItem.NewQuantity == newItem.PreviousQuantity)
                        {
                            throw new InvalidOperationException($"New quantity cannot be the same as previous quantity for StockAdjustmentItemId: {newItem.StockAdjustmentItemId}.");
                        }

                        existingItem.PreviousQuantity = newItem.PreviousQuantity;
                        existingItem.NewQuantity = newItem.NewQuantity;
                        existingItem.AdjustedQuantity = newItem.NewQuantity - newItem.PreviousQuantity;
                    }
                    else
                    {
                        // Add new item
                        if (newItem.NewQuantity == newItem.PreviousQuantity)
                        {
                            throw new InvalidOperationException("New quantity cannot be the same as previous quantity for a new StockAdjustmentItem.");
                        }

                        newItem.StockAdjustmentId = existingAdjustment.StockAdjustmentId;
                        context.StockAdjustmentItems.Add(newItem); // Let the database generate StockAdjustmentItemId
                    }
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Stock Adjustment Items for ReferenceNumber: {ReferenceNumber}", referenceNumber);
                throw; // Rethrow the exception
            }
        }

        public async Task StockAdjustmentToCompleteAsync(string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
                throw new ArgumentNullException(nameof(referenceNumber), "Reference number cannot be null or empty.");
            }

            using var context = _dbFactory.CreateDbContext();

            try
            {
                var stockAdjustment = await context.StockAdjustments
                    .Include(sa => sa.StockAdjustmentItems)
                    .Include(sa => sa.StockAdjustmentStatus)
                    .FirstOrDefaultAsync(sa => sa.ReferenceNumber == referenceNumber);

                if (stockAdjustment == null)
                {
                    throw new InvalidOperationException($"Stock Adjustment with reference number '{referenceNumber}' not found.");
                }

                if (stockAdjustment.StockAdjustmentStatus.StatusName != "Draft")
                {
                    throw new InvalidOperationException($"Stock Adjustment with reference number '{referenceNumber}' cannot be completed as its status is '{stockAdjustment.StockAdjustmentStatus.StatusName}'.");
                }

                foreach (var item in stockAdjustment.StockAdjustmentItems)
                {
                    // Retrieve the inventory item based on ProductId and BranchId
                    var inventory = await _inventoryService.GetInventoryByProductIdAndBranchIdAsync(item.ProductId, stockAdjustment.SourceBranchId);

                    if (inventory == null)
                    {
                        throw new InvalidOperationException($"Inventory not found for ProductId: {item.ProductId} in BranchId: {stockAdjustment.SourceBranchId}.");
                    }

                    // Check if NewQuantity is valid against AvailableQuantity
                    if (item.NewQuantity == inventory.AvailableQuantity)
                    {
                        throw new InvalidOperationException($"New quantity cannot be the same as current quantity for ProductId: {item.ProductId}.");
                    }

                    // Calculate the adjusted quantity
                    item.AdjustedQuantity = item.NewQuantity - item.PreviousQuantity;

                    // Update the inventory based on the adjustment (using AvailableQuantity)
                    inventory.AvailableQuantity = item.NewQuantity;
                    inventory.OnHandquantity = inventory.AvailableQuantity + inventory.Allocated;

                    // Update the inventory in the database
                    await _inventoryService.UpdateInventoryAsync(inventory);
                }

                // Update StockAdjustment Status to "Completed"
                stockAdjustment.StatusId = context.StockAdjustmentStatuses.FirstOrDefault(s => s.StatusName == "Completed").StockAdjustmentStatusId;

                await context.SaveChangesAsync();

                _logger.LogInformation("Stock Adjustment {ReferenceNumber} completed successfully.", referenceNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing Stock Adjustment {ReferenceNumber}.", referenceNumber);
                throw;
            }
        }

      
    }
}
