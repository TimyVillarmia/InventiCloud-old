using InventiCloud.Entities;
using InventiCloud.Services.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Services
{
    public class ProductService(ILogger<ProductService> _logger,
        IInventoryService inventoryService,
        IDbContextFactory<InventiCloud.Data.ApplicationDbContext> DbFactory) : IProductService
    {
        public async Task AddProductAsync(Product product)
        {
            try
            {
                using var context = DbFactory.CreateDbContext();
                // Check for existing SKU
                if (await context.Products.AnyAsync(p => p.SKU == product.SKU))
                {
                    throw new InvalidOperationException($"SKU '{product.SKU}' already exists.");
                }

                context.Products.Add(product);
                await context.SaveChangesAsync();
                await inventoryService.AddInventoryAsync(product);



            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the product.", product);
                // Handle database-specific exceptions (e.g., unique constraint violations)
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE constraint failed"))
                {
                    throw new InvalidOperationException($"SKU '{product.SKU}' already exists.");
                }
                throw; // Rethrow other DbUpdateExceptions
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the product.", product);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding the product.", product);
                throw;
            }
        }

        

        public async Task DeleteProductAsync(Product product)
        {
            try
            {
                using var context = DbFactory.CreateDbContext();

                if (await context.PurchaseOrderItems.AnyAsync(poi => poi.Product == product))
                       //await context.StockTransfers.AnyAsync(st => st.ProductId == product.Id) ||
                       //await context.StockAdjustments.AnyAsync(sa => sa.ProductId == product.Id) ||
                       //await context.SalesOrderDetails.AnyAsync(sod => sod.ProductId == product.Id))
                {
                    throw new InvalidOperationException("Cannot delete category. It has associated products.");
                }

                context.Products.Remove(product!);
                await context.SaveChangesAsync();

            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the category.", product);
                throw; // Re-throw the exception to be handled in the calling method
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the category.", product);
                throw; // Re-throw the exception to be handled in the calling method
            }
        }

        public async Task DisposeAsync()
        {
            using var context = DbFactory.CreateDbContext();
            await context.DisposeAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductAsync()
        {
            using var context = DbFactory.CreateDbContext();
            return await context.Products
                .Include(p => p.Inventories)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            using var context = DbFactory.CreateDbContext();
            return await context.Products.SingleAsync(e => e.ProductId == id);
        }

        public async Task<Product> GetProductBySKUAsync(string sku)
        {
            using var context = DbFactory.CreateDbContext();
            return await context.Products.SingleAsync(e => e.SKU == sku);
        }

        public bool ProductExists(int productid)
        {
            using var context = DbFactory.CreateDbContext();
            return context.Products.Any(e => e.ProductId == productid);
        }

        public async Task UpdateProductAsync(Product product)
        {
            using var context = DbFactory.CreateDbContext();
            context.Attach(product!).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product!.ProductId))
                {
                    throw;
                }
            }
        }
    }
}
