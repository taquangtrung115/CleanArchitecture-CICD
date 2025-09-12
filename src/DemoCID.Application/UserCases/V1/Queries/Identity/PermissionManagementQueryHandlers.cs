using DemoCICD.Application.Abstractions;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using Microsoft.Extensions.Logging;

namespace DemoCICD.Application.UserCases.V1.Queries.Identity;

public class GetPermissionsQueryHandler : IQueryHandler<Query.GetPermissions, Response.PermissionList>
{
    private readonly IPermissionManagementService _permissionManagementService;
    private readonly ILogger<GetPermissionsQueryHandler> _logger;

    public GetPermissionsQueryHandler(
        IPermissionManagementService permissionManagementService,
        ILogger<GetPermissionsQueryHandler> logger)
    {
        _permissionManagementService = permissionManagementService;
        _logger = logger;
    }

    public async Task<Result<Response.PermissionList>> Handle(Query.GetPermissions request, CancellationToken cancellationToken)
    {
        try
        {
            var (permissions, totalCount) = await _permissionManagementService.GetPermissionsAsync(
                request.Page,
                request.PageSize);

            var permissionSummaries = permissions.Select(p => new Response.PermissionSummary(
                p.RoleId,
                p.FunctionId,
                p.ActionId,
                string.Empty, // Role name would be populated from a join
                string.Empty, // Function name would be populated from a join
                string.Empty)); // Action name would be populated from a join

            var response = new Response.PermissionList(
                permissionSummaries,
                totalCount,
                request.Page,
                request.PageSize);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting permissions with page: {Page}, pageSize: {PageSize}", 
                request.Page, request.PageSize);
            return Result.Failure<Response.PermissionList>(
                new Error("Permissions.GetError", "An error occurred while retrieving permissions"));
        }
    }
}

public class GetPermissionByIdQueryHandler : IQueryHandler<Query.GetPermissionById, Response.PermissionDetails>
{
    private readonly IPermissionManagementService _permissionManagementService;
    private readonly ILogger<GetPermissionByIdQueryHandler> _logger;

    public GetPermissionByIdQueryHandler(
        IPermissionManagementService permissionManagementService,
        ILogger<GetPermissionByIdQueryHandler> logger)
    {
        _permissionManagementService = permissionManagementService;
        _logger = logger;
    }

    public async Task<Result<Response.PermissionDetails>> Handle(Query.GetPermissionById request, CancellationToken cancellationToken)
    {
        try
        {
            var permission = await _permissionManagementService.GetPermissionAsync(
                request.RoleId,
                request.FunctionId,
                request.ActionId);

            if (permission == null)
            {
                return Result.Failure<Response.PermissionDetails>(
                    new Error("Permission.NotFound", "Permission not found"));
            }

            var response = new Response.PermissionDetails(
                permission.RoleId,
                permission.FunctionId,
                permission.ActionId,
                string.Empty, // Role name would be populated from a join
                string.Empty, // Function name would be populated from a join
                string.Empty); // Action name would be populated from a join

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting permission by role: {RoleId}, function: {FunctionId}, action: {ActionId}", 
                request.RoleId, request.FunctionId, request.ActionId);
            return Result.Failure<Response.PermissionDetails>(
                new Error("Permission.GetError", "An error occurred while retrieving permission"));
        }
    }
}