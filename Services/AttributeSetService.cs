using InventiCloud.Models;
using InventiCloud.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Services
{
    public class AttributeSetService(ILogger<AttributeSetService> _logger, IDbContextFactory<InventiCloud.Data.ApplicationDbContext> DbFactory) : IAttributeSetService
    {
        public async Task<IEnumerable<AttributeSetService>> GetAllAttributeSetAsync()
        {
            using var context = DbFactory.CreateDbContext();
            return await context.AttributeSets
                .Include(c => c.Attributes) //Galibog ko asa ni siya connect
                .ToListAsync();
        }
        public async Task AddAttributeSet(AttributeSet attributesets)
        {
            try
            {
                using var context = DbFactory.CreateDbContext();

                if (await context.AttributeSets.AnyAsync(c => c.AttributeSetName == attributesets.AttributeSetName))
                {
                    throw new InvalidOperationException($"AttributeSet name '{attributesets.AttributeSetName}' already exists.");
                }

                context.AttributeSets.Add(attributesets);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the category.", attributesets);
                // Handle database-specific exceptions (e.g., unique constraint violations)
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE constraint failed"))
                {
                    throw new InvalidOperationException($"AttributeSet name '{attributesets.AttributeSetName}' already exists.");
                }
                throw; // Rethrow other DbUpdateExceptions
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the AttributeSet Name.", attributesets);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding the AttributeSet Name.", attributesets);
                throw;
            }

        }

        public async Task DeleteAttributeSet(AttributeSet attributesets)
        {
            try
            {
                using var context = DbFactory.CreateDbContext();

                if (attributesets.AttributeSetName.Any())
                {
                    throw new InvalidOperationException("Cannot delete category. It has associated products.");
                }

                context.AttributeSets.Remove(attributesets!);
                await context.SaveChangesAsync();

            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the attributeset name.", attributesets);
                throw; // Re-throw the exception to be handled in the calling method
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the attributeset name.", attributesets);
                throw; // Re-throw the exception to be handled in the calling method
            }
        }

        public async Task DisposeAsync()
        {
            using var context = DbFactory.CreateDbContext();
            await context.DisposeAsync();
        }

    }
}