using InventiCloud.Models;

namespace InventiCloud.Services.Interface
{
    public interface ICategoryService
    {
        Task AddCategory(Category category);
        Task DeleteCategory(Category category);

        Task<IEnumerable<Category>> GetAllCategoryAsync();

        Task DisposeAsync();
    }
}
