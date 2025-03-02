using InventiCloud.Entities;
using InventiCloud.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Services
{
    public class BranchService(ILogger<BranchAccountService> _logger, IDbContextFactory<InventiCloud.Data.ApplicationDbContext> DbFactory) : IBranchService
    {
        public async Task AddBranch(Branch branch)
        {
            try
            {
                using var context = DbFactory.CreateDbContext();
                // Check for existing SKU
                if (await context.Branches.AnyAsync(b => b.BranchName == branch.BranchName))
                {
                    throw new InvalidOperationException($"'{branch.BranchName}' already exists.");
                }

                context.Branches.Add(branch);
                await context.SaveChangesAsync();



            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the branch.", branch);
                // Handle database-specific exceptions (e.g., unique constraint violations)
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE constraint failed"))
                {
                    throw new InvalidOperationException($"'{branch.BranchName}' already exists.");
                }
                throw; // Rethrow other DbUpdateExceptions
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the branch.", branch);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the branch.", branch);
                throw;
            }
        }

        public async Task DeleteBranch(Branch branch)
        {
            try
            {
                using var context = DbFactory.CreateDbContext();

                if (branch.Inventories.Any())
                {
                    throw new InvalidOperationException("Cannot delete branch. It has existing inventories.");
                }

                context.Branches.Remove(branch!);
                await context.SaveChangesAsync();

            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the branch.", branch);
                throw; // Re-throw the exception to be handled in the calling method
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the branch.", branch);
                throw; // Re-throw the exception to be handled in the calling method
            }
        }

        public async Task DisposeAsync()
        {
            using var context = DbFactory.CreateDbContext();
            await context.DisposeAsync();
        }

        public async Task<IEnumerable<Branch>> GetAllBranchAsync()
        {
            using var context = DbFactory.CreateDbContext();
            return await context.Branches
                .Include(b => b.Inventories)
                .Include(b => b.BranchAccounts)
                .ToListAsync();
        }
    }
}
