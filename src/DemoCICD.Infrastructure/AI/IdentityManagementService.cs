using DemoCICD.Application.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using DemoCICD.Contract.Services.V1.Identity;

namespace DemoCICD.Infrastructure.AI;

public class IdentityManagementService : IIdentityManagementService
{
    private readonly ISender _sender;
    private readonly ILogger<IdentityManagementService> _logger;

    public IdentityManagementService(ISender sender, ILogger<IdentityManagementService> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    public async Task<object?> CreateRoleAsync(string roleName, string description, string roleCode, CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new Command.CreateRole(roleName, description, roleCode);
            var result = await _sender.Send(command, cancellationToken);
            
            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully created role: {RoleName}", roleName);
                return result.Value;
            }
            
            _logger.LogWarning("Failed to create role: {RoleName}. Error: {Error}", roleName, result.Error);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating role: {RoleName}", roleName);
            return null;
        }
    }

    public async Task<object?> CreatePermissionAsync(Guid roleId, string functionId, string actionId, CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new Command.CreatePermission(roleId, functionId, actionId);
            var result = await _sender.Send(command, cancellationToken);
            
            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully created permission: {FunctionId}.{ActionId} for role: {RoleId}", functionId, actionId, roleId);
                return result.Value;
            }
            
            _logger.LogWarning("Failed to create permission: {FunctionId}.{ActionId}. Error: {Error}", functionId, actionId, result.Error);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating permission: {FunctionId}.{ActionId}", functionId, actionId);
            return null;
        }
    }

    public async Task<object?> AssignPermissionToRoleAsync(Guid roleId, string functionId, string actionId, CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new Command.GrantPermissionToRole(roleId, functionId, actionId);
            var result = await _sender.Send(command, cancellationToken);
            
            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully assigned permission: {FunctionId}.{ActionId} to role: {RoleId}", functionId, actionId, roleId);
                return "Permission assigned to role successfully";
            }
            
            _logger.LogWarning("Failed to assign permission: {FunctionId}.{ActionId} to role: {RoleId}. Error: {Error}", functionId, actionId, roleId, result.Error);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning permission: {FunctionId}.{ActionId} to role: {RoleId}", functionId, actionId, roleId);
            return null;
        }
    }

    public async Task<object?> AssignUserToRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new Command.AssignUserToRole(userId, roleId);
            var result = await _sender.Send(command, cancellationToken);
            
            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully assigned user: {UserId} to role: {RoleId}", userId, roleId);
                return "User assigned to role successfully";
            }
            
            _logger.LogWarning("Failed to assign user: {UserId} to role: {RoleId}. Error: {Error}", userId, roleId, result.Error);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning user: {UserId} to role: {RoleId}", userId, roleId);
            return null;
        }
    }

    public async Task<object?> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new Query.GetRoles(1, 100, null);
            var result = await _sender.Send(query, cancellationToken);
            
            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully retrieved roles");
                return result.Value;
            }
            
            _logger.LogWarning("Failed to retrieve roles. Error: {Error}", result.Error);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving roles");
            return null;
        }
    }

    public async Task<object?> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new Query.GetUsers(1, 100, null);
            var result = await _sender.Send(query, cancellationToken);
            
            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully retrieved users");
                return result.Value;
            }
            
            _logger.LogWarning("Failed to retrieve users. Error: {Error}", result.Error);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return null;
        }
    }

    public async Task<object?> GetPermissionsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new Query.GetPermissions(1, 100);
            var result = await _sender.Send(query, cancellationToken);
            
            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully retrieved permissions");
                return result.Value;
            }
            
            _logger.LogWarning("Failed to retrieve permissions. Error: {Error}", result.Error);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving permissions");
            return null;
        }
    }
}