using InventiCloud.Entities;
using InventiCloud.Services.Interface;
using InventiCloud.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Services
{
    public class StockTransferService(ILogger<PurchaseOrderService> _logger,
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

        public Task<IEnumerable<StockTransferItem>> GetAllStockTransferItemByIdAsync(int? stockTransferId)
        {
            throw new NotImplementedException();
        }

        public Task<StockTransfer> GetStockTransferByIdAsync(int? stockTransferId)
        {
            throw new NotImplementedException();
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

        public async Task UpdateStockTransferStatusAsync(StockTransfer stockTransfer, int statusId, string statusName)
        {
            if (stockTransfer == null)
            {
                throw new ArgumentNullException(nameof(stockTransfer), "Stock Transfer cannot be null.");
            }

            try
            {
                using var context = DbFactory.CreateDbContext();
                var existingPurchaseOrder = await GetStockTransferByReferenceNumberAsync(stockTransfer.ReferenceNumber);

                context.Attach(existingPurchaseOrder);
                stockTransfer.StatusId = statusId; // change status
                context.Entry(existingPurchaseOrder).CurrentValues.SetValues(stockTransfer);
                await context.SaveChangesAsync();

                _logger.LogInformation("Stock Transfer {PurchaseOrderId} status updated to '{StatusName}'.", stockTransfer.StockTransferId, statusName);
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
    }
}
