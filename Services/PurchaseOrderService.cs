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

        public Task DeletePurchaseOrderAsync(PurchaseOrder purchaseorder)
        {
            throw new NotImplementedException();
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

        public Task SetPurchaseOrderStatusAsync(PurchaseOrder purchaseorder, string statusName)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePurchaseOrderAsync(PurchaseOrder purchaseorder)
        {
            throw new NotImplementedException();
        }
    }
}
