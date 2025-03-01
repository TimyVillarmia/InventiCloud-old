using InventiCloud.Entities;
using InventiCloud.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Services
{
    public class PurchaseOrderService(ILogger<PurchaseOrderService> _logger, IDbContextFactory<InventiCloud.Data.ApplicationDbContext> DbFactory) /*IPurchaseOrderService*/
    {
        //public async Task<IEnumerable<PurchaseOrderService>> GetAllAttributeSetAsync()
        //{
        //    using var context = DbFactory.CreateDbContext();
        //    return await context.PurchaseOrders
        //        .Include(c => c.Supplier) //Galibog ko asa ni siya connect 
        //        .ToListAsync();
        //}
        //public async Task AddAttributeSet(PurchaseOrder purchaseorder)
        //{
        //    try
        //    {
        //        using var context = DbFactory.CreateDbContext();

        //        if (await context.AttributeSets.AnyAsync(c => c.AttributeSetName == purchaseorder.AttributeSetName)) 
        //        {
        //            throw new InvalidOperationException($"AttributeSet name '{purchaseorder.AttributeSetName}' already exists.");
        //        }

        //        context.AttributeSets.Add(purchaseorder);
        //        await context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while adding the category.", purchaseorder);
        //        // Handle database-specific exceptions (e.g., unique constraint violations)
        //        if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE constraint failed"))
        //        {
        //            throw new InvalidOperationException($"AttributeSet name '{purchaseorder.AttributeSetName}' already exists.");
        //        }
        //        throw; // Rethrow other DbUpdateExceptions
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while adding the AttributeSet Name.", purchaseorder);
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An unexpected error occurred while adding the AttributeSet Name.", purchaseorder);
        //        throw;
        //    }

        //}
    }
}
