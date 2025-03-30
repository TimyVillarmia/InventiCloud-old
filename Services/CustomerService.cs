using InventiCloud.Entities;
using Microsoft.EntityFrameworkCore;
using InventiCloud.Services;
using InventiCloud.Services.Interface;

namespace InventiCloud.Services
{
    public class CustomerService(ILogger<CustomerService> _logger, IDbContextFactory<InventiCloud.Data.ApplicationDbContext> DbFactory) : ICustomerService
    {
        public async Task<IEnumerable<Customer>> GetAllCustomerAsync()
        {
            using var context = DbFactory.CreateDbContext();
            return await context.Customers
                .ToListAsync();
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            try
            {
                using var context = DbFactory.CreateDbContext();

                if (await context.Customers.AnyAsync(c => c.CustomerName == customer.CustomerName))
                {
                    throw new InvalidOperationException($"Customer name '{customer.CustomerName}' already exists.");
                }

                context.Customers.Add(customer);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the customer.", customer);
                // Handle database-specific exceptions (e.g., unique constraint violations)
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE constraint failed"))
                {
                    throw new InvalidOperationException($"Customer name '{customer.CustomerName}' already exists.");
                }
                throw; // Rethrow other DbUpdateExceptions
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the customer.", customer);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding the customer.", customer);
                throw;
            }
        }

        public async Task DeleteCustomerAsync(Customer customer)
        {
            try
            {
                using var context = DbFactory.CreateDbContext();

                if (customer.CustomerName.Any())
                {
                    throw new InvalidOperationException("Cannot delete customer.");
                }

                context.Customers.Remove(customer!);
                await context.SaveChangesAsync();

            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the customer.", customer);
                throw; // Re-throw the exception to be handled in the calling method
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the customer.", customer);
                throw; // Re-throw the exception to be handled in the calling method
            }
        }

        public bool CustomerExists(int customerid)
        {
            using var context = DbFactory.CreateDbContext();
            return context.Customers.Any(e => e.CustomerId == customerid);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            using var context = DbFactory.CreateDbContext();
            context.Attach(customer!).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customer!.CustomerId))
                {
                    throw;
                }
            }
        }

        public async Task DisposeAsync()
        {
            using var context = DbFactory.CreateDbContext();
            await context.DisposeAsync();
        }

        public async Task<Customer> GetCustomerByNameAsync(string customerName)
        {
            using var context = DbFactory.CreateDbContext();
            return await context.Customers.FirstAsync(e => e.CustomerName == customerName);
        }

    }
}
