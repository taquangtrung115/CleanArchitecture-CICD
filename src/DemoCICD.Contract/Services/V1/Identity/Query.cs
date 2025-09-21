using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using MediatR;

namespace DemoCICD.Contract.Services.V1.Identity;

public static class Query
{
    // Authentication Queries
    public record Login(string UserName, string Password) : IQuery<Response.Authenticated>;
    public record Token(string? AccessToken, string? RefreshToken) : IQuery<Response.Authenticated>;
    
    // User Management Queries
    public record GetUserById(Guid UserId) : IQuery<Response.UserDetails>;
    public record GetCurrentUserProfile() : IQuery<Response.UserDetails>;
    public record GetUsers(int Page = 1, int PageSize = 10, string? SearchTerm = null) : IQuery<Response.UserList>;
    public record GetUserRoles(Guid UserId) : IQuery<Response.UserRoleList>;
    
    // Role Management Queries
    public record GetRoleById(Guid RoleId) : IQuery<Response.RoleDetails>;
    public record GetRoles(int Page = 1, int PageSize = 10, string? SearchTerm = null) : IQuery<Response.RoleList>;
    public record GetUsersInRole(Guid RoleId) : IQuery<Response.UserList>;
    public record GetRolePermissions(Guid RoleId) : IQuery<Response.PermissionList>;
    
    // Permission Management Queries
    public record GetPermissions(int Page = 1, int PageSize = 10) : IQuery<Response.PermissionList>;
    public record GetPermissionById(Guid RoleId, string FunctionId, string ActionId) : IQuery<Response.PermissionDetails>;
    
    // Position Management Queries
    public record GetPositions(int Page = 1, int PageSize = 10, string? SearchTerm = null) : IQuery<Response.PositionList>;
    public record GetPositionById(Guid PositionId) : IQuery<Response.PositionDetails>;
    public record GetActivePositions() : IQuery<Response.PositionList>;
    
    // User Management Helper Queries  
    public record GetUsersForManagerSelection() : IQuery<Response.UserList>;
    
    // Action Management Queries
    public record GetActions(int Page = 1, int PageSize = 10, string? SearchTerm = null) : IQuery<Response.ActionList>;
    public record GetActionById(string Id) : IQuery<Response.ActionDetails>;
    public record GetActiveActions() : IQuery<Response.ActionList>;
    
    // Function Management Queries for dropdowns
    public record GetActiveFunctions() : IQuery<Response.FunctionList>;
    
    // ActionInFunction Management Queries
    public record GetActionInFunctions(int Page = 1, int PageSize = 10, string? SearchTerm = null) : IQuery<Response.ActionInFunctionList>;
    public record GetActionInFunction(string ActionId, string FunctionId) : IQuery<Response.ActionInFunctionDetails>;
    
    // Account Settings Queries
    public record GetNotificationSettings(Guid UserId) : IQuery<Response.NotificationSettings>;
    public record GetPrivacySettings(Guid UserId) : IQuery<Response.PrivacySettings>;
    public record GetUserSessions(Guid UserId) : IQuery<Response.UserSessionList>;
}
