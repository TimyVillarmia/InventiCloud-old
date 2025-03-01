using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface IProductService
    {

        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);

        Task DeleteProductAsync(Product product);

        Task<IEnumerable<Product>> GetAllProductAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> GetProductBySKUAsync(string sku);
        bool ProductExists(int productid);
        Task DisposeAsync();
    }
}
