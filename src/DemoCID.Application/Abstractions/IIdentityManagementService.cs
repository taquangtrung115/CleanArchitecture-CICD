namespace DemoCICD.Application.Abstractions;

/// <summary>
/// Interface cho Identity Management Service
/// Cầu nối giữa AI Chat và các operations quản lý identity
/// </summary>
public interface IIdentityManagementService
{
    /// <summary>
    /// Tạo role mới trong hệ thống
    /// </summary>
    /// <param name="roleName">Tên role</param>
    /// <param name="description">Mô tả role</param>
    /// <param name="roleCode">Mã code của role</param>
    /// <param name="cancellationToken">Token để cancel operation</param>
    /// <returns>Kết quả tạo role</returns>
    Task<object?> CreateRoleAsync(string roleName, string description, string roleCode, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Tạo permission mới
    /// </summary>
    /// <param name="roleId">ID của role</param>
    /// <param name="functionId">ID của function</param>
    /// <param name="actionId">ID của action</param>
    /// <param name="cancellationToken">Token để cancel operation</param>
    /// <returns>Kết quả tạo permission</returns>
    Task<object?> CreatePermissionAsync(Guid roleId, string functionId, string actionId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gán permission vào role
    /// </summary>
    /// <param name="roleId">ID của role</param>
    /// <param name="functionId">ID của function</param>
    /// <param name="actionId">ID của action</param>
    /// <param name="cancellationToken">Token để cancel operation</param>
    /// <returns>Kết quả gán permission</returns>
    Task<object?> AssignPermissionToRoleAsync(Guid roleId, string functionId, string actionId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gán user vào role
    /// </summary>
    /// <param name="userId">ID của user</param>
    /// <param name="roleId">ID của role</param>
    /// <param name="cancellationToken">Token để cancel operation</param>
    /// <returns>Kết quả gán user</returns>
    Task<object?> AssignUserToRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Lấy danh sách tất cả roles
    /// </summary>
    /// <param name="cancellationToken">Token để cancel operation</param>
    /// <returns>Danh sách roles</returns>
    Task<object?> GetRolesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Lấy danh sách tất cả users
    /// </summary>
    /// <param name="cancellationToken">Token để cancel operation</param>
    /// <returns>Danh sách users</returns>
    Task<object?> GetUsersAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Lấy danh sách tất cả permissions
    /// </summary>
    /// <param name="cancellationToken">Token để cancel operation</param>
    /// <returns>Danh sách permissions</returns>
    Task<object?> GetPermissionsAsync(CancellationToken cancellationToken = default);
}