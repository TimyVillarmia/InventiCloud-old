using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface ISalesPersonService
    {
        Task AddSalesPersonAsync(SalesPerson salesperson);
        Task DeleteSalesPersonAsync(SalesPerson salesperson);
        Task UpdateSalesPersonAsync(SalesPerson salesperson);


        Task<IEnumerable<SalesPerson>> GetAllSalesPersonAsync();

        Task<SalesPerson> GetSalesPersonByNameAsync(string fullname);
        Task DisposeAsync();
    }
}
