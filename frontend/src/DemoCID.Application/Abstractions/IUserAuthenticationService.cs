using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoCICD.Domain.Entities.Identity;

namespace DemoCICD.Application.Abstractions;

public interface IUserAuthenticationService
{
    Task<UserAuthResult> ValidateUserAsync(string userName, string password);
    Task<IEnumerable<string>> GetUserRolesAsync(string userId);
    Task<UserAuthResult> RegisterUserAsync(string userName, string email, string password, string firstName, string lastName, DateTime? dayOfBirth);
}

public interface IUserManagementService
{
    Task<AppUser?> GetUserByIdAsync(Guid userId);
    Task<(IEnumerable<AppUser> Users, int TotalCount)> GetUsersAsync(int page, int pageSize, string? searchTerm);
    Task<UserAuthResult> CreateUserAsync(string userName, string email, string password, string firstName, string lastName, DateTime? dayOfBirth, bool? isDirector, bool? isHeadOfDepartment, Guid? managerId, Guid positionId);
    Task<bool> UpdateUserAsync(Guid userId, string email, string firstName, string lastName, DateTime? dayOfBirth, bool? isDirector, bool? isHeadOfDepartment, Guid? managerId, Guid positionId);
    Task<bool> DeleteUserAsync(Guid userId);
    Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    Task<bool> ResetPasswordAsync(Guid userId, string newPassword);
    Task<bool> LockUserAsync(Guid userId);
    Task<bool> UnlockUserAsync(Guid userId);
    Task<bool> AssignUserToRoleAsync(Guid userId, Guid roleId);
    Task<bool> RemoveUserFromRoleAsync(Guid userId, Guid roleId);
    Task<IEnumerable<AppRole>> GetUserRolesAsync(Guid userId);
}

public interface IRoleManagementService
{
    Task<AppRole?> GetRoleByIdAsync(Guid roleId);
    Task<(IEnumerable<AppRole> Roles, int TotalCount)> GetRolesAsync(int page, int pageSize, string? searchTerm);
    Task<AppRole> CreateRoleAsync(string name, string description, string roleCode);
    Task<bool> UpdateRoleAsync(Guid roleId, string name, string description, string roleCode);
    Task<bool> DeleteRoleAsync(Guid roleId);
    Task<IEnumerable<AppUser>> GetUsersInRoleAsync(Guid roleId);
    Task<IEnumerable<Permission>> GetRolePermissionsAsync(Guid roleId);
    Task<bool> GrantPermissionToRoleAsync(Guid roleId, string functionId, string actionId);
    Task<bool> RevokePermissionFromRoleAsync(Guid roleId, string functionId, string actionId);
}

public interface IPermissionManagementService
{
    Task<(IEnumerable<Permission> Permissions, int TotalCount)> GetPermissionsAsync(int page, int pageSize);
    Task<Permission?> GetPermissionAsync(Guid roleId, string functionId, string actionId);
    Task<Permission> CreatePermissionAsync(Guid roleId, string functionId, string actionId);
    Task<bool> DeletePermissionAsync(Guid roleId, string functionId, string actionId);
}

public class UserAuthResult
{
    public bool IsSuccess { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? ErrorMessage { get; set; }

    public static UserAuthResult Success(string userId, string userName, string? email = null, string? fullName = null)
    {
        return new UserAuthResult
        {
            IsSuccess = true,
            UserId = userId,
            UserName = userName,
            Email = email,
            FullName = fullName
        };
    }

    public static UserAuthResult Failure(string errorMessage)
    {
        return new UserAuthResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}