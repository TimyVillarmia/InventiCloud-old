using InventiCloud.Entities;
using InventiCloud.Services.Interface;
using InventiCloud.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Services
{
    public class StockTransferService(ILogger<StockTransferService> _logger,
        NavigationManager NavigationManager,
        IDbContextFactory<InventiCloud.Data.ApplicationDbContext> DbFactory) : IStockTransferService
    {
        public async Task AddStockTransferAsync(StockTransfer stockTransfer, ICollection<StockTransferItem> stockTransferItems)
        {
            if (stockTransfer == null)
            {
                throw new ArgumentNullException(nameof(stockTransfer), "Stock Transfer cannot be null.");
            }

            if (stockTransferItems == null || !stockTransferItems.Any())
            {
                throw new InvalidOperationException("Stock Transfer must have at least one item.");
            }

            using var context = DbFactory.CreateDbContext();

            try
            {
                // Update status to draft.
                stockTransfer.StatusId = 1;
                // Add the purchase order.
                context.StockTransfers.Add(stockTransfer);
                await context.SaveChangesAsync(); // Generate StockTransferId

                // Generate and set the reference number.
                stockTransfer.ReferenceNumber = ReferenceNumberGenerator.GenerateStockTransferReference(stockTransfer.StockTransferId);

                // Update the purchase order 
                context.StockTransfers.Update(stockTransfer);


                // Save all changes.
                await context.SaveChangesAsync();


                // Set PurchaseOrderId for items.
                foreach (var item in stockTransferItems)
                {
                    item.StockTransferId = stockTransfer.StockTransferId;
                }

                context.StockTransferItems.AddRange(stockTransferItems);

                // Save all changes.
                await context.SaveChangesAsync();


                NavigationManager.NavigateTo($"/inventory/stock-transfers/{stockTransfer.ReferenceNumber}");


                _logger.LogInformation("Stock Transfer {StockTransferId} added successfully.", stockTransfer.StockTransferId);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE constraint failed"))
                {
                    _logger.LogError(ex, "Unique constraint violation adding purchase order {StockTransferId}.", stockTransfer.StockTransferId);
                    throw new InvalidOperationException("A unique constraint violation occurred (e.g., duplicate reference number).", ex);
                }
                _logger.LogError(ex, "Database error adding stock transfer {StockTransferId}.", stockTransfer.StockTransferId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding stock transfer {StockTransferId}.", stockTransfer.StockTransferId);
                throw;
            }
        }

    
        public async Task DisposeAsync()
        {
            using var context = DbFactory.CreateDbContext();
            await context.DisposeAsync();
        }

        public async Task<IEnumerable<StockTransfer>> GetAllStockTransferAsync()
        {
            using var context = DbFactory.CreateDbContext();
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

     

        public async Task<StockTransfer> GetStockTransferByReferenceNumberAsync(string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
                throw new ArgumentNullException(nameof(referenceNumber), "Reference Number cannot be null or empty.");
            }

            using var context = DbFactory.CreateDbContext();

            try
            {


                var stockTransfer = await context.StockTransfers
                    .Include(st => st.CreatedBy)
                    .Include(st => st.SourceBranch)
                    .Include(st => st.DestinationBranch)
                    .Include(st => st.Status)
                    .Include(st => st.StockTransferItems)
                        .ThenInclude(st => st.Product)
                    .FirstAsync(po => po.ReferenceNumber == referenceNumber);


                return stockTransfer;
            }
            catch (Exception ex)
            {
                // Log the exception (use your logging framework)
                // _logger.LogError(ex, "Error retrieving purchase order by reference number.");
                Console.WriteLine($"Error retrieving purchase order by reference number: {ex.Message}");
                return null; // Or re-throw the exception if appropriate
            }
        }

      
   
        public async Task UpdateStockTransferStatusAsync(StockTransfer stockTransfer, int statusId, string statusName)
        {
            if (stockTransfer == null)
            {
                throw new ArgumentNullException(nameof(stockTransfer), "Stock Transfer cannot be null.");
            }

            try
            {
                using var context = DbFactory.CreateDbContext();
                var existingStockTransfer = context.StockTransfers.Where(st => st.ReferenceNumber == stockTransfer.ReferenceNumber).FirstOrDefault();


                context.Attach(existingStockTransfer);
                stockTransfer.StatusId = statusId; // change status
                context.Entry(existingStockTransfer).CurrentValues.SetValues(stockTransfer);
                await context.SaveChangesAsync();

                _logger.LogInformation("Stock Transfer {StockTransferId} status updated to '{StatusName}'.", stockTransfer.StockTransferId, statusName);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating stock transfer {StockTransferId} to '{StatusName}'.", stockTransfer.StockTransferId, statusName);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating stock transfer {StockTransferId} to '{StatusName}'.", stockTransfer.StockTransferId, statusName);
                throw;
            }
        }

        public async Task UpdateStockTransferItemsAsync(string referenceNumber, ICollection<StockTransferItem> updatedStockTransferItems)
        {


            using var context = DbFactory.CreateDbContext();

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
                    }
                    else
                    {
                        // Modification
                        context.Entry(existingStockTransferItem).CurrentValues.SetValues(updatedItem);
                    }
                }

                await context.SaveChangesAsync();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Stock Transfer not found for reference number: {ReferenceNumber}", referenceNumber);
                throw;
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
            {
                throw new ArgumentNullException(nameof(referenceNumber), "Reference number cannot be null or empty.");
            }

            using var context = DbFactory.CreateDbContext();

            try
            {
                var stockTransfer = await context.StockTransfers
                    .Include(st => st.Status) // Include the Status navigation property
                    .FirstOrDefaultAsync(st => st.ReferenceNumber == referenceNumber);

                if (stockTransfer == null)
                {
                    throw new InvalidOperationException($"Stock Transfer with Reference Number {referenceNumber} not found.");
                }

                if (stockTransfer.Status.StatusName != "Allocated") // Check the status name
                {
                    throw new InvalidOperationException($"Stock Transfer with Reference Number {referenceNumber} is not in 'Allocated' status and cannot be deleted.");
                }

                context.StockTransfers.Remove(stockTransfer);
                await context.SaveChangesAsync();

                _logger.LogInformation("Stock Transfer {ReferenceNumber} deleted.", referenceNumber);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Stock Transfer not found or not in 'Allocated' status for deletion {ReferenceNumber}.", referenceNumber);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error deleting stock transfer {ReferenceNumber}.", referenceNumber);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting stock transfer {ReferenceNumber}.", referenceNumber);
                throw;
            }
        }

        public async Task UpdateStockTransferAsync(string referenceNumber, StockTransfer newStockTransfer)
        {
            if (newStockTransfer == null)
            {
                throw new ArgumentNullException(nameof(newStockTransfer), "Stock Transfer cannot be null.");
            }

            using var context = DbFactory.CreateDbContext();

            try
            {
                var existingStockTransfer = await context.StockTransfers
                    .Include(st => st.Status)
                    .FirstOrDefaultAsync(st => st.ReferenceNumber == referenceNumber);

                if (existingStockTransfer == null)
                {
                    throw new InvalidOperationException($"Stock Transfer with Reference Number {referenceNumber} not found.");
                }

                if (existingStockTransfer.Status.StatusName != "Allocated")
                {
                    throw new InvalidOperationException("Cannot update Stock Transfer. It is not in 'Allocated' status.");
                }

                // Update the existing stock transfer with the new values.
                existingStockTransfer.SourceBranchId = newStockTransfer.SourceBranchId;
                existingStockTransfer.DestinationBranchId = newStockTransfer.DestinationBranchId;

                context.StockTransfers.Update(existingStockTransfer);
                await context.SaveChangesAsync();

                _logger.LogInformation("Stock Transfer {ReferenceNumber} updated.", referenceNumber);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation while updating stock transfer {ReferenceNumber}.", referenceNumber);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating stock transfer {ReferenceNumber}.", referenceNumber);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating stock transfer {ReferenceNumber}.", referenceNumber);
                throw;
            }
        }

        public async Task StockTransferToAllocatedAsync(string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
                throw new ArgumentNullException(nameof(referenceNumber), "Reference number cannot be null or empty.");
            }

            using var context = DbFactory.CreateDbContext();

            try
            {
                var stockTransfer = await context.StockTransfers
                    .Include(st => st.Status)
                    .FirstOrDefaultAsync(st => st.ReferenceNumber == referenceNumber);

                if (stockTransfer == null)
                {
                    throw new InvalidOperationException($"Stock Transfer with Reference Number {referenceNumber} not found.");
                }

                if (stockTransfer.Status.StatusName == "Allocated")
                {
                    // Already allocated, no need to change
                    _logger.LogInformation("Stock Transfer {ReferenceNumber} is already in 'Allocated' status.", referenceNumber);
                    return;
                }

                // Find the "Allocated" status
                var allocatedStatus = await context.StockTransferStatuses
                    .FirstOrDefaultAsync(s => s.StatusName == "Allocated");

                if (allocatedStatus == null)
                {
                    throw new InvalidOperationException("Allocated status not found in the database.");
                }

                stockTransfer.StatusId = allocatedStatus.StockTransferStatusId;
                await context.SaveChangesAsync();

                _logger.LogInformation("Stock Transfer {ReferenceNumber} status changed to 'Allocated'.", referenceNumber);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Stock Transfer not found or 'Allocated' status not found for {ReferenceNumber}.", referenceNumber);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error changing stock transfer {ReferenceNumber} to 'Allocated'.", referenceNumber);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing stock transfer {ReferenceNumber} to 'Allocated'.", referenceNumber);
                throw;
            }
        }

        public async Task StockTransferToInTransitAsync(string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
                throw new ArgumentNullException(nameof(referenceNumber), "Reference number cannot be null or empty.");
            }

            using var context = DbFactory.CreateDbContext();

            try
            {
                var stockTransfer = await context.StockTransfers
                    .Include(st => st.Status)
                    .FirstOrDefaultAsync(st => st.ReferenceNumber == referenceNumber);

                if (stockTransfer == null)
                {
                    throw new InvalidOperationException($"Stock Transfer with Reference Number {referenceNumber} not found.");
                }

                if (stockTransfer.Status.StatusName != "Allocated")
                {
                    throw new InvalidOperationException($"Stock Transfer with Reference Number {referenceNumber} is not in 'Allocated' status and cannot be moved to 'In Transit'.");
                }

                // Find the "In Transit" status
                var inTransitStatus = await context.StockTransferStatuses
                    .FirstOrDefaultAsync(s => s.StatusName == "In Transit");

                if (inTransitStatus == null)
                {
                    throw new InvalidOperationException("In Transit status not found in the database.");
                }

                stockTransfer.StatusId = inTransitStatus.StockTransferStatusId;
                await context.SaveChangesAsync();

                _logger.LogInformation("Stock Transfer {ReferenceNumber} status changed to 'In Transit'.", referenceNumber);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Stock Transfer not found or not in 'Allocated' status or 'In Transit' status not found for {ReferenceNumber}.", referenceNumber);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error changing stock transfer {ReferenceNumber} to 'In Transit'.", referenceNumber);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing stock transfer {ReferenceNumber} to 'In Transit'.", referenceNumber);
                throw;
            }
        }

        public async Task StockTransferToCompleteAsync(string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
                throw new ArgumentNullException(nameof(referenceNumber), "Reference number cannot be null or empty.");
            }

            using var context = DbFactory.CreateDbContext();

            try
            {
                var stockTransfer = await context.StockTransfers
                    .Include(st => st.Status)
                    .FirstOrDefaultAsync(st => st.ReferenceNumber == referenceNumber);

                if (stockTransfer == null)
                {
                    throw new InvalidOperationException($"Stock Transfer with Reference Number {referenceNumber} not found.");
                }

                if (stockTransfer.Status.StatusName != "In Transit")
                {
                    throw new InvalidOperationException($"Stock Transfer with Reference Number {referenceNumber} is not in 'In Transit' status and cannot be moved to 'Completed'.");
                }

                // Find the "Completed" status
                var completedStatus = await context.StockTransferStatuses
                    .FirstOrDefaultAsync(s => s.StatusName == "Completed");

                if (completedStatus == null)
                {
                    throw new InvalidOperationException("Completed status not found in the database.");
                }

                stockTransfer.StatusId = completedStatus.StockTransferStatusId;
                await context.SaveChangesAsync();

                _logger.LogInformation("Stock Transfer {ReferenceNumber} status changed to 'Completed'.", referenceNumber);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Stock Transfer not found or not in 'In Transit' status or 'Completed' status not found for {ReferenceNumber}.", referenceNumber);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error changing stock transfer {ReferenceNumber} to 'Completed'.", referenceNumber);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing stock transfer {ReferenceNumber} to 'Completed'.", referenceNumber);
                throw;
            }
        }

        public async Task StockTransferCancelledAsync(string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
                throw new ArgumentNullException(nameof(referenceNumber), "Reference number cannot be null or empty.");
            }

            using var context = DbFactory.CreateDbContext();

            try
            {
                var stockTransfer = await context.StockTransfers
                    .Include(st => st.Status)
                    .FirstOrDefaultAsync(st => st.ReferenceNumber == referenceNumber);

                if (stockTransfer == null)
                {
                    throw new InvalidOperationException($"Stock Transfer with Reference Number {referenceNumber} not found.");
                }

                if (stockTransfer.Status.StatusName != "In Transit")
                {
                    throw new InvalidOperationException($"Stock Transfer with Reference Number {referenceNumber} is not in 'In Transit' status and cannot be cancelled.");
                }

                // Find the "Cancelled" status
                var cancelledStatus = await context.StockTransferStatuses
                    .FirstOrDefaultAsync(s => s.StatusName == "Cancelled");

                if (cancelledStatus == null)
                {
                    throw new InvalidOperationException("Cancelled status not found in the database.");
                }

                stockTransfer.StatusId = cancelledStatus.StockTransferStatusId;
                await context.SaveChangesAsync();

                _logger.LogInformation("Stock Transfer {ReferenceNumber} status changed to 'Cancelled'.", referenceNumber);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Stock Transfer not found or not in 'In Transit' status or 'Cancelled' status not found for {ReferenceNumber}.", referenceNumber);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error changing stock transfer {ReferenceNumber} to 'Cancelled'.", referenceNumber);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing stock transfer {ReferenceNumber} to 'Cancelled'.", referenceNumber);
                throw;
            }
        }

        public async Task<IEnumerable<StockTransferItem>> GetAllStockTransferItemByIdAsync(int? stockTransferId)
        {
            if (!stockTransferId.HasValue)
            {
                throw new ArgumentNullException(nameof(stockTransferId), "Stock Transfer ID cannot be null.");
            }

            using var context = DbFactory.CreateDbContext();

            try
            {
                var stockTransferItems = await context.StockTransferItems
                    .Where(sti => sti.StockTransferId == stockTransferId)
                    .Include(sti => sti.Product) // Include Product information if needed
                    .ToListAsync();

                if (stockTransferItems == null || !stockTransferItems.Any())
                {
                    _logger.LogWarning("No Stock Transfer Items found for Stock Transfer ID: {StockTransferId}.", stockTransferId);
                    return new List<StockTransferItem>(); // Return an empty list
                }

                return stockTransferItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Stock Transfer Items for Stock Transfer ID: {StockTransferId}.", stockTransferId);
                throw;
            }
        }

        public async Task<StockTransfer> GetStockTransferByIdAsync(int? stockTransferId)
        {
            if (!stockTransferId.HasValue)
            {
                throw new ArgumentNullException(nameof(stockTransferId), "Stock Transfer ID cannot be null.");
            }

            using var context = DbFactory.CreateDbContext();

            try
            {
                var stockTransfer = await context.StockTransfers
                    .Include(st => st.Status) // Include related Status data
                    .Include(st => st.SourceBranch) // Include related SourceBranch data
                    .Include(st => st.DestinationBranch) // Include related DestinationBranch data
                    .Include(st => st.StockTransferItems) // Include related StockTransferItems data
                        .ThenInclude(sti => sti.Product) // Include Product data within StockTransferItems
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
    }
    
}
