using InventiCloud.Entities;
using InventiCloud.Services.Interface;
using InventiCloud.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace InventiCloud.Services
{
    public class PurchaseOrderService(ILogger<PurchaseOrderService> _logger, IDbContextFactory<InventiCloud.Data.ApplicationDbContext> DbFactory) : IPurchaseOrderService
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

        public Task PurchaseOrderToCancelAsync(PurchaseOrder purchaseorder)
        {
            throw new NotImplementedException();
        }

        public Task PurchaseOrderToCompleteAsync(PurchaseOrder purchaseorder)
        {
            throw new NotImplementedException();
        }



        public Task PurchaseOrderToOrderedAsync(PurchaseOrder purchaseorder)
        {
            throw new NotImplementedException();
        }


        public Task UpdatePurchaseOrderAsync(PurchaseOrder purchaseorder)
        {
            throw new NotImplementedException();
        }
    }
}
