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
    public record UserDetails(Guid UserId, string UserName, string Email, string FirstName, string LastName, string FullName, DateTime? DayOfBirth, bool? IsDirector, bool? IsHeadOfDepartment, Guid? ManagerId, Guid PositionId, bool IsLocked, DateTime CreatedAt, string? Phone = null, string? Address = null, string? City = null, string? Country = null, string? Bio = null, string? Website = null, string? Avatar = null);
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
    
    // Position Management Responses
    public record PositionCreated(Guid PositionId, string Name, string Code);
    public record PositionUpdated(Guid PositionId, string Name, string Code);
    public record PositionDetails(Guid PositionId, string Name, string Description, string Code, int Level, bool IsActive, DateTime CreatedAt, DateTime? UpdatedAt);
    public record PositionList(IEnumerable<PositionSummary> Positions, int TotalCount, int Page, int PageSize);
    public record PositionSummary(Guid PositionId, string Name, string Code, int Level, bool IsActive);
    
    // Action Management Responses
    public record ActionCreated(string Id, string Name);
    public record ActionUpdated(string Id, string Name);
    public record ActionDetails(string Id, string Name, int? SortOrder, bool? IsActive);
    public record ActionList(IEnumerable<ActionSummary> Actions, int TotalCount, int Page, int PageSize);
    public record ActionSummary(string Id, string Name, int? SortOrder, bool? IsActive);
    
    // Function Management Responses for dropdowns
    public record FunctionList(IEnumerable<FunctionSummary> Functions, int TotalCount, int Page, int PageSize);
    public record FunctionSummary(string Id, string Name, bool? IsActive);
    
    // Account Settings Responses
    public record NotificationSettings(bool EmailNotifications, bool PushNotifications, bool SmsNotifications, bool NewsUpdates, bool SecurityAlerts, bool MarketingEmails);
    public record PrivacySettings(string ProfileVisibility, bool ShowEmail, bool ShowPhone, bool AllowSearchByEmail, bool AllowSearchByPhone);
    public record UserSession(string SessionId, string DeviceType, string Browser, string Location, DateTime LastActive, bool IsCurrent);
    public record UserSessionList(IEnumerable<UserSession> Sessions);
    
    // Password Reset Responses
    public record ResetCodeVerified(bool IsValid);
}
