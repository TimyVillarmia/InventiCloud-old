using InventiCloud.Models;
using InventiCloud.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Services
{
    public class InventoryService(ILogger<InventoryService> _logger, IDbContextFactory<InventiCloud.Data.ApplicationDbContext> DbFactory) : IInventoryService
    {
        public async Task AddInventoryAsync(Product product)
        {
            using var context = DbFactory.CreateDbContext();


            try
            {
                if (! await context.Branches.AnyAsync())
                {
                    throw new InvalidOperationException("Cannot add inventory. No branches exist.");

                }
                var branches = await context.Branches.ToListAsync(); // Fetch branches once

                foreach (var branch in branches)
                {
                    context.Inventories.Add(new Inventory
                    {
                        ProductID = product.ProductId,
                        BranchID = branch.BranchId,
                        OnHand = 0,
                        Incoming = 0,
                        Unavailable = 0
                    });
                }
                await context.SaveChangesAsync();
            }
            catch (InvalidOperationException ex)
            {
                // Handle specific InvalidOperationException (no branches)
                _logger.LogError(ex, "Error adding inventory for product {ProductId}: {Message}", product.ProductId, ex.Message);
                throw; // Rethrow to propagate the exception
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exceptions
                _logger.LogError(ex, "Database error adding inventory for product {ProductId}: {Message}", product.ProductId, ex.Message);
                throw; // Rethrow
            }
            catch (Exception ex)
            {
                // Handle generic exceptions
                _logger.LogError(ex, "Unexpected error adding inventory for product {ProductId}: {Message}", product.ProductId, ex.Message);
                throw; // Rethrow
            }

        }

        public Task DeleteInventoryAsync(Inventory inventory)
        {
            throw new NotImplementedException();
        }

        public async Task DisposeAsync()
        {
            using var context = DbFactory.CreateDbContext();
            await context.DisposeAsync();
        }

        public async Task<IEnumerable<Inventory>> GetAllInventoryAsync()
        {
            using var context = DbFactory.CreateDbContext();
            return await context.Inventories
                .Include(i => i.Branch)
                .Include(i => i.Product)
                .ToListAsync();
        }
    }
}
