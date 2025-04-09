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
                bool skuExists = await context.Products.AnyAsync(p => p.SKU == product.SKU);

                // Check for existing Product Name
                bool productNameExists = await context.Products.AnyAsync(p => p.ProductName == product.ProductName);

                if (skuExists && productNameExists)
                {
                    throw new InvalidOperationException($"A product with the SKU '{product.SKU}' and name '{product.ProductName}' already exists.");
                }
                else if (skuExists)
                {
                    throw new InvalidOperationException($"A product with the SKU '{product.SKU}' already exists.");
                }
                else if (productNameExists)
                {
                    throw new InvalidOperationException($"A product with the name '{product.ProductName}' already exists.");
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
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            }

            try
            {
                using var context = DbFactory.CreateDbContext();

                // Check if there are any associated PurchaseOrderItems
                if (await context.PurchaseOrderItems.AnyAsync(poi => poi.ProductID == product.ProductId))
                {
                    throw new InvalidOperationException("Cannot delete product. It has associated purchase order items.");
                }

                // Check if any inventory quantities are not zero
                var inventory = await context.Inventories.FirstOrDefaultAsync(i => i.ProductId == product.ProductId);
                if (inventory != null && (inventory.OnHandquantity != 0 && inventory.IncomingQuantity != 0 && inventory.AvailableQuantity != 0 && inventory.Allocated != 0))
                {
                    throw new InvalidOperationException("Unable to delete. Please adjust inventory to zero before deleting this product.");
                }

                // Remove the product if no associated data is found
                context.Products.Remove(product);
                await context.SaveChangesAsync();

                _logger.LogInformation("Product {ProductId} deleted successfully.", product.ProductId);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Failed to delete product {ProductId}: {Message}", product.ProductId, ex.Message);
                throw; // Re-throw the exception to be handled in the calling method
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting product {ProductId}.", product.ProductId);
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
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            using var context = DbFactory.CreateDbContext();
            try
            {
                return await context.Products.FirstOrDefaultAsync(e => e.ProductId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product by ID: {ProductId}", id);
                return null; // Or re-throw, depending on your error handling policy
            }
        }

        public async Task<Product?> GetProductBySKUAsync(string sku)
        {
            using var context = DbFactory.CreateDbContext();
            try
            {
                return await context.Products.FirstOrDefaultAsync(e => e.SKU == sku);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product by SKU: {SKU}", sku);
                return null; // Or re-throw, depending on your error handling policy
            }
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
