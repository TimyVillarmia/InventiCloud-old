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
    public class SalesPersonService : ISalesPersonService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        private readonly ILogger<SalesPersonService> _logger;

        public SalesPersonService(ILogger<SalesPersonService> logger, IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<SalesPerson>> GetAllSalesPersonAsync()
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.SalesPersons.ToListAsync();
        }

        public async Task AddSalesPersonAsync(SalesPerson salesperson)
        {
            using var context = _dbFactory.CreateDbContext();

            try
            {
                bool emailExists = await context.SalesPersons.AnyAsync(sp => sp.Email == salesperson.Email);
                bool phoneNumberExists = await context.SalesPersons.AnyAsync(sp => sp.PhoneNumber == salesperson.PhoneNumber);

                if (emailExists && phoneNumberExists)
                {
                    throw new InvalidOperationException($"A sales person with the email '{salesperson.Email}' and phone number '{salesperson.PhoneNumber}' already exists.");
                }
                else if (emailExists)
                {
                    throw new InvalidOperationException($"A sales person with the email '{salesperson.Email}' already exists.");
                }
                else if (phoneNumberExists)
                {
                    throw new InvalidOperationException($"A sales person with the phone number '{salesperson.PhoneNumber}' already exists.");
                }

                salesperson.BirthDate = salesperson.BirthDate?.Date; // Normalize date to remove time component

                context.SalesPersons.Add(salesperson);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error adding sales person: {Sales Person}", salesperson);
                throw; // Rethrow DbUpdateException
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Validation error adding sales person: {Sales Person}", salesperson);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error adding sales person: {Sales Person}", salesperson);
                throw;
            }
        }

        public async Task DeleteSalesPersonAsync(SalesPerson salesperson)
        {
            using var context = _dbFactory.CreateDbContext();

            try
            {
                if (await context.SalesOrders.AnyAsync(so => so.SalesPersonId == salesperson.SalesPersonId))
                {
                    throw new InvalidOperationException("Cannot delete Sales Person with associated sales orders.");
                }

                context.SalesPersons.Remove(salesperson);
                await context.SaveChangesAsync();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the salesperson: {Sales Person}", salesperson);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting the salesperson: {Sales Person}", salesperson);
                throw;
            }
        }

        public bool SalesPersonExists(int salesPersonId)
        {
            using var context = _dbFactory.CreateDbContext();
            return context.SalesPersons.Any(sp => sp.SalesPersonId == salesPersonId);
        }

        public async Task UpdateSalesPersonAsync(SalesPerson salesperson)
        {
            using var context = _dbFactory.CreateDbContext();
            context.Attach(salesperson).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesPersonExists(salesperson.SalesPersonId))
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

        public async Task<SalesPerson> GetSalesPersonByNameAsync(string fullname)
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.SalesPersons.FirstOrDefaultAsync(sp => sp.FullName == fullname);
        }
    }
}