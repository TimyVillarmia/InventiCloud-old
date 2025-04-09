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
                .Include(st => st.RequestedBy)
                .Include(st => st.ApprovedBy)
                .Include(st => st.SourceBranch)
                .Include(st => st.DestinationBranch)
                .Include(st => st.Status)
                .Include(st => st.StockTransferItems)
                    .ThenInclude(st => st.Product)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<StockTransfer>> GetStockTransfersRequestedByUserAsync(string requestedByUserId)
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.StockTransfers
                .Include(st => st.SourceBranch)
                .Include(st => st.DestinationBranch)
                .Include(st => st.Status)
                .Where(st => st.RequestedById == requestedByUserId)
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<IEnumerable<StockTransfer>> GetStockTransfersForApprovalAsync(string approvedByUserId)
        {
            using var context = _dbFactory.CreateDbContext();


            return await context.StockTransfers
                .Include(st => st.SourceBranch)
                .Include(st => st.DestinationBranch)
                .Include(st => st.Status)
                .Where(st => st.ApprovedById == approvedByUserId)
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
                stockTransfer.StatusId = 1; // Requested
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

                    await _inventoryService.UpdateInventoryAsync(sourceInventory);
                }

                context.StockTransferItems.AddRange(stockTransferItems);
                await context.SaveChangesAsync();


                _navigationManager.NavigateTo($"/inventory/stock-transfers/requests/{stockTransfer.ReferenceNumber}");
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

            if (string.IsNullOrEmpty(referenceNumber))
            {
                return null;
            }

            using var context = _dbFactory.CreateDbContext();

            try
            {
                var stockTransfer = await context.StockTransfers
                    .Include(st => st.RequestedBy)
                    .Include(st => st.ApprovedBy)
                    .Include(st => st.SourceBranch)
                    .Include(st => st.DestinationBranch)
                    .Include(st => st.Status)
                    .Include(st => st.StockTransferItems)
                        .ThenInclude(st => st.Product)
                    .FirstAsync(st => st.ReferenceNumber == referenceNumber);

                return stockTransfer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Stock Transfer by Reference Number '{ReferenceNumber}'.", referenceNumber);
                // Consider re-throwing a more specific exception or returning null based on your error handling strategy
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
            if (string.IsNullOrEmpty(referenceNumber))
                throw new ArgumentNullException(nameof(referenceNumber), "Reference number cannot be null or empty.");

            using var context = _dbFactory.CreateDbContext();

            try
            {
                var stockTransfer = await context.StockTransfers
                    .Include(st => st.Status)
                    .Include(st => st.StockTransferItems)
                    .FirstOrDefaultAsync(st => st.ReferenceNumber == referenceNumber);

                if (stockTransfer == null)
                    throw new InvalidOperationException($"Stock Transfer with Reference Number {referenceNumber} not found.");

                if (stockTransfer.Status.StatusName != "Requested")
                    throw new InvalidOperationException($"Stock Transfer with Reference Number {referenceNumber} is not in 'Requested' status and cannot be deleted through this method.");


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

        public async Task StockTransferToApprovedAsync(string referenceNumber) => await UpdateStockTransferStatusAndDates(referenceNumber, "Approved", DateTime.Now, null, null);

        public async Task StockTransferToCompletedAsync(string referenceNumber) => await UpdateStockTransferStatusAndDates(referenceNumber, "Completed", null, DateTime.Now, null);

        public async Task StockTransferToRejectedAsync(string referenceNumber) => await UpdateStockTransferStatusAndDates(referenceNumber, "Rejected", null, null, DateTime.Now);

        private async Task UpdateStockTransferStatusAndDates(string referenceNumber, string statusName, DateTime? dateApproved, DateTime? dateCompleted, DateTime? rejectedDate)
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

                // Update dates based on the new status
                stockTransfer.DateApproved = dateApproved ?? stockTransfer.DateApproved;
                stockTransfer.DateCompleted = dateCompleted ?? stockTransfer.DateCompleted;
                stockTransfer.RejectedDate = rejectedDate ?? stockTransfer.RejectedDate;

                await context.SaveChangesAsync();

                switch (statusName.ToLower())
                {
                    case "approved": await UpdateInventoryForApproved(stockTransfer); break;
                    case "completed": await UpdateInventoryForComplete(stockTransfer); break;
                    case "rejected": // No specific inventory update for rejection in this example
                        _logger.LogInformation("Stock Transfer {ReferenceNumber} rejected.", referenceNumber);
                        break;
                }

                _logger.LogInformation("Stock Transfer {ReferenceNumber} status changed to '{StatusName}'.", referenceNumber, statusName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing stock transfer {ReferenceNumber} to '{StatusName}'.", referenceNumber, statusName);
                throw;
            }
        }

        private async Task UpdateInventoryForApproved(StockTransfer stockTransfer)
        {
            foreach (var item in stockTransfer.StockTransferItems)
            {
                // Get inventory for source and destination branches
                var sourceInventory = await _inventoryService.GetInventoryByProductIdAndBranchIdAsync(item.ProductId, stockTransfer.SourceBranchId);
                var destinationInventory = await _inventoryService.GetInventoryByProductIdAndBranchIdAsync(item.ProductId, stockTransfer.DestinationBranchId);

                // Validate source inventory exists
                if (sourceInventory == null)
                {
                    throw new InvalidOperationException($"Source inventory not found for ProductId: {item.ProductId} in BranchId: {stockTransfer.SourceBranchId}.");
                }

                // Validate sufficient available quantity in source
                if (sourceInventory.AvailableQuantity < item.TransferQuantity)
                {
                    throw new InvalidOperationException($"Insufficient available quantity in source branch (ProductId: {item.ProductId}" +
                                                        $"Available: {sourceInventory.AvailableQuantity}, Requested: {item.TransferQuantity}.");
                }

                // Update source inventory
                sourceInventory.Allocated += item.TransferQuantity;
                sourceInventory.AvailableQuantity = sourceInventory.OnHandquantity - sourceInventory.Allocated;

                await _inventoryService.UpdateInventoryAsync(sourceInventory);

                // Update destination inventory (it should exist, but handle gracefully if not)
                if (destinationInventory != null)
                {
                    destinationInventory.IncomingQuantity += item.TransferQuantity;
                    await _inventoryService.UpdateInventoryAsync(destinationInventory);
                }
                else
                {
                    // Consider logging a warning or creating the inventory if business rules allow
                    _logger.LogWarning($"Destination inventory not found for ProductId: {item.ProductId} in BranchId: {stockTransfer.DestinationBranchId}. Incoming quantity not updated.");
                }
            }

        }

        private async Task UpdateInventoryForComplete(StockTransfer stockTransfer)
        {
            foreach (var item in stockTransfer.StockTransferItems)
            {
                // approver
                var sourceInventory = await _inventoryService.GetInventoryByProductIdAndBranchIdAsync(item.ProductId, stockTransfer.SourceBranchId);
                // requester
                var destinationInventory = await _inventoryService.GetInventoryByProductIdAndBranchIdAsync(item.ProductId, stockTransfer.DestinationBranchId);

                if (sourceInventory != null)
                {

                    // after completed
                    sourceInventory.Allocated -= item.TransferQuantity; // decrease the allocated since transfer is already at the source/requester branch
                    //recalculate
                    sourceInventory.OnHandquantity = sourceInventory.AvailableQuantity + sourceInventory.Allocated;
                    sourceInventory.AvailableQuantity = sourceInventory.OnHandquantity - sourceInventory.Allocated;

                    await _inventoryService.UpdateInventoryAsync(sourceInventory);
                }


                if (destinationInventory != null)
                {
                    destinationInventory.IncomingQuantity -= item.TransferQuantity;
                    destinationInventory.OnHandquantity += item.TransferQuantity;
                    destinationInventory.AvailableQuantity = destinationInventory.OnHandquantity - destinationInventory.Allocated;
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
                case "requested":
                    return newStatus == "approved";
                case "approved":
                    return newStatus == "completed" || newStatus == "rejected";
                default:
                    return false;
            }
        }







    }
}