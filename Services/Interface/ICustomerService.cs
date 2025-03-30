using InventiCloud.Entities;

namespace InventiCloud.Services.Interface
{
    public interface ICustomerService
    {
        Task AddCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);


        Task<IEnumerable<Customer>> GetAllCustomerAsync();

        Task<Customer> GetCustomerByNameAsync(string customerName);

        Task DisposeAsync();
    }
}
