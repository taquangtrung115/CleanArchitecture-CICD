using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;

namespace DemoCICD.Contract.Services.V1.Identity;

public static class Command
{
    // Authentication Commands
    public record Logout(string AccessToken) : ICommand;
    public record RefreshTokenRequest(string AccessToken, string RefreshToken) : ICommand<Response.Authenticated>;
    public record Register(string UserName, string Email, string Password, string FirstName, string LastName, DateTime? DayOfBirth) : ICommand<Response.UserCreated>;
    public record ForgotPassword(string Email) : ICommand;
    
    // User Management Commands
    public record CreateUser(string UserName, string Email, string Password, string FirstName, string LastName, DateTime? DayOfBirth, bool? IsDirector, bool? IsHeadOfDepartment, string? ManagerId, string? PositionId) : ICommand<Response.UserCreated>;
    public record UpdateUser(Guid UserId, string Email, string FirstName, string LastName, DateTime? DayOfBirth, bool? IsDirector, bool? IsHeadOfDepartment, string? ManagerId, string? PositionId, string? Phone = null, string? Address = null, string? City = null, string? Country = null, string? Bio = null, string? Website = null) : ICommand<Response.UserUpdated>;
    public record UpdateProfile(Guid UserId, string FirstName, string LastName, string? Phone = null, string? Address = null, string? City = null, string? Country = null, string? Bio = null, string? Website = null) : ICommand<Response.UserUpdated>;
    public record DeleteUser(Guid UserId) : ICommand;
    public record ChangePassword(Guid UserId, string CurrentPassword, string NewPassword) : ICommand;
    public record ResetPassword(Guid UserId, string NewPassword) : ICommand;
    
    // Password Reset Commands
    public record VerifyResetCode(string Email, string ResetCode) : ICommand<Response.ResetCodeVerified>;
    public record ResetPasswordWithCode(string Email, string ResetCode, string NewPassword) : ICommand;
    public record LockUser(Guid UserId) : ICommand;
    public record UnlockUser(Guid UserId) : ICommand;
    public record AssignUserToRole(Guid UserId, Guid RoleId) : ICommand;
    public record RemoveUserFromRole(Guid UserId, Guid RoleId) : ICommand;
    
    // Account Settings Commands
    public record UpdateNotificationSettings(Guid UserId, bool EmailNotifications, bool PushNotifications, bool SmsNotifications, bool NewsUpdates, bool SecurityAlerts, bool MarketingEmails) : ICommand;
    public record UpdatePrivacySettings(Guid UserId, string ProfileVisibility, bool ShowEmail, bool ShowPhone, bool AllowSearchByEmail, bool AllowSearchByPhone) : ICommand;
    public record RevokeUserSession(Guid UserId, string SessionId) : ICommand;
    public record RevokeAllUserSessions(Guid UserId) : ICommand;
    
    // Role Management Commands
    public record CreateRole(string Name, string Description, string RoleCode) : ICommand<Response.RoleCreated>;
    public record UpdateRole(Guid RoleId, string Name, string Description, string RoleCode) : ICommand<Response.RoleUpdated>;
    public record DeleteRole(Guid RoleId) : ICommand;
    public record GrantPermissionToRole(Guid RoleId, string FunctionId, string ActionId) : ICommand;
    public record RevokePermissionFromRole(Guid RoleId, string FunctionId, string ActionId) : ICommand;
    
    // Permission Management Commands
    public record CreatePermission(Guid RoleId, string FunctionId, string ActionId) : ICommand<Response.PermissionCreated>;
    public record DeletePermission(Guid RoleId, string FunctionId, string ActionId) : ICommand;
    
    // Position Management Commands
    public record CreatePosition(string Name, string Description, string Code, int Level = 1) : ICommand<Response.PositionCreated>;
    public record UpdatePosition(Guid PositionId, string Name, string Description, string Code, int Level, bool IsActive) : ICommand<Response.PositionUpdated>;
    public record DeletePosition(Guid PositionId) : ICommand;
}
