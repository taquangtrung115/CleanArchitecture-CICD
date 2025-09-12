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
}
