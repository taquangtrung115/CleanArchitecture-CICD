using DemoCICD.Application.Abstractions;
using DemoCICD.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DemoCICD.Infrastructure.Authentication;

public class UserManagementService : IUserManagementService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly ILogger<UserManagementService> _logger;

    public UserManagementService(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        ILogger<UserManagementService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<AppUser?> GetUserByIdAsync(Guid userId)
    {
        return await _userManager.FindByIdAsync(userId.ToString());
    }

    public async Task<(IEnumerable<AppUser> Users, int TotalCount)> GetUsersAsync(int page, int pageSize, string? searchTerm)
    {
        var query = _userManager.Users.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(u => u.UserName!.Contains(searchTerm) ||
                                   u.Email!.Contains(searchTerm) ||
                                   u.FirstName.Contains(searchTerm) ||
                                   u.LastName.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();
        var users = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (users, totalCount);
    }

    public async Task<UserAuthResult> CreateUserAsync(string userName, string email, string password, string firstName, string lastName, DateTime? dayOfBirth, bool? isDirector, bool? isHeadOfDepartment, Guid? managerId, Guid positionId)
    {
        try
        {
            var existingUser = await _userManager.FindByNameAsync(userName);
            if (existingUser != null)
            {
                return UserAuthResult.Failure("Username already exists");
            }

            existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return UserAuthResult.Failure("Email already exists");
            }

            var user = new AppUser
            {
                UserName = userName,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                FullName = $"{firstName} {lastName}",
                DayOfBirth = dayOfBirth,
                IsDirector = isDirector,
                IsHeadOfDepartment = isHeadOfDepartment,
                ManagerId = managerId,
                PositionId = positionId,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return UserAuthResult.Failure($"User creation failed: {errors}");
            }

            return UserAuthResult.Success(user.Id.ToString(), user.UserName, user.Email, user.FullName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user: {UserName}", userName);
            return UserAuthResult.Failure("An error occurred during user creation");
        }
    }

    public async Task<bool> UpdateUserAsync(Guid userId, string email, string firstName, string lastName, DateTime? dayOfBirth, bool? isDirector, bool? isHeadOfDepartment, Guid? managerId, Guid positionId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            user.Email = email;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.FullName = $"{firstName} {lastName}";
            user.DayOfBirth = dayOfBirth;
            user.IsDirector = isDirector;
            user.IsHeadOfDepartment = isHeadOfDepartment;
            user.ManagerId = managerId;
            user.PositionId = positionId;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> ResetPasswordAsync(Guid userId, string newPassword)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting password for user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> LockUserAsync(Guid userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error locking user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> UnlockUserAsync(Guid userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.SetLockoutEndDateAsync(user, null);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unlocking user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> AssignUserToRoleAsync(Guid userId, Guid roleId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                return false;
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name!);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning user {UserId} to role {RoleId}", userId, roleId);
            return false;
        }
    }

    public async Task<bool> RemoveUserFromRoleAsync(Guid userId, Guid roleId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                return false;
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role.Name!);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing user {UserId} from role {RoleId}", userId, roleId);
            return false;
        }
    }

    public async Task<IEnumerable<AppRole>> GetUserRolesAsync(Guid userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return Enumerable.Empty<AppRole>();
            }

            var roleNames = await _userManager.GetRolesAsync(user);
            var roles = new List<AppRole>();

            foreach (var roleName in roleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    roles.Add(role);
                }
            }

            return roles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting roles for user: {UserId}", userId);
            return Enumerable.Empty<AppRole>();
        }
    }
}