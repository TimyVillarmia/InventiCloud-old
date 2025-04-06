using InventiCloud.Entities;
using InventiCloud.Services.Interface;
using InventiCloud.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;

namespace InventiCloud.Services
{
    public class PurchaseOrderService(ILogger<PurchaseOrderService> _logger,
        IInventoryService inventoryService,
        NavigationManager NavigationManager,
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

            try
            {

                purchaseOrder.TotalAmount = purchaseOrderItems.Sum(item => item.SubTotal);

                // Add the purchase order.
                context.PurchaseOrders.Add(purchaseOrder);
                await context.SaveChangesAsync(); // Generate PurchaseOrderId

                // Generate and set the reference number.
                purchaseOrder.ReferenceNumber = ReferenceNumberGenerator.GeneratePurchaseOrderReference(purchaseOrder.PurchaseOrderId);

                // Update the purchase order 
                context.PurchaseOrders.Update(purchaseOrder);

                // Update status to draft.
                await UpdatePurchaseOrderStatusAsync(purchaseOrder, 1, "Draft");

                // Save all changes.
                await context.SaveChangesAsync();


                // Set PurchaseOrderId for items.
                foreach (var item in purchaseOrderItems)
                {
                    item.PurchaseOrderID = purchaseOrder.PurchaseOrderId;
                }

                context.PurchaseOrderItems.AddRange(purchaseOrderItems);

                // Save all changes.
                await context.SaveChangesAsync();


                NavigationManager.NavigateTo($"/purchase/orders/{purchaseOrder.ReferenceNumber}");


                _logger.LogInformation("Purchase order {PurchaseOrderId} added successfully.", purchaseOrder.PurchaseOrderId);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE constraint failed"))
                {
                    _logger.LogError(ex, "Unique constraint violation adding purchase order {PurchaseOrderId}.", purchaseOrder.PurchaseOrderId);
                    throw new InvalidOperationException("A unique constraint violation occurred (e.g., duplicate reference number).", ex);
                }
                _logger.LogError(ex, "Database error adding purchase order {PurchaseOrderId}.", purchaseOrder.PurchaseOrderId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding purchase order {PurchaseOrderId}.", purchaseOrder.PurchaseOrderId);
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
                .ToListAsync();
        }

        public async Task PurchaseOrderToCancelAsync(PurchaseOrder purchaseOrder)
        {
            // Added a check to ensure that the purchase order is in the "Ordered" state
            // before allowing it to be marked as "Cancelled."
            if (purchaseOrder.PurchaseOrderStatus.StatusName.ToLower() != "ordered")
            {
                throw new InvalidOperationException("Purchase order must be in 'Ordered' status to be marked as 'Cancelled'.");
            }


            try
            {
                await UpdatePurchaseOrderStatusAsync(purchaseOrder, 4, "Cancelled");

                // Subtract the PurchaseOrderItem quantity from IncomingQuantity
                var purchaseOrderItems = await GetAllPurchaseOrderItemByIdAsync(purchaseOrder.PurchaseOrderId);

                foreach (var item in purchaseOrderItems)
                {
                    var inventory = await inventoryService.GetInventoryByProductIdAndBranchIdAsync(item.ProductID, purchaseOrder.DestinationBranchId);

                    if (inventory != null)
                    {
                        inventory.IncomingQuantity -= item.Quantity; // Subtract Quantity
                        if (inventory.IncomingQuantity < 0)
                        {
                            inventory.IncomingQuantity = 0; // ensure incoming quantity does not become negative.
                        }
                        await inventoryService.UpdateInventoryAsync(inventory);
                    }
                    else
                    {
                        // Handle the case where the product has no inventory in the destination branch.
                        // You might want to create a new inventory entry or log an error.
                        _logger.LogInformation("Warning: Product {ProductId} has no inventory in branch {BranchId}.", item.ProductID, purchaseOrder.DestinationBranchId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adjusting incoming quantity.");
                throw; // Re-throw the exception to propagate it.
            }
        }

        public async Task PurchaseOrderToCompleteAsync(PurchaseOrder purchaseOrder)
        {
            // Added a check to ensure that the purchase order is in the "Ordered" state
            // before allowing it to be marked as "Completed."
            if (purchaseOrder.PurchaseOrderStatus.StatusName.ToLower() != "ordered")
            {
                throw new InvalidOperationException("Purchase order must be in 'Ordered' status to be marked as 'Completed'.");
            }
            try
            {
                await UpdatePurchaseOrderStatusAsync(purchaseOrder, 3, "Completed");

                // Update OnHandQuantity and reset IncomingQuantity for each PurchaseOrderItem's product
                var purchaseOrderItems = await GetAllPurchaseOrderItemByIdAsync(purchaseOrder.PurchaseOrderId);

                foreach (var item in purchaseOrderItems)
                {
                    var inventory = await inventoryService.GetInventoryByProductIdAndBranchIdAsync(item.ProductID, purchaseOrder.DestinationBranchId);

                    if (inventory != null)
                    {
                        inventory.OnHandquantity += item.Quantity; // Increase OnHandQuantity
                        inventory.IncomingQuantity -= item.Quantity; // Decrease Quantity
                        inventory.AvailableQuantity = inventory.OnHandquantity - inventory.Allocated;
                        await inventoryService.UpdateInventoryAsync(inventory);
                    }
                    else
                    {
                        // Handle the case where the product has no inventory in the destination branch.
                        // You might want to create a new inventory entry or log an error.
                        _logger.LogInformation("Warning: Product {ProductId} has no inventory in branch {BranchId}.", item.ProductID, purchaseOrder.DestinationBranchId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating inventory on-hand and incoming quantity.");
                throw; // Re-throw the exception to propagate it.
            }
        }

        public async Task PurchaseOrderToOrderedAsync(PurchaseOrder purchaseOrder)
        {
            // Added a check to ensure that the purchase order is in the "Draft" state
            // before allowing it to be marked as "Ordered."
            if (purchaseOrder.PurchaseOrderStatus.StatusName.ToLower() != "draft")
            {
                throw new InvalidOperationException("Purchase order must be in 'Draft' status to be marked as 'Ordered'.");
            }

            try
            {
                await UpdatePurchaseOrderStatusAsync(purchaseOrder, 2, "Ordered");

                // Update IncomingQuantity for each PurchaseOrderItem's product
                var purchaseOrderItems = await GetAllPurchaseOrderItemByIdAsync(purchaseOrder.PurchaseOrderId);

                foreach (var item in purchaseOrderItems)
                {
                    var inventory = await inventoryService.GetInventoryByProductIdAndBranchIdAsync(item.ProductID, purchaseOrder.DestinationBranchId);

                    if (inventory != null)
                    {
                        inventory.IncomingQuantity += item.Quantity; // Increase IncomingQuantity
                        await inventoryService.UpdateInventoryAsync(inventory);
                    }
                    else
                    {
                        // Handle the case where the product has no inventory in the destination branch.
                        // You might want to create a new inventory entry or log an error.
                        Console.WriteLine($"Warning: Product {item.ProductID} has no inventory in branch {purchaseOrder.DestinationBranchId}.");
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating incoming quantity: {ex.Message}");
                throw; // Re-throw the exception to propagate it.
            }
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
                var existingPurchaseOrder = await GetPurchaseOrderByIdAsync(purchaseOrder.PurchaseOrderId);

                context.Attach(existingPurchaseOrder);
                purchaseOrder.StatusId = statusId; // change status
                context.Entry(existingPurchaseOrder).CurrentValues.SetValues(purchaseOrder);
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
                var existingPurchaseOrder = await GetPurchaseOrderByIdAsync(purchaseOrder.PurchaseOrderId);

                if (existingPurchaseOrder == null)
                {
                    throw new InvalidOperationException($"Purchase order with ID {purchaseOrder.PurchaseOrderId} not found.");
                }

                if (existingPurchaseOrder.PurchaseOrderStatus.StatusName != "Draft")
                {
                    throw new InvalidOperationException("Cannot update purchase order. It is not in 'Draft' status.");
                }

                // Update the existing purchase order with the new values.
                context.PurchaseOrders.Update(purchaseOrder);
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

        public async Task<PurchaseOrder> GetPurchaseOrderByIdAsync(int? purchaeOrderId)
        {
            if (purchaeOrderId == null)
            {
                throw new ArgumentNullException(nameof(purchaeOrderId), "Purchase Order Id cannot be null or empty.");
            }


            try
            {
                using var context = DbFactory.CreateDbContext();

                var purchaseOrder = await context.PurchaseOrders
                    .Include(po => po.CreatedBy)
                    .Include(po => po.Supplier)
                    .Include(po => po.DestinationBranch)
                    .Include(po => po.PurchaseOrderStatus)
                    .Include(po => po.PurchaseOrderItems)
                    .FirstAsync(po => po.PurchaseOrderId == purchaeOrderId);

                return purchaseOrder;
            }
            catch (Exception ex)
            {
                // Log the exception (use your logging framework)
                // _logger.LogError(ex, "Error retrieving purchase order by reference number.");
                Console.WriteLine($"Error retrieving purchase order by reference number: {ex.Message}");
                return null; // Or re-throw the exception if appropriate
            }

        }

        public async Task<IEnumerable<PurchaseOrderItem>> GetAllPurchaseOrderItemByIdAsync(int? purchaseOrderID)
        {
            if (purchaseOrderID == null)
            {
                throw new ArgumentNullException(nameof(purchaseOrderID), "Purchase Order Id cannot be null or empty.");
            }

            using var context = DbFactory.CreateDbContext();

            try
            {


                var purchaseOrderItems = await context.PurchaseOrderItems
                    .Include(item => item.Product)
                    .Include(po => po.PurchaseOrder)
                    .Where(po => po.PurchaseOrderID == purchaseOrderID)
                    .ToListAsync();

                return purchaseOrderItems;
            }
            catch (Exception ex)
            {
                // Log the exception (use your logging framework)
                // _logger.LogError(ex, "Error retrieving purchase order by reference number.");
                Console.WriteLine($"Error retrieving purchase order items by reference number: {ex.Message}");
                return null; // Or re-throw the exception if appropriate
            }
        }

        public async Task AddPurchaseOrderItemAsync(PurchaseOrderItem item)
        {

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            using var context = DbFactory.CreateDbContext();

            context.PurchaseOrderItems.Add(item);
            // Save all changes.
            await context.SaveChangesAsync();


        }

        public async Task DeletePurchaseOrderItemAsync(PurchaseOrderItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            using var context = DbFactory.CreateDbContext();
            var purchaseOrder = await GetPurchaseOrderByIdAsync(item.PurchaseOrderID);
            int itemCount = await context.PurchaseOrderItems.CountAsync(poi => poi.PurchaseOrderID == item.PurchaseOrderID);

            if (purchaseOrder == null)
            {
                throw new InvalidOperationException("Purchase Order not found.");
            }

            if (itemCount <= 1)
            {
                throw new InvalidOperationException("Purchase order must have at least one item.");
            }



            context.PurchaseOrderItems.Remove(item);
            await context.SaveChangesAsync();
        }

        public async Task<PurchaseOrder> GetPurchaseOrderByReferenceNumberAsync(string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
                throw new ArgumentNullException(nameof(referenceNumber), "Reference Number cannot be null or empty.");
            }

            using var context = DbFactory.CreateDbContext();

            try
            {

                var purchaseOrder = await context.PurchaseOrders
                    .Include(po => po.CreatedBy)
                    .Include(po => po.Supplier)
                    .Include(po => po.DestinationBranch)
                    .Include(po => po.PurchaseOrderStatus)
                    .Include(po => po.PurchaseOrderItems)
                        .ThenInclude(po => po.Product)
                    .FirstAsync(po => po.ReferenceNumber == referenceNumber);


                return purchaseOrder;
            }
            catch (Exception ex)
            {
                // Log the exception (use your logging framework)
                // _logger.LogError(ex, "Error retrieving purchase order by reference number.");
                Console.WriteLine($"Error retrieving purchase order by reference number: {ex.Message}");
                return null; // Or re-throw the exception if appropriate
            }
        }
    }
}
