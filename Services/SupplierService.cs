using InventiCloud.Models;
using InventiCloud.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Services
{
    public class SupplierService(ILogger<SupplierService> _logger, IDbContextFactory<InventiCloud.Data.ApplicationDbContext> DbFactory) : ISupplierService
    {
        public async Task AddSupplier(Supplier supplier)
        {
            try
            {
                using var context = DbFactory.CreateDbContext();

                if (await context.Suppliers.AnyAsync(s => s.SupplierName == supplier.SupplierName))
                {
                    throw new InvalidOperationException($"Supplier name '{supplier.SupplierName}' already exists.");
                }

                context.Suppliers.Add(supplier);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the category.", supplier);
                // Handle database-specific exceptions (e.g., unique constraint violations)
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE constraint failed"))
                {
                    throw new InvalidOperationException($"Supplier name '{supplier.SupplierName}' already exists.");
                }
                throw; // Rethrow other DbUpdateExceptions
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the supplier.", supplier);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding the supplier.", supplier);
                throw;
            }
        }

        public Task DeleteSupplier(Supplier supplier)
        {
            throw new NotImplementedException();
        }

        public Task DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Supplier>> GetAllSupplierAsync()
        {
            using var context = DbFactory.CreateDbContext();
            return await context.Suppliers
                .ToListAsync();
        }

        public bool SupplierExist(int supplierid)
        {
            using var context = DbFactory.CreateDbContext();
            return context.Suppliers.Any(e => e.SupplierId == supplierid);
        }

        public async Task UpdateSupplierAsync(Supplier supplier)
        {
            using var context = DbFactory.CreateDbContext();
            context.Attach(supplier!).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExist(supplier!.SupplierId))
                {
                    throw;
                }
            }
        }
    }
}
