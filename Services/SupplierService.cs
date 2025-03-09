using InventiCloud.Entities;
using InventiCloud.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Services
{
    public class SupplierService(ILogger<SupplierService> _logger, IDbContextFactory<InventiCloud.Data.ApplicationDbContext> DbFactory) : ISupplierService
    {
        public async Task AddSupplierAsync(Supplier supplier)
        {
            if (supplier == null)
            {
                throw new ArgumentNullException(nameof(supplier), "Supplier cannot be null.");
            }

            try
            {
                using var context = DbFactory.CreateDbContext();

                if (await context.Suppliers.AnyAsync(s => s.SupplierName == supplier.SupplierName))
                {
                    throw new InvalidOperationException($"Supplier name '{supplier.SupplierName}' already exists.");
                }

                if (await context.Suppliers.AnyAsync(s => s.SupplierCode == supplier.SupplierCode))
                {
                    throw new InvalidOperationException($"Supplier code '{supplier.SupplierCode}' already exists.");
                }

                context.Suppliers.Add(supplier);
                await context.SaveChangesAsync();

                _logger.LogInformation("Supplier '{SupplierName}' added successfully.", supplier.SupplierName);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE constraint failed"))
                {
                    _logger.LogError(ex, "Unique constraint violation while adding supplier '{SupplierName}'.", supplier.SupplierName);
                    throw new InvalidOperationException($"A unique constraint violation occurred (e.g., duplicate supplier name or code).", ex);
                }

                _logger.LogError(ex, "Database error adding supplier '{SupplierName}'.", supplier.SupplierName);
                throw; // Rethrow other DbUpdateExceptions
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation while adding supplier '{SupplierName}'.", supplier.SupplierName);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding supplier '{SupplierName}'.", supplier.SupplierName);
                throw;
            }
        }


        public async Task DeleteSupplierAsync(Supplier supplier)
        {
            if (supplier == null)
            {
                throw new ArgumentNullException(nameof(supplier), "Supplier cannot be null.");
            }

            try
            {
                using var context = DbFactory.CreateDbContext();

                // Check if the supplier has associated purchase orders.
                if (await context.PurchaseOrders.AnyAsync(po => po.SupplierCode == supplier.SupplierCode))
                {
                    throw new InvalidOperationException($"Cannot delete supplier '{supplier.SupplierName}'. It has associated purchase orders.");
                }

                context.Suppliers.Remove(supplier);
                await context.SaveChangesAsync();

                _logger.LogInformation("Supplier '{SupplierName}' deleted successfully.", supplier.SupplierName);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation while deleting supplier '{SupplierName}'.", supplier.SupplierName);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error deleting supplier '{SupplierName}'.", supplier.SupplierName);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting supplier '{SupplierName}'.", supplier.SupplierName);
                throw;
            }
        }

        public async Task DisposeAsync()
        {
            using var context = DbFactory.CreateDbContext();
            await context.DisposeAsync();
        }

        public async Task<IEnumerable<Supplier>> GetAllSupplierAsync()
        {
            using var context = DbFactory.CreateDbContext();
            return await context.Suppliers
                .Include(s => s.PurchaseOrders)
                .ToListAsync();
        }

        public async Task<bool> SupplierExistsAsync(string supplierCode)
        {
            using var context = DbFactory.CreateDbContext();
            return await context.Suppliers.AnyAsync(s => s.SupplierCode == supplierCode);
        }


        public async Task UpdateSupplierAsync(Supplier supplier)
        {
            if (supplier == null)
            {
                throw new ArgumentNullException(nameof(supplier), "Supplier cannot be null.");
            }

            try
            {
                using var context = DbFactory.CreateDbContext();

                context.Update(supplier); // Use context.Update instead of Attach + State

                await context.SaveChangesAsync();

                _logger.LogInformation("Supplier '{SupplierName}' updated successfully.", supplier.SupplierName);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await SupplierExistsAsync(supplier.SupplierCode))
                {
                    _logger.LogError(ex, "Concurrency exception: Supplier '{SupplierName}' no longer exists.", supplier.SupplierName);
                    throw; // Re-throw if supplier doesn't exist.
                }

                _logger.LogError(ex, "Concurrency exception updating supplier '{SupplierName}'.", supplier.SupplierName);
                throw new DbUpdateConcurrencyException($"Concurrency conflict updating supplier '{supplier.SupplierName}'.", ex); // Provide more context
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating supplier '{SupplierName}'.", supplier.SupplierName);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating supplier '{SupplierName}'.", supplier.SupplierName);
                throw;
            }
        }
    }
}
