using InventiCloud.Models;

namespace InventiCloud.Services.Interface
{
    public interface ISupplierService
    {
        Task AddSupplier(Supplier supplier);
        Task DeleteSupplier(Supplier supplier);

        Task<IEnumerable<Supplier>> GetAllSupplierAsync();

        Task DisposeAsync();
    }
}
