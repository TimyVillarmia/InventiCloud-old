using InventiCloud.Entities;
using Microsoft.EntityFrameworkCore;
using InventiCloud.Services.Interface;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventiCloud.Data;

namespace InventiCloud.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ILogger<CustomerService> logger, IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomerAsync()
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.Customers.ToListAsync();
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            using var context = _dbFactory.CreateDbContext();

            try
            {
                bool emailExists = await context.Customers.AnyAsync(c => c.Email == customer.Email);
                bool phoneNumberExists = await context.Customers.AnyAsync(c => c.PhoneNumber == customer.PhoneNumber);

                if (emailExists && phoneNumberExists)
                {
                    throw new InvalidOperationException($"A customer with the email '{customer.Email}' and phone number '{customer.PhoneNumber}' already exists.");
                }
                else if (emailExists)
                {
                    throw new InvalidOperationException($"A customer with the email '{customer.Email}' already exists.");
                }
                else if (phoneNumberExists)
                {
                    throw new InvalidOperationException($"A customer with the phone number '{customer.PhoneNumber}' already exists.");
                }

                context.Customers.Add(customer);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error adding customer: {Customer}", customer);
                throw; // Rethrow DbUpdateException
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Validation error adding customer: {Customer}", customer);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error adding customer: {Customer}", customer);
                throw;
            }
        }
        public async Task DeleteCustomerAsync(Customer customer)
        {
            using var context = _dbFactory.CreateDbContext();

            try
            {
                if (await context.SalesOrders.AnyAsync(so => so.CustomerId == customer.CustomerId))
                {
                    throw new InvalidOperationException("Cannot delete customer with associated sales orders.");
                }

                context.Customers.Remove(customer);
                await context.SaveChangesAsync();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the customer: {Customer}", customer);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting the customer: {Customer}", customer);
                throw;
            }
        }

        public bool CustomerExists(int customerId)
        {
            using var context = _dbFactory.CreateDbContext();
            return context.Customers.Any(e => e.CustomerId == customerId);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            using var context = _dbFactory.CreateDbContext();
            context.Attach(customer).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customer.CustomerId))
                {
                    throw;
                }
            }
        }

        public async Task DisposeAsync()
        {
            using var context = _dbFactory.CreateDbContext();
            await context.DisposeAsync();
        }

        public async Task<Customer> GetCustomerByNameAsync(string customerName)
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.Customers.FirstOrDefaultAsync(e => e.CustomerName == customerName);
        }

   
    }
}