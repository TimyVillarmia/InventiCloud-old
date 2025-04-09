using InventiCloud.Data;
using InventiCloud.Entities;
using InventiCloud.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace InventiCloud.Services
{
    public class BranchAccountService : IBranchAccountService
    {
        private readonly ILogger<BranchAccountService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

        public BranchAccountService(
            ILogger<BranchAccountService> logger,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _dbFactory = dbFactory;
        }

        public async Task AddBranchAccountAsync(string username, string email, string password, int branchId)
        {
      

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be empty.", nameof(username));
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty.", nameof(email));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty.", nameof(password));
            }

            try
            {
                using var context = _dbFactory.CreateDbContext();

                // Check if the branch already has a user assigned
                var existingUserInAnyBranch = await context.Users
                    .FirstOrDefaultAsync(u => u.BranchId == branchId);
                if (existingUserInAnyBranch != null)
                {
                    var branchName = await context.Branches
                        .Where(b => b.BranchId == branchId)
                        .Select(b => b.BranchName)
                        .FirstOrDefaultAsync();
                    throw new InvalidOperationException($"Branch '{branchName}' already has a user assigned.");
                }

                // Check if a user with the same email (case-insensitive) already exists globally
                var existingUserWithEmail = await _userManager.FindByEmailAsync(email);
                if (existingUserWithEmail != null)
                {
                    throw new InvalidOperationException($"A user with the email '{email}' already exists");
                }

                // Create the new ApplicationUser
                var user = new ApplicationUser { UserName = username, Email = email, BranchId = branchId, EmailConfirmed = true }; // Set EmailConfirmed to true
                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create user '{username}': {errors}");
                }

                const string branchRoleName = "Branch";
                if (!await _roleManager.RoleExistsAsync(branchRoleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(branchRoleName));
                    _logger.LogInformation($"Role '{branchRoleName}' created.");
                }

                // Assign the "branch" role to the new user
                await _userManager.AddToRoleAsync(user, branchRoleName);
                _logger.LogInformation($"Branch account '{username}' (Email: {email}) created and assigned to Branch ID: {branchId} with role '{branchRoleName}'. Email Confirmed: True");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating branch account for Branch ID: {BranchId}, Username: {Username}, Email: {Email}", branchId, username, email);
                throw;
            }
        }




        public async Task DeleteBranchAccountAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));
            }

            try
            {
                using var context = _dbFactory.CreateDbContext();
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    _logger.LogWarning("Branch account with ID '{UserId}' not found during deletion attempt.", userId);
                    return; // Or throw a specific NotFoundException
                }

                // Check if the user has associated PurchaseOrders
                var hasPurchaseOrders = await context.PurchaseOrders
                    .AnyAsync(po => po.CreatedById == userId); 

                if (hasPurchaseOrders)
                {
                    throw new InvalidOperationException($"'{user.UserName}' cannot be deleted as it has associated transactions");
                }

                // Remove user from all roles
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any())
                {
                    var resultRemoveRoles = await _userManager.RemoveFromRolesAsync(user, roles);
                    if (!resultRemoveRoles.Succeeded)
                    {
                        var errors = string.Join(", ", resultRemoveRoles.Errors.Select(e => e.Description));
                        _logger.LogError("Failed to remove roles for user '{UserId}': {Errors}", userId, errors);
                        throw new Exception($"Failed to remove roles for user '{user.UserName}': {errors}");
                    }
                }

                // Attempt to delete the user
                var resultDelete = await _userManager.DeleteAsync(user);
                if (!resultDelete.Succeeded)
                {
                    var errors = string.Join(", ", resultDelete.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to delete user with ID '{UserId}': {Errors}", userId, errors);
                    throw new Exception($"Failed to delete branch account  '{user.UserName}': {errors}");
                }

                _logger.LogInformation("Branch account with ID '{UserId}' and associated user successfully deleted.", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting branch account with ID: {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllBranchAccountsAsync()
        {
            try
            {
                using var context = _dbFactory.CreateDbContext();
                return await context.Users
                    .Where(u => u.BranchId.HasValue)
                    .Include(u => u.Branch) // Assuming you want to load the Branch navigation property
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all branch accounts.");
                throw;
            }
        }

        public async Task<ApplicationUser> GetBranchAccountByBranchIdAsync(int branchId)
        {
            try
            {
                using var context = _dbFactory.CreateDbContext();
                return await context.Users
                    .FirstOrDefaultAsync(u => u.BranchId == branchId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving branch account for Branch ID: {BranchId}", branchId);
                throw;
            }
        }

        public async Task DisposeAsync()
        {
            using var context = _dbFactory.CreateDbContext();
            await context.DisposeAsync();
        }

       public async Task UpdateBranchAccountAsync(string userId, string? newEmail = null, string? newPassword = null, int? newBranchId = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or whitespace.", nameof(userId));
            }

            try
            {
                using var context = _dbFactory.CreateDbContext();
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    throw new InvalidOperationException($"Branch account with ID '{userId}' not found.");
                }

                bool hasChanges = false;

                // Update Username
                if (!string.IsNullOrWhiteSpace(newEmail) && user.UserName != newEmail)
                {
                    var existingUserWithUsername = await _userManager.FindByNameAsync(newEmail);
                    if (existingUserWithUsername != null && existingUserWithUsername.Id != userId)
                    {
                        throw new ValidationException($"Email '{newEmail}' is already taken.");
                    }
                    user.UserName = newEmail;
                    hasChanges = true;
                }

                // Update Email
                if (!string.IsNullOrWhiteSpace(newEmail) && user.Email != newEmail)
                {
                    var existingUserWithEmail = await _userManager.FindByEmailAsync(newEmail);
                    if (existingUserWithEmail != null && existingUserWithEmail.Id != userId)
                    {
                        throw new ValidationException($"Email '{newEmail}' is already taken.");
                    }
                    user.Email = newEmail;
                    user.EmailConfirmed = false; // Require email confirmation again
                    hasChanges = true;
                }

               

                // Update Password
                if (!string.IsNullOrWhiteSpace(newPassword))
                {
                    // Check password against regex
                    const string PasswordRegex = "^(?=.*[0-9])(?=.*[A-Z]).+$"; // Consider making this a configurable constant
                    const string PasswordErrorMessage = "Passwords must have at least one digit and one uppercase letter."; // Consider making this a configurable constant
                    if (!Regex.IsMatch(newPassword, PasswordRegex))
                    {
                        throw new ValidationException(PasswordErrorMessage);
                    }

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                    if (!result.Succeeded)
                    {
                        string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"{errors}");
                    }
                    hasChanges = true;
                }

                if (hasChanges)
                {
                    var updateResult = await _userManager.UpdateAsync(user);
                    if (!updateResult.Succeeded)
                    {
                        string errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                        throw new Exception($"{errors}");
                    }
                    _logger.LogInformation($"Branch account with ID '{userId}' updated successfully.");
                }
                else
                {
                    _logger.LogInformation($"No changes were provided for branch account with ID '{userId}'.");
                }
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error updating branch account with ID: {UserId}", userId);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation error updating branch account with ID: {UserId}", userId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating branch account with ID: {UserId}", userId);
                throw;
            }
        }


        public async Task<ApplicationUser> GetBranchAccountByUserName(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be null or whitespace.", nameof(username));
            }

            try
            {
                using var context = _dbFactory.CreateDbContext();
                return await context.Users
                    .Include(u => u.Branch) 
                    .FirstOrDefaultAsync(u => u.UserName == username && u.BranchId.HasValue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving branch account by username: {Username}", username);
                throw;
            }
        }


        public async Task<bool> IsBranchAccountExist(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false; // Or throw ArgumentException if at least one should be provided
            }

            try
            {
                
                // Check by email (case-insensitive)
                if (!string.IsNullOrWhiteSpace(email))
                {
                    var userByEmail = await _userManager.FindByEmailAsync(email);
                    if (userByEmail != null)
                    {
                        return true;
                    }
                }

                return false; // No matching user found
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if branch account exists. Email: {Email}", email);
                throw; // Re-throw the exception for the calling code to handle
            }
        }
    }
}