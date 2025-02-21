using InventiCloud.Models;
using InventiCloud.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Services
{
    public class SupplierService(ILogger<SupplierService> _logger, IDbContextFactory<InventiCloud.Data.ApplicationDbContext> DbFactory) : ISupplierService
    {
        public async Task AddSupplier(Supplier supplier)
        {
            //try
            //{
            //    using var context = DbFactory.CreateDbContext();

            //    if (await context.Suppliers.AnyAsync(s => s.SupplierId == category.CategoryName))
            //    {
            //        throw new InvalidOperationException($"Category name '{category.CategoryName}' already exists.");
            //    }

            //    context.Categories.Add(category);
            //    await context.SaveChangesAsync();
            //}
            //catch (DbUpdateException ex)
            //{
            //    _logger.LogError(ex, "An error occurred while adding the category.", category);
            //    // Handle database-specific exceptions (e.g., unique constraint violations)
            //    if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE constraint failed"))
            //    {
            //        throw new InvalidOperationException($"Category name '{category.CategoryName}' already exists.");
            //    }
            //    throw; // Rethrow other DbUpdateExceptions
            //}
            //catch (InvalidOperationException ex)
            //{
            //    _logger.LogError(ex, "An error occurred while adding the category.", category);
            //    throw;
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "An unexpected error occurred while adding the category.", category);
            //    throw;
            //}
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
    }
}
