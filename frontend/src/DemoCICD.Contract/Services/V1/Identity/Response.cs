using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Contract.Services.V1.Identity;

public static class Response
{
    // Authentication Responses
    public record Authenticated(string? AccessToken, string? RefreshToken, DateTime? RefreshTokenExpiryTime);
    
    // User Management Responses
    public record UserCreated(Guid UserId, string UserName, string Email);
    public record UserUpdated(Guid UserId, string UserName, string Email);
    public record UserDetails(Guid UserId, string UserName, string Email, string FirstName, string LastName, string FullName, DateTime? DayOfBirth, bool? IsDirector, bool? IsHeadOfDepartment, Guid? ManagerId, Guid PositionId, bool IsLocked, DateTime CreatedAt);
    public record UserList(IEnumerable<UserSummary> Users, int TotalCount, int Page, int PageSize);
    public record UserSummary(Guid UserId, string UserName, string Email, string FullName, bool IsLocked);
    public record UserRoleList(Guid UserId, IEnumerable<RoleSummary> Roles);
    
    // Role Management Responses
    public record RoleCreated(Guid RoleId, string Name, string RoleCode);
    public record RoleUpdated(Guid RoleId, string Name, string RoleCode);
    public record RoleDetails(Guid RoleId, string Name, string Description, string RoleCode, DateTime CreatedAt);
    public record RoleList(IEnumerable<RoleSummary> Roles, int TotalCount, int Page, int PageSize);
    public record RoleSummary(Guid RoleId, string Name, string RoleCode, string Description);
    
    // Permission Management Responses
    public record PermissionCreated(Guid RoleId, string FunctionId, string ActionId);
    public record PermissionDetails(Guid RoleId, string FunctionId, string ActionId, string RoleName, string FunctionName, string ActionName);
    public record PermissionList(IEnumerable<PermissionSummary> Permissions, int TotalCount, int Page, int PageSize);
    public record PermissionSummary(Guid RoleId, string FunctionId, string ActionId, string RoleName, string FunctionName, string ActionName);
}
