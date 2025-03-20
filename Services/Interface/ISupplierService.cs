using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface ISupplierService
    {
        Task AddSupplierAsync(Supplier supplier);
        Task UpdateSupplierAsync(Supplier supplier);
        Task DeleteSupplierAsync(Supplier supplier);
        Task<bool> SupplierExistsAsync(string supplierCode);
        Task<IEnumerable<Supplier>> GetAllSupplierAsync();
        Task<Supplier> GetSupplierByCodeAsync(string supplierCode);

        Task DisposeAsync();
    }
}
