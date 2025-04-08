using InventiCloud.Data;
using InventiCloud.Entities;
using InventiCloud.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InventiCloud.Services
{
    public class BranchService(ILogger<BranchService> _logger,
        IDbContextFactory<InventiCloud.Data.ApplicationDbContext> DbFactory) : IBranchService
    {
        public async Task AddBranch(Branch branch)
        {
            try
            {
                using var context = DbFactory.CreateDbContext();
                if (await context.Branches.AnyAsync(b => b.BranchName == branch.BranchName))
                {
                    throw new InvalidOperationException($"'{branch.BranchName}' already exists.");
                }
                if (await context.Branches.AnyAsync(b => b.PhoneNumber == branch.PhoneNumber))
                {
                    throw new InvalidOperationException($"A branch with the phone number '{branch.PhoneNumber}' already exists.");
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
                var existingbranch = context.Branches
                    .Include(b => b.Inventories)
                    .FirstOrDefault(b => b.BranchName == branch.BranchName);

                if (existingbranch.Inventories.Any())
                {
                    throw new InvalidOperationException("Cannot delete branch. It has existing inventories.");
                }

                context.Branches.Remove(existingbranch!);
                await context.SaveChangesAsync();

            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.Message, "An error occurred while deleting the branch.", branch);
                throw; // Re-throw the exception to be handled in the calling method
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "An error occurred while deleting the branch.", branch);
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
                .Include(b => b.ApplicationUser)
                .ToListAsync();
        }

        public async Task<Branch> GetBranchByNameAsync(string branchName)
        {
            if (string.IsNullOrWhiteSpace(branchName))
            {
                throw new ArgumentException("Branch name cannot be null or whitespace.", nameof(branchName));
            }

            try
            {
                using var context = DbFactory.CreateDbContext();
                return await context.Branches
                    .FirstOrDefaultAsync(b => b.BranchName == branchName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving branch by name: {BranchName}", branchName);
                throw;
            }
        }

        public async Task<bool> IsBranchExist(string branchName)
        {
            if (branchName == null)
            {
                return false; // Or throw ArgumentNullException if you consider null an error
            }

            try
            {
                using var context = DbFactory.CreateDbContext();
                return await context.Branches.AnyAsync(b => b.BranchName == branchName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if branch exists: {BranchName}", branchName);
                throw; // Re-throw the exception for the calling code to handle
            }
        }

        public async Task<bool> IsBranchNumberExist(string branchNumber)
        {
            if (branchNumber == null)
            {
                return false; // Or throw ArgumentNullException if you consider null an error
            }

            try
            {
                using var context = DbFactory.CreateDbContext();
                return await context.Branches.AnyAsync(b => b.PhoneNumber == branchNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if branch exists: {branchNumber}", branchNumber);
                throw; // Re-throw the exception for the calling code to handle
            }
        }


        public async Task UpdateBranch(Branch branch)
        {
            if (branch == null)
            {
                throw new ArgumentNullException(nameof(branch), "Branch object cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(branch.BranchName))
            {
                throw new ValidationException("Branch name cannot be null or whitespace.");
            }

            try
            {
                using var context = DbFactory.CreateDbContext();

                // Check for uniqueness of the branch name (excluding the current branch being updated)
                var existingBranch = await context.Branches
                    .Where(b => b.BranchName == branch.BranchName && b.BranchId != branch.BranchId)
                    .FirstOrDefaultAsync();

                if (existingBranch != null)
                {
                    throw new ValidationException($"A branch with the name '{branch.BranchName}' already exists.");
                }

                context.Branches.Update(branch);
                await context.SaveChangesAsync();
                _logger.LogInformation("Branch with ID {BranchId} updated successfully.", branch.BranchId);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error updating branch with ID {BranchId}.", branch.BranchId);
                throw;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating branch with ID {BranchId}.", branch.BranchId);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating branch with ID {BranchId}.", branch.BranchId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating branch with ID {BranchId}.", branch.BranchId);
                throw;
            }
        }

    }
    
}
