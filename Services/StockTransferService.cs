using InventiCloud.Data;
using InventiCloud.Entities;
using InventiCloud.Services.Interface;
using InventiCloud.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventiCloud.Services
{
    public class StockTransferService : IStockTransferService
    {
        private readonly ILogger<StockTransferService> _logger;
        private readonly NavigationManager _navigationManager;
        private readonly IInventoryService _inventoryService;
        private readonly IDbContextFactory<InventiCloud.Data.ApplicationDbContext> _dbFactory;

        public StockTransferService(ILogger<StockTransferService> logger,
                                    NavigationManager navigationManager,
                                    IInventoryService inventoryService,
                                    IDbContextFactory<InventiCloud.Data.ApplicationDbContext> dbFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        public async Task<IEnumerable<StockTransfer>> GetAllStockTransferAsync()
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.StockTransfers
                .Include(st => st.CreatedBy)
                .Include(st => st.SourceBranch)
                .Include(st => st.DestinationBranch)
                .Include(st => st.Status)
                .Include(st => st.StockTransferItems)
                    .ThenInclude(st => st.Product)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<StockTransferItem>> GetAllStockTransferItemByIdAsync(int? stockTransferId)
        {
            if (!stockTransferId.HasValue) throw new ArgumentNullException(nameof(stockTransferId), "Stock Transfer ID cannot be null.");

            using var context = _dbFactory.CreateDbContext();

            try
            {
                var stockTransferItems = await context.StockTransferItems
                    .Where(sti => sti.StockTransferId == stockTransferId)
                    .Include(sti => sti.Product)
                    .ToListAsync();

                if (stockTransferItems == null || !stockTransferItems.Any())
                {
                    _logger.LogWarning("No Stock Transfer Items found for Stock Transfer ID: {StockTransferId}.", stockTransferId);
                    return new List<StockTransferItem>();
                }

                return stockTransferItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Stock Transfer Items for Stock Transfer ID: {StockTransferId}.", stockTransferId);
                throw;
            }
        }

        public async Task AddStockTransferAsync(StockTransfer stockTransfer, ICollection<StockTransferItem> stockTransferItems)
        {
            if (stockTransfer == null) throw new ArgumentNullException(nameof(stockTransfer), "Stock Transfer cannot be null.");
            if (stockTransferItems == null || !stockTransferItems.Any()) throw new InvalidOperationException("Stock Transfer must have at least one item.");

            using var context = _dbFactory.CreateDbContext();

            try
            {
                stockTransfer.StatusId = 1; // Allocated
                context.StockTransfers.Add(stockTransfer);
                await context.SaveChangesAsync(); // Save to get StockTransferId

                stockTransfer.ReferenceNumber = ReferenceNumberGenerator.GenerateStockTransferReference(stockTransfer.StockTransferId);
                context.StockTransfers.Update(stockTransfer);

                foreach (var item in stockTransferItems)
                {
                    item.StockTransferId = stockTransfer.StockTransferId;
                    var sourceInventory = await _inventoryService.GetInventoryByProductIdAndBranchIdAsync(item.ProductId, stockTransfer.SourceBranchId);

                    if (sourceInventory == null || sourceInventory.OnHandquantity < item.TransferQuantity)
                    {
                        throw new InvalidOperationException($"Insufficient inventory for ProductId: {item.ProductId} in SourceBranchId: {stockTransfer.SourceBranchId}");
                    }

                    sourceInventory.Allocated += item.TransferQuantity;
                    sourceInventory.AvailableQuantity = sourceInventory.OnHandquantity - sourceInventory.Allocated;
                    await _inventoryService.UpdateInventoryAsync(sourceInventory);
                }

                context.StockTransferItems.AddRange(stockTransferItems);
                await context.SaveChangesAsync();


                _navigationManager.NavigateTo($"/inventory/stock-transfers/{stockTransfer.ReferenceNumber}");
                _logger.LogInformation("Stock Transfer {StockTransferId} added successfully.", stockTransfer.StockTransferId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding stock transfer {StockTransferId}.", stockTransfer.StockTransferId);
                throw;
            }
        }

        public async Task<StockTransfer> GetStockTransferByReferenceNumberAsync(string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber)) throw new ArgumentNullException(nameof(referenceNumber), "Reference Number cannot be null or empty.");

            using var context = _dbFactory.CreateDbContext();

            try
            {
                return await context.StockTransfers
                    .Include(st => st.CreatedBy)
                    .Include(st => st.SourceBranch)
                    .Include(st => st.DestinationBranch)
                    .Include(st => st.Status)
                    .Include(st => st.StockTransferItems)
                        .ThenInclude(st => st.Product)
                    .FirstAsync(po => po.ReferenceNumber == referenceNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving purchase order by reference number.");
                return null;
            }
        }

        public async Task<StockTransfer> GetStockTransferByIdAsync(int? stockTransferId)
        {
            if (!stockTransferId.HasValue) throw new ArgumentNullException(nameof(stockTransferId), "Stock Transfer ID cannot be null.");

            using var context = _dbFactory.CreateDbContext();

            try
            {
                var stockTransfer = await context.StockTransfers
                    .Include(st => st.Status)
                    .Include(st => st.SourceBranch)
                    .Include(st => st.DestinationBranch)
                    .Include(st => st.StockTransferItems)
                        .ThenInclude(sti => sti.Product)
                    .FirstOrDefaultAsync(st => st.StockTransferId == stockTransferId);

                if (stockTransfer == null)
                {
                    _logger.LogWarning("Stock Transfer with ID {StockTransferId} not found.", stockTransferId);
                    return null;
                }

                return stockTransfer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Stock Transfer with ID {StockTransferId}.", stockTransferId);
                throw;
            }
        }

        public async Task UpdateStockTransferAsync(string referenceNumber, StockTransfer newStockTransfer)
        {
            if (newStockTransfer == null)
            {
                throw new ArgumentNullException(nameof(newStockTransfer), "Stock Transfer cannot be null.");
            }

            using var context = _dbFactory.CreateDbContext();

            try
            {
                var existingStockTransfer = await context.StockTransfers
                    .Include(st => st.Status)
                    .Include(st => st.StockTransferItems)
                    .FirstOrDefaultAsync(st => st.ReferenceNumber == referenceNumber);

                if (existingStockTransfer == null)
                {
                    throw new InvalidOperationException($"Stock Transfer with Reference Number {referenceNumber} not found.");
                }

                if (existingStockTransfer.Status.StatusName != "Allocated")
                {
                    throw new InvalidOperationException("Cannot update Stock Transfer. It is not in 'Allocated' status.");
                }

                // Update Stock Transfer Details
                existingStockTransfer.SourceBranchId = newStockTransfer.SourceBranchId;
                existingStockTransfer.DestinationBranchId = newStockTransfer.DestinationBranchId;

               
                context.StockTransfers.Update(existingStockTransfer);
                await context.SaveChangesAsync();

                _logger.LogInformation("Stock Transfer {ReferenceNumber} updated.", referenceNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating stock transfer {ReferenceNumber}.", referenceNumber);
                throw;
            }
        }


        public async Task DeleteStockTransferAsync(string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber)) throw new ArgumentNullException(nameof(referenceNumber), "Reference number cannot be null or empty.");

            using var context = _dbFactory.CreateDbContext();

            try
            {
                var stockTransfer = await context.StockTransfers
                    .Include(st => st.Status)
                    .Include(st => st.StockTransferItems)
                    .FirstOrDefaultAsync(st => st.ReferenceNumber == referenceNumber);

                if (stockTransfer == null) throw new InvalidOperationException($"Stock Transfer with Reference Number {referenceNumber} not found.");
                if (stockTransfer.Status.StatusName != "Allocated") throw new InvalidOperationException($"Stock Transfer with Reference Number {referenceNumber} is not in 'Allocated' status and cannot be deleted.");

                foreach (var item in stockTransfer.StockTransferItems)
                {
                    var sourceInventory = await _inventoryService.GetInventoryByProductIdAndBranchIdAsync(item.ProductId, stockTransfer.SourceBranchId);

                    if (sourceInventory == null) throw new InvalidOperationException($"Inventory not found for ProductId: {item.ProductId} and BranchId: {stockTransfer.SourceBranchId}");

                    sourceInventory.Allocated -= item.TransferQuantity;
                    sourceInventory.AvailableQuantity = sourceInventory.OnHandquantity - sourceInventory.Allocated;

                    await _inventoryService.UpdateInventoryAsync(sourceInventory);
                }

                context.StockTransfers.Remove(stockTransfer);
                await context.SaveChangesAsync();

                _logger.LogInformation("Stock Transfer {ReferenceNumber} deleted.", referenceNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting stock transfer {ReferenceNumber}.", referenceNumber);
                throw;
            }
        }


        public async Task UpdateStockTransferItemsAsync(string referenceNumber, ICollection<StockTransferItem> updatedStockTransferItems)
        {
            using var context = _dbFactory.CreateDbContext();

            try
            {
                var stockTransfer = await context.StockTransfers
                    .Include(st => st.StockTransferItems)
                    .FirstOrDefaultAsync(st => st.ReferenceNumber == referenceNumber);

                if (stockTransfer == null)
                {
                    _logger.LogWarning("Stock Transfer with reference number '{ReferenceNumber}' not found.", referenceNumber);
                    throw new InvalidOperationException($"Stock Transfer with reference number '{referenceNumber}' not found.");
                }

                var existingStockTransferItems = stockTransfer.StockTransferItems.ToList();

                // Deletions
                foreach (var existingItem in existingStockTransferItems)
                {
                    if (!updatedStockTransferItems.Any(updatedItem => updatedItem.StockTransferItemlId == existingItem.StockTransferItemlId))
                    {
                        context.StockTransferItems.Remove(existingItem);

                        // Update Inventory on Deletion
                        var sourceInventory = await _inventoryService.GetInventoryByProductIdAndBranchIdAsync(existingItem.ProductId, stockTransfer.SourceBranchId);
                        if (sourceInventory != null)
                        {
                            sourceInventory.Allocated -= existingItem.TransferQuantity; // Corrected: Reduce the allocation
                            sourceInventory.AvailableQuantity = sourceInventory.OnHandquantity - sourceInventory.Allocated;
                            await _inventoryService.UpdateInventoryAsync(sourceInventory);
                        }
                        else
                        {
                            _logger.LogWarning($"Source Inventory not found for ProductId: {existingItem.ProductId} and BranchId: {stockTransfer.SourceBranchId}");
                        }
                    }
                }

                // Additions and Modifications
                foreach (var updatedItem in updatedStockTransferItems)
                {
                    var existingStockTransferItem = existingStockTransferItems.FirstOrDefault(sti => sti.StockTransferItemlId == updatedItem.StockTransferItemlId);

                    if (existingStockTransferItem == null)
                    {
                        // Addition
                        updatedItem.StockTransferId = stockTransfer.StockTransferId;
                        context.StockTransferItems.Add(updatedItem);

                        // Update Inventory on Addition
                        var sourceInventory = await _inventoryService.GetInventoryByProductIdAndBranchIdAsync(updatedItem.ProductId, stockTransfer.SourceBranchId);
                        if (sourceInventory != null)
                        {
                            sourceInventory.Allocated += updatedItem.TransferQuantity; // Increase Allocation
                            sourceInventory.AvailableQuantity = sourceInventory.OnHandquantity - sourceInventory.Allocated;
                            await _inventoryService.UpdateInventoryAsync(sourceInventory);
                        }
                        else
                        {
                            _logger.LogWarning($"Source Inventory not found for ProductId: {updatedItem.ProductId} and BranchId: {stockTransfer.SourceBranchId}");
                        }
                    }
                    else
                    {
                        // Modification
                        context.Entry(existingStockTransferItem).CurrentValues.SetValues(updatedItem);

                        // Update Inventory on Modification (if TransferQuantity changed)
                        if (existingStockTransferItem.TransferQuantity != updatedItem.TransferQuantity)
                        {
                            var sourceInventory = await _inventoryService.GetInventoryByProductIdAndBranchIdAsync(updatedItem.ProductId, stockTransfer.SourceBranchId);
                            if (sourceInventory != null)
                            {
                                sourceInventory.Allocated += (updatedItem.TransferQuantity - existingStockTransferItem.TransferQuantity); // Adjust allocation
                                sourceInventory.AvailableQuantity = sourceInventory.OnHandquantity - sourceInventory.Allocated;
                                await _inventoryService.UpdateInventoryAsync(sourceInventory);
                            }
                            else
                            {
                                _logger.LogWarning($"Source Inventory not found for ProductId: {updatedItem.ProductId} and BranchId: {stockTransfer.SourceBranchId}");
                            }
                        }
                    }
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating stock transfer {ReferenceNumber}.", referenceNumber);
                throw;
            }
        }

        public async Task StockTransferToInTransitAsync(string referenceNumber) => await UpdateStockTransferStatusAndInventory(referenceNumber, "In Transit");

        public async Task StockTransferToCompleteAsync(string referenceNumber) => await UpdateStockTransferStatusAndInventory(referenceNumber, "Completed");

        public async Task StockTransferCancelledAsync(string referenceNumber) => await UpdateStockTransferStatusAndInventory(referenceNumber, "Cancelled");

        private async Task UpdateStockTransferStatusAndInventory(string referenceNumber, string statusName)
        {
            if (string.IsNullOrEmpty(referenceNumber)) throw new ArgumentNullException(nameof(referenceNumber), "Reference number cannot be null or empty.");

            using var context = _dbFactory.CreateDbContext();

            try
            {
                var stockTransfer = await context.StockTransfers
                    .Include(st => st.Status)
                    .Include(st => st.StockTransferItems)
                    .FirstOrDefaultAsync(st => st.ReferenceNumber == referenceNumber);

                if (stockTransfer == null) throw new InvalidOperationException($"Stock Transfer with Reference Number {referenceNumber} not found.");

                // Check if the status change is valid
                if (!IsValidStatusChange(stockTransfer.Status.StatusName.ToLower(), statusName.ToLower()))
                {
                    _logger.LogWarning("Invalid status change from '{OldStatus}' to '{NewStatus}' for Stock Transfer {ReferenceNumber}.", stockTransfer.Status.StatusName, statusName, referenceNumber);
                    throw new InvalidOperationException($"Invalid status change from '{stockTransfer.Status.StatusName}' to '{statusName}' for Stock Transfer {referenceNumber}.");
                }

                if (stockTransfer.Status.StatusName.ToLower() == statusName.ToLower())
                {
                    _logger.LogInformation("Stock Transfer {ReferenceNumber} is already in '{StatusName}' status.", referenceNumber, statusName);
                    return;
                }

                var newStatus = await context.StockTransferStatuses.FirstOrDefaultAsync(s => s.StatusName == statusName);
                if (newStatus == null) throw new InvalidOperationException($"{statusName} status not found in the database.");

                stockTransfer.StatusId = newStatus.StockTransferStatusId;
                await context.SaveChangesAsync();

                switch (statusName.ToLower())
                {
                    case "in transit": await UpdateInventoryForInTransit(stockTransfer); break;
                    case "completed": await UpdateInventoryForComplete(stockTransfer); break;
                    case "cancelled": await RevertInventoryChanges(stockTransfer); break;
                }

                _logger.LogInformation("Stock Transfer {ReferenceNumber} status changed to '{StatusName}'.", referenceNumber, statusName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing stock transfer {ReferenceNumber} to '{StatusName}'.", referenceNumber, statusName);
                throw;
            }
        }

        private async Task UpdateInventoryForInTransit(StockTransfer stockTransfer)
        {
            foreach (var item in stockTransfer.StockTransferItems)
            {
                var sourceInventory = await _inventoryService.GetInventoryByProductIdAndBranchIdAsync(item.ProductId, stockTransfer.SourceBranchId);
                var destinationInventory = await _inventoryService.GetInventoryByProductIdAndBranchIdAsync(item.ProductId, stockTransfer.DestinationBranchId);

                if (sourceInventory != null)
                {
                    sourceInventory.Allocated -= item.TransferQuantity;
                    sourceInventory.OnHandquantity -= item.TransferQuantity;
                    sourceInventory.AvailableQuantity = sourceInventory.OnHandquantity - sourceInventory.Allocated;
                    await _inventoryService.UpdateInventoryAsync(sourceInventory);
                }

                if (destinationInventory != null)
                {
                    destinationInventory.IncomingQuantity += item.TransferQuantity;
                    await _inventoryService.UpdateInventoryAsync(destinationInventory);
                }
            }
        }

        private async Task UpdateInventoryForComplete(StockTransfer stockTransfer)
        {
            foreach (var item in stockTransfer.StockTransferItems)
            {
                var destinationInventory = await _inventoryService.GetInventoryByProductIdAndBranchIdAsync(item.ProductId, stockTransfer.DestinationBranchId);

                if (destinationInventory != null)
                {
                    destinationInventory.IncomingQuantity -= item.TransferQuantity;
                    destinationInventory.OnHandquantity += item.TransferQuantity;
                    destinationInventory.AvailableQuantity = destinationInventory.OnHandquantity - destinationInventory.Allocated;
                    await _inventoryService.UpdateInventoryAsync(destinationInventory);
                }
            }
        }

        private async Task RevertInventoryChanges(StockTransfer stockTransfer)
        {
            foreach (var item in stockTransfer.StockTransferItems)
            {
                var sourceInventory = await _inventoryService.GetInventoryByProductIdAndBranchIdAsync(item.ProductId, stockTransfer.SourceBranchId);
                var destinationInventory = await _inventoryService.GetInventoryByProductIdAndBranchIdAsync(item.ProductId, stockTransfer.DestinationBranchId);

                if (sourceInventory != null)
                {
                    sourceInventory.OnHandquantity += item.TransferQuantity; // Add back to OnHandquantity
                    sourceInventory.AvailableQuantity = sourceInventory.OnHandquantity - sourceInventory.Allocated; // Recalculate AvailableQuantity
                    await _inventoryService.UpdateInventoryAsync(sourceInventory);
                }

                if (destinationInventory != null)
                {
                    destinationInventory.IncomingQuantity -= item.TransferQuantity;
                    await _inventoryService.UpdateInventoryAsync(destinationInventory);
                }
            }
        }

        public async Task DisposeAsync()
        {
            using var context = _dbFactory.CreateDbContext();
            await context.DisposeAsync();
        }


        private bool IsValidStatusChange(string currentStatus, string newStatus)
        {
            switch (currentStatus)
            {
                case "allocated":
                    return newStatus == "in transit";
                case "in transit":
                    return newStatus == "completed" || newStatus == "cancelled";
                default:
                    return false;
            }
        }







    }
}