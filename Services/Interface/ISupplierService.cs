using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface ISupplierService
    {
        Task AddSupplier(Supplier supplier);
        Task UpdateSupplierAsync(Supplier supplier);
        Task DeleteSupplierAsync(Supplier supplier);

        Task<IEnumerable<Supplier>> GetAllSupplierAsync();

        Task DisposeAsync();
    }
}
