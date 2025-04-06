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
    public class SalesOrderService(ILogger<SalesOrderService> _logger,
        IInventoryService inventoryService,
        NavigationManager NavigationManager,
        ICustomerService customerService,
        IDbContextFactory<InventiCloud.Data.ApplicationDbContext> DbFactory) : ISalesOrderService
    {
        public async Task AddSalesOrderAsync(SalesOrder salesOrder, ICollection<SalesOrderItem> salesOrderItems)
        {
            if (salesOrder == null)
            {
                throw new ArgumentNullException(nameof(salesOrder), "Sales order cannot be null.");
            }

            if (salesOrderItems == null || !salesOrderItems.Any())
            {
                throw new InvalidOperationException("Sales order must have at least one item.");
            }

            using var context = DbFactory.CreateDbContext();

            try
            {

                //purchaseOrder.TotalAmount = salesOrderItems.Sum(item => item.SubTotal);

                // Add the sales order.
                context.SalesOrders.Add(salesOrder);
                await context.SaveChangesAsync(); // Generate PurchaseOrderId

                // Generate and set the reference number.
                salesOrder.ReferenceNumber = ReferenceNumberGenerator.GeneratePurchaseOrderReference(salesOrder.SalesOrderId);

                // Update the sales order 
                context.SalesOrders.Update(salesOrder);

                ////// Update status to draft.
                ////await UpdatePurchaseOrderStatusAsync(purchaseOrder, 1, "Draft");

                // Save all changes.
                await context.SaveChangesAsync();


                // Set SalesOrderID for items.
                foreach (var item in salesOrderItems)
                {
                    item.SalesOrderId = salesOrder.SalesOrderId;
                }

                context.SalesOrderItems.AddRange(salesOrderItems);

                // Save all changes.
                await context.SaveChangesAsync();


                NavigationManager.NavigateTo($"/sales/orders/{salesOrder.ReferenceNumber}");


                _logger.LogInformation("Sales order {SalesOrderId} added successfully.", salesOrder.SalesOrderId);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE constraint failed"))
                {
                    _logger.LogError(ex, "Unique constraint violation adding sales order {SalesOrderId}.", salesOrder.SalesOrderId);
                    throw new InvalidOperationException("A unique constraint violation occurred (e.g., duplicate reference number).", ex);
                }
                _logger.LogError(ex, "Database error adding purchase order {SalesOrderId}.", salesOrder.SalesOrderId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding purchase order {SalesOrderId}.", salesOrder.SalesOrderId);
                throw;
            }


        }

        public async Task DeleteSalesOrderAsync(SalesOrder salesOrder)
        {

            if (salesOrder == null)
            {
                throw new ArgumentNullException(nameof(salesOrder), "Sales order cannot be null.");
            }

            try
            {
                using var context = DbFactory.CreateDbContext();

                context.SalesOrders.Remove(salesOrder);
                await context.SaveChangesAsync();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation while deleting sales order. Purchase order not in draft status. SalesOrderId: {SalesOrderId}", salesOrder.SalesOrderId);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while deleting sales order. SalesOrderId: {SalesOrderId}", salesOrder.SalesOrderId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deletisalesng purchase order. SalesOrderId: {SalesOrderId}", salesOrder.SalesOrderId);
                throw;
            }
        }

        public async Task DisposeAsync()
        {
            using var context = DbFactory.CreateDbContext();
            await context.DisposeAsync();
        }

        public async Task<IEnumerable<SalesOrder>> GetAllSalesOrderAsync()
        {
            using var context = DbFactory.CreateDbContext();
            return await context.SalesOrders
                .Include(so => so.CreatedBy)
                .Include(so => so.SalesPerson)
                .Include(so => so.OrderBranch)
                .ToListAsync();
        }

        public async Task AddSalesOrderItemAsync(SalesOrderItem item)
        {

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            using var context = DbFactory.CreateDbContext();

            context.SalesOrderItems.Add(item);
            // Save all changes.
            await context.SaveChangesAsync();

        }

        public async Task UpdateSalesOrderAsync(SalesOrder salesOrder)
        {
            if (salesOrder == null)
            {
                throw new ArgumentNullException(nameof(salesOrder), "Sales order cannot be null.");
            }

            try
            {
                using var context = DbFactory.CreateDbContext();

                // Load the existing purchase order from the database to check its status.
                var existingSalesOrder = await GetSalesOrderByIdAsync(salesOrder.SalesOrderId);

                if (existingSalesOrder == null)
                {
                    throw new InvalidOperationException($"Purchase order with ID {salesOrder.SalesOrderId} not found.");
                }



                // Update the existing purchase order with the new values.
                context.SalesOrders.Update(salesOrder);
                await context.SaveChangesAsync();

                _logger.LogInformation("Sales order {SalesOrderId} updated.", salesOrder.SalesOrderId);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation while updating sales order {SalesOrderId}.", salesOrder.SalesOrderId);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating sales order {SalesOrderId}.", salesOrder.SalesOrderId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating sales order {SalesOrderId}.", salesOrder.SalesOrderId);
                throw;
            }
        }

        public async Task<SalesOrder> GetSalesOrderByIdAsync(int? salesOrderId)
        {
            if (salesOrderId == null)
            {
                throw new ArgumentNullException(nameof(salesOrderId), "Sales Order Id cannot be null or empty.");
            }


            try
            {
                using var context = DbFactory.CreateDbContext();

                var salesOrder = await context.SalesOrders
                    .Include(so => so.CreatedBy)
                    .Include(so => so.SalesPerson)
                    .Include(so => so.OrderBranch)
                    .Include(so => so.SalesOrderItems)
                    .FirstAsync(so => so.SalesOrderId == salesOrderId);

                return salesOrder;
            }
            catch (Exception ex)
            {
                // Log the exception (use your logging framework)
                // _logger.LogError(ex, "Error retrieving purchase order by reference number.");
                Console.WriteLine($"Error retrieving sales order by reference number: {ex.Message}");
                return null; // Or re-throw the exception if appropriate
            }

        }

        public async Task DeleteSalesOrderItemAsync(SalesOrderItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            using var context = DbFactory.CreateDbContext();
            var salesOrder = await GetSalesOrderByIdAsync(item.SalesOrderId);
            int itemCount = await context.SalesOrderItems.CountAsync(soi => soi.SalesOrderId == item.SalesOrderId);

            if (salesOrder == null)
            {
                throw new InvalidOperationException("Purchase Order not found.");
            }

            if (itemCount <= 1)
            {
                throw new InvalidOperationException("Purchase order must have at least one item.");
            }



            context.SalesOrderItems.Remove(item);
            await context.SaveChangesAsync();
        }

        public async Task<SalesOrder> GetSalesOrderByReferenceNumberAsync(string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
                throw new ArgumentNullException(nameof(referenceNumber), "Reference Number cannot be null or empty.");
            }

            using var context = DbFactory.CreateDbContext();

            try
            {

                var salesOrder = await context.SalesOrders
                    .Include(so => so.CreatedBy)
                    .Include(so => so.SalesPerson)
                    .Include(so => so.OrderBranch)
                    .Include(so => so.SalesOrderItems)
                    .ThenInclude(so => so.Product)
                    .FirstAsync(so => so.ReferenceNumber == referenceNumber);


                return salesOrder;
            }
            catch (Exception ex)
            {
                // Log the exception (use your logging framework)
                // _logger.LogError(ex, "Error retrieving purchase order by reference number.");
                Console.WriteLine($"Error retrieving purchase order by reference number: {ex.Message}");
                return null; // Or re-throw the exception if appropriate
            }
        }

            public async Task<IEnumerable<SalesOrderItem>> GetAllSalesOrderItemByIdAsync(int? salesOrderId)
        {
            if (salesOrderId == null)
            {
                throw new ArgumentNullException(nameof(salesOrderId), "Purchase Order Id cannot be null or empty.");
            }

            using var context = DbFactory.CreateDbContext();

            try
            {


                var salesOrderItems = await context.SalesOrderItems
                    .Include(item => item.Product)
                    .Include(so => so.SalesOrder)
                    .Where(so => so.SalesOrderId == salesOrderId)
                    .ToListAsync();

                return salesOrderItems;
            }
            catch (Exception ex)
            {
                // Log the exception (use your logging framework)
                // _logger.LogError(ex, "Error retrieving purchase order by reference number.");
                Console.WriteLine($"Error retrieving purchase order items by reference number: {ex.Message}");
                return null; // Or re-throw the exception if appropriate
            }
        }
    
    }
}
