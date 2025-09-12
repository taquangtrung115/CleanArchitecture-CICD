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
    
    // User Management Commands
    public record CreateUser(string UserName, string Email, string Password, string FirstName, string LastName, DateTime? DayOfBirth, bool? IsDirector, bool? IsHeadOfDepartment, Guid? ManagerId, Guid PositionId) : ICommand<Response.UserCreated>;
    public record UpdateUser(Guid UserId, string Email, string FirstName, string LastName, DateTime? DayOfBirth, bool? IsDirector, bool? IsHeadOfDepartment, Guid? ManagerId, Guid PositionId) : ICommand<Response.UserUpdated>;
    public record DeleteUser(Guid UserId) : ICommand;
    public record ChangePassword(Guid UserId, string CurrentPassword, string NewPassword) : ICommand;
    public record ResetPassword(Guid UserId, string NewPassword) : ICommand;
    public record LockUser(Guid UserId) : ICommand;
    public record UnlockUser(Guid UserId) : ICommand;
    public record AssignUserToRole(Guid UserId, Guid RoleId) : ICommand;
    public record RemoveUserFromRole(Guid UserId, Guid RoleId) : ICommand;
    
    // Role Management Commands
    public record CreateRole(string Name, string Description, string RoleCode) : ICommand<Response.RoleCreated>;
    public record UpdateRole(Guid RoleId, string Name, string Description, string RoleCode) : ICommand<Response.RoleUpdated>;
    public record DeleteRole(Guid RoleId) : ICommand;
    public record GrantPermissionToRole(Guid RoleId, string FunctionId, string ActionId) : ICommand;
    public record RevokePermissionFromRole(Guid RoleId, string FunctionId, string ActionId) : ICommand;
    
    // Permission Management Commands
    public record CreatePermission(Guid RoleId, string FunctionId, string ActionId) : ICommand<Response.PermissionCreated>;
    public record DeletePermission(Guid RoleId, string FunctionId, string ActionId) : ICommand;
}