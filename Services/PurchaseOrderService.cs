using InventiCloud.Entities;
using InventiCloud.Services.Interface;
using InventiCloud.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace InventiCloud.Services
{
    public class PurchaseOrderService(ILogger<PurchaseOrderService> _logger,
        IDbContextFactory<InventiCloud.Data.ApplicationDbContext> DbFactory) : IPurchaseOrderService
    {
        public async Task AddPurchaseOrderAsync(PurchaseOrder purchaseOrder, ICollection<PurchaseOrderItem> purchaseOrderItems)
        {
            if (purchaseOrder == null)
            {
                throw new ArgumentNullException(nameof(purchaseOrder), "Purchase order cannot be null.");
            }

            if (purchaseOrderItems == null || !purchaseOrderItems.Any())
            {
                throw new InvalidOperationException("Purchase order must have at least one item.");
            }

            using var context = DbFactory.CreateDbContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                // Add the purchase order to the context first.
                context.PurchaseOrders.Add(purchaseOrder);
                await context.SaveChangesAsync(); // Generate PurchaseOrderId

                // Generate Reference Number after PurchaseOrderId is generated.
                purchaseOrder.ReferenceNumber = PurchaseOrderGenerateReferenceNumber.GenerateReferenceNumber(purchaseOrder.PurchaseOrderId);
                context.PurchaseOrders.Update(purchaseOrder);
                await context.SaveChangesAsync();

                // Set PurchaseOrderID for items and add them.
                foreach (var item in purchaseOrderItems)
                {
                    item.PurchaseOrderID = purchaseOrder.PurchaseOrderId;
                }

                context.PurchaseOrderItems.AddRange(purchaseOrderItems);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE constraint failed"))
                {
                    throw new InvalidOperationException("A unique constraint violation occurred (e.g., duplicate reference number).", ex);
                }
                _logger.LogError(ex, "Error adding purchase order (DbUpdateException).");
                throw;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error adding purchase order.");
                throw;
            }
        }

        public async Task DeletePurchaseOrderAsync(PurchaseOrder purchaseorder)
        {

            if (purchaseorder == null)
            {
                throw new ArgumentNullException(nameof(purchaseorder), "Purchase order cannot be null.");
            }

            try
            {
                using var context = DbFactory.CreateDbContext();

                // Assuming PurchaseOrderStatus is a navigation property and loaded
                if (purchaseorder.PurchaseOrderStatus?.StatusName != "Draft")
                {
                    throw new InvalidOperationException("Cannot delete purchase order. It is not in 'Draft' status.");
                }

                context.PurchaseOrders.Remove(purchaseorder);
                await context.SaveChangesAsync();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation while deleting purchase order. Purchase order not in draft status. PurchaseOrderId: {PurchaseOrderId}", purchaseorder.PurchaseOrderId);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while deleting purchase order. PurchaseOrderId: {PurchaseOrderId}", purchaseorder.PurchaseOrderId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting purchase order. PurchaseOrderId: {PurchaseOrderId}", purchaseorder.PurchaseOrderId);
                throw;
            }
        }

        public async Task DisposeAsync()
        {
            using var context = DbFactory.CreateDbContext();
            await context.DisposeAsync();
        }

        public async Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrderAsync()
        {
            using var context = DbFactory.CreateDbContext();
            return await context.PurchaseOrders
                .Include(po => po.CreatedBy)
                .Include(po => po.Supplier)
                .Include(po => po.DestinationBranch)
                .Include(po => po.PurchaseOrderStatus)
                .Include(po => po.PurchaseOrderItems)
                .ToListAsync();
        }

        public async Task PurchaseOrderToCancelAsync(PurchaseOrder purchaseOrder)
        {
            // Added a check to ensure that the purchase order is in the "Ordered" state
            // before allowing it to be marked as "Cancelled."
            if (purchaseOrder.PurchaseOrderStatus.StatusName != "Ordered")
            {
                throw new InvalidOperationException("Purchase order must be in 'Ordered' status to be marked as 'Cancelled'.");
            }
            await UpdatePurchaseOrderStatusAsync(purchaseOrder, 4, "Cancelled"); 
        }

        public async Task PurchaseOrderToCompleteAsync(PurchaseOrder purchaseOrder)
        {
            // Added a check to ensure that the purchase order is in the "Ordered" state
            // before allowing it to be marked as "Completed."
            if (purchaseOrder.PurchaseOrderStatus.StatusName != "Ordered")
            {
                throw new InvalidOperationException("Purchase order must be in 'Ordered' status to be marked as 'Completed'.");
            }
            await UpdatePurchaseOrderStatusAsync(purchaseOrder, 3, "Completed"); 
        }

        public async Task PurchaseOrderToOrderedAsync(PurchaseOrder purchaseOrder)
        {
            // Added a check to ensure that the purchase order is in the "Draft" state
            // before allowing it to be marked as "Ordered."
            if (purchaseOrder.PurchaseOrderStatus.StatusName != "Draft") 
            {
                throw new InvalidOperationException("Purchase order must be in 'Draft' status to be marked as 'Ordered'.");
            }

            await UpdatePurchaseOrderStatusAsync(purchaseOrder, 2, "Ordered");
        }

        public async Task UpdatePurchaseOrderStatusAsync(PurchaseOrder purchaseOrder, int statusId, string statusName)
        {
            if (purchaseOrder == null)
            {
                throw new ArgumentNullException(nameof(purchaseOrder), "Purchase order cannot be null.");
            }

            try
            {
                using var context = DbFactory.CreateDbContext();

                purchaseOrder.StatusId = statusId;

                context.PurchaseOrders.Update(purchaseOrder);
                await context.SaveChangesAsync();

                _logger.LogInformation("Purchase order {PurchaseOrderId} status updated to '{StatusName}'.", purchaseOrder.PurchaseOrderId, statusName);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating purchase order {PurchaseOrderId} to '{StatusName}'.", purchaseOrder.PurchaseOrderId, statusName);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating purchase order {PurchaseOrderId} to '{StatusName}'.", purchaseOrder.PurchaseOrderId, statusName);
                throw;
            }
        }


        public async Task UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder == null)
            {
                throw new ArgumentNullException(nameof(purchaseOrder), "Purchase order cannot be null.");
            }

            try
            {
                using var context = DbFactory.CreateDbContext();

                // Load the existing purchase order from the database to check its status.
                var existingPurchaseOrder = await context.PurchaseOrders.FindAsync(purchaseOrder.PurchaseOrderId);

                if (existingPurchaseOrder == null)
                {
                    throw new InvalidOperationException($"Purchase order with ID {purchaseOrder.PurchaseOrderId} not found.");
                }

                if (existingPurchaseOrder.PurchaseOrderStatus.StatusName != "Draft") 
                {
                    throw new InvalidOperationException("Cannot update purchase order. It is not in 'Draft' status.");
                }

                // Update the existing purchase order with the new values.
                context.Entry(existingPurchaseOrder).CurrentValues.SetValues(purchaseOrder);

                await context.SaveChangesAsync();

                _logger.LogInformation("Purchase order {PurchaseOrderId} updated.", purchaseOrder.PurchaseOrderId);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation while updating purchase order {PurchaseOrderId}.", purchaseOrder.PurchaseOrderId);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating purchase order {PurchaseOrderId}.", purchaseOrder.PurchaseOrderId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating purchase order {PurchaseOrderId}.", purchaseOrder.PurchaseOrderId);
                throw;
            }
        }

    }
}
