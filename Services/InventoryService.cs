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
            foreach (var branch in await context.Branches.ToListAsync())
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
