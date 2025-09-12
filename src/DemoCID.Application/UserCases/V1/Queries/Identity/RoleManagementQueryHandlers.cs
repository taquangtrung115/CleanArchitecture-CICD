using DemoCICD.Application.Abstractions;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using Microsoft.Extensions.Logging;

namespace DemoCICD.Application.UserCases.V1.Queries.Identity;

public class GetRoleByIdQueryHandler : IQueryHandler<Query.GetRoleById, Response.RoleDetails>
{
    private readonly IRoleManagementService _roleManagementService;
    private readonly ILogger<GetRoleByIdQueryHandler> _logger;

    public GetRoleByIdQueryHandler(
        IRoleManagementService roleManagementService,
        ILogger<GetRoleByIdQueryHandler> logger)
    {
        _roleManagementService = roleManagementService;
        _logger = logger;
    }

    public async Task<Result<Response.RoleDetails>> Handle(Query.GetRoleById request, CancellationToken cancellationToken)
    {
        try
        {
            var role = await _roleManagementService.GetRoleByIdAsync(request.RoleId);

            if (role == null)
            {
                return Result.Failure<Response.RoleDetails>(
                    new Error("Role.NotFound", "Role not found"));
            }

            var response = new Response.RoleDetails(
                role.Id,
                role.Name!,
                role.Description,
                role.RoleCode,
                DateTime.UtcNow); // This would come from a created timestamp in real implementation

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting role by ID: {RoleId}", request.RoleId);
            return Result.Failure<Response.RoleDetails>(
                new Error("Role.GetError", "An error occurred while retrieving role"));
        }
    }
}

public class GetRolesQueryHandler : IQueryHandler<Query.GetRoles, Response.RoleList>
{
    private readonly IRoleManagementService _roleManagementService;
    private readonly ILogger<GetRolesQueryHandler> _logger;

    public GetRolesQueryHandler(
        IRoleManagementService roleManagementService,
        ILogger<GetRolesQueryHandler> logger)
    {
        _roleManagementService = roleManagementService;
        _logger = logger;
    }

    public async Task<Result<Response.RoleList>> Handle(Query.GetRoles request, CancellationToken cancellationToken)
    {
        try
        {
            var (roles, totalCount) = await _roleManagementService.GetRolesAsync(
                request.Page,
                request.PageSize,
                request.SearchTerm);

            var roleSummaries = roles.Select(r => new Response.RoleSummary(
                r.Id,
                r.Name!,
                r.RoleCode,
                r.Description));

            var response = new Response.RoleList(
                roleSummaries,
                totalCount,
                request.Page,
                request.PageSize);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting roles with page: {Page}, pageSize: {PageSize}, searchTerm: {SearchTerm}", 
                request.Page, request.PageSize, request.SearchTerm);
            return Result.Failure<Response.RoleList>(
                new Error("Roles.GetError", "An error occurred while retrieving roles"));
        }
    }
}

public class GetUsersInRoleQueryHandler : IQueryHandler<Query.GetUsersInRole, Response.UserList>
{
    private readonly IRoleManagementService _roleManagementService;
    private readonly ILogger<GetUsersInRoleQueryHandler> _logger;

    public GetUsersInRoleQueryHandler(
        IRoleManagementService roleManagementService,
        ILogger<GetUsersInRoleQueryHandler> logger)
    {
        _roleManagementService = roleManagementService;
        _logger = logger;
    }

    public async Task<Result<Response.UserList>> Handle(Query.GetUsersInRole request, CancellationToken cancellationToken)
    {
        try
        {
            var users = await _roleManagementService.GetUsersInRoleAsync(request.RoleId);

            var userSummaries = users.Select(u => new Response.UserSummary(
                u.Id,
                u.UserName!,
                u.Email!,
                u.FullName,
                u.LockoutEnd.HasValue && u.LockoutEnd > DateTimeOffset.UtcNow));

            var response = new Response.UserList(
                userSummaries,
                users.Count(),
                1,
                users.Count());

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting users in role: {RoleId}", request.RoleId);
            return Result.Failure<Response.UserList>(
                new Error("RoleUsers.GetError", "An error occurred while retrieving users in role"));
        }
    }
}

public class GetRolePermissionsQueryHandler : IQueryHandler<Query.GetRolePermissions, Response.PermissionList>
{
    private readonly IRoleManagementService _roleManagementService;
    private readonly ILogger<GetRolePermissionsQueryHandler> _logger;

    public GetRolePermissionsQueryHandler(
        IRoleManagementService roleManagementService,
        ILogger<GetRolePermissionsQueryHandler> logger)
    {
        _roleManagementService = roleManagementService;
        _logger = logger;
    }

    public async Task<Result<Response.PermissionList>> Handle(Query.GetRolePermissions request, CancellationToken cancellationToken)
    {
        try
        {
            var permissions = await _roleManagementService.GetRolePermissionsAsync(request.RoleId);

            var permissionSummaries = permissions.Select(p => new Response.PermissionSummary(
                p.RoleId,
                p.FunctionId,
                p.ActionId,
                string.Empty, // Role name would be populated from a join
                string.Empty, // Function name would be populated from a join
                string.Empty)); // Action name would be populated from a join

            var response = new Response.PermissionList(
                permissionSummaries,
                permissions.Count(),
                1,
                permissions.Count());

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting role permissions for role: {RoleId}", request.RoleId);
            return Result.Failure<Response.PermissionList>(
                new Error("RolePermissions.GetError", "An error occurred while retrieving role permissions"));
        }
    }
}