using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(Category category);
        Task DeleteCategoryyAsync(Category category);

        Task<Category> GetCategoryByName(string categoryname);

        Task<IEnumerable<Category>> GetAllCategoryAsync();

        Task DisposeAsync();
    }
}
