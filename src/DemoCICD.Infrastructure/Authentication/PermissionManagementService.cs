using DemoCICD.Application.Abstractions;
using DemoCICD.Domain.Entities.Identity;
using DemoCICD.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DemoCICD.Infrastructure.Authentication;

public class PermissionManagementService : IPermissionManagementService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PermissionManagementService> _logger;

    public PermissionManagementService(
        ApplicationDbContext context,
        ILogger<PermissionManagementService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<(IEnumerable<Permission> Permissions, int TotalCount)> GetPermissionsAsync(int page, int pageSize)
    {
        try
        {
            var query = _context.Permissions.AsQueryable();
            
            var totalCount = await query.CountAsync();
            var permissions = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (permissions, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting permissions with page: {Page}, pageSize: {PageSize}", page, pageSize);
            return (Enumerable.Empty<Permission>(), 0);
        }
    }

    public async Task<Permission?> GetPermissionAsync(Guid roleId, string functionId, string actionId)
    {
        try
        {
            return await _context.Permissions
                .FirstOrDefaultAsync(p => p.RoleId == roleId && 
                                         p.FunctionId == functionId && 
                                         p.ActionId == actionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting permission for role: {RoleId}, function: {FunctionId}, action: {ActionId}", 
                roleId, functionId, actionId);
            return null;
        }
    }

    public async Task<Permission> CreatePermissionAsync(Guid roleId, string functionId, string actionId)
    {
        try
        {
            // Check if permission already exists
            var existingPermission = await GetPermissionAsync(roleId, functionId, actionId);
            if (existingPermission != null)
            {
                return existingPermission;
            }

            var permission = new Permission
            {
                RoleId = roleId,
                FunctionId = functionId,
                ActionId = actionId
            };

            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();

            return permission;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating permission for role: {RoleId}, function: {FunctionId}, action: {ActionId}", 
                roleId, functionId, actionId);
            throw;
        }
    }

    public async Task<bool> DeletePermissionAsync(Guid roleId, string functionId, string actionId)
    {
        try
        {
            var permission = await GetPermissionAsync(roleId, functionId, actionId);
            if (permission == null)
            {
                return false;
            }

            _context.Permissions.Remove(permission);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting permission for role: {RoleId}, function: {FunctionId}, action: {ActionId}", 
                roleId, functionId, actionId);
            return false;
        }
    }
}