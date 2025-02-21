using InventiCloud.Models;
using Microsoft.EntityFrameworkCore;
using InventiCloud.Services;
using InventiCloud.Services.Interface;


namespace InventiCloud.Services
{
    public class CategoryService(ILogger<CategoryService> _logger,IDbContextFactory<InventiCloud.Data.ApplicationDbContext> DbFactory) : ICategoryService
    {
        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            using var context = DbFactory.CreateDbContext();
            return await context.Categories
                .Include(c => c.Products)
                .ToListAsync();
        }


        public async Task AddCategory(Category category)
        {
            try
            {
                using var context = DbFactory.CreateDbContext();

                if (await context.Categories.AnyAsync(c => c.CategoryName == category.CategoryName))
                {
                    throw new InvalidOperationException($"Category name '{category.CategoryName}' already exists.");
                }

                context.Categories.Add(category);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the category.", category);
                // Handle database-specific exceptions (e.g., unique constraint violations)
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE constraint failed"))
                {
                    throw new InvalidOperationException($"Category name '{category.CategoryName}' already exists.");
                }
                throw; // Rethrow other DbUpdateExceptions
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the category.", category);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding the category.", category);
                throw;
            }
        }

        public async Task DeleteCategory(Category category)
        {
            try
            {
                using var context = DbFactory.CreateDbContext();

                if (category.Products.Any())
                {
                    throw new InvalidOperationException("Cannot delete category. It has associated products.");
                }

                context.Categories.Remove(category!);
                await context.SaveChangesAsync();

            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the category.", category);
                throw; // Re-throw the exception to be handled in the calling method
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the category.", category);
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
