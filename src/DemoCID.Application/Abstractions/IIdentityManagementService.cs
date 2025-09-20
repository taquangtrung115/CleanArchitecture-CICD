namespace DemoCICD.Application.Abstractions;

public interface IIdentityManagementService
{
    Task<object?> CreateRoleAsync(string roleName, string description, string roleCode, CancellationToken cancellationToken = default);
    Task<object?> CreatePermissionAsync(Guid roleId, string functionId, string actionId, CancellationToken cancellationToken = default);
    Task<object?> AssignPermissionToRoleAsync(Guid roleId, string functionId, string actionId, CancellationToken cancellationToken = default);
    Task<object?> AssignUserToRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default);
    Task<object?> GetRolesAsync(CancellationToken cancellationToken = default);
    Task<object?> GetUsersAsync(CancellationToken cancellationToken = default);
    Task<object?> GetPermissionsAsync(CancellationToken cancellationToken = default);
}