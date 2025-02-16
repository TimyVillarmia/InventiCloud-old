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
                context.Categories.Add(category);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
