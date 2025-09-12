using DemoCICD.Application.Abstractions;
using DemoCICD.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DemoCICD.Infrastructure.Authentication;

public class RoleManagementService : IRoleManagementService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IPermissionManagementService _permissionManagementService;

    public RoleManagementService(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        IPermissionManagementService permissionManagementService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _permissionManagementService = permissionManagementService;
    }

    public async Task<AppRole?> GetRoleByIdAsync(Guid roleId)
    {
        return await _roleManager.FindByIdAsync(roleId.ToString());
    }

    public async Task<(IEnumerable<AppRole> Roles, int TotalCount)> GetRolesAsync(int page, int pageSize, string? searchTerm)
    {
        var query = _roleManager.Roles.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(r => r.Name!.Contains(searchTerm) ||
                                   r.RoleCode.Contains(searchTerm) ||
                                   r.Description.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();
        var roles = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (roles, totalCount);
    }

    public async Task<AppRole> CreateRoleAsync(string name, string description, string roleCode)
    {
        try
        {
            var role = new AppRole
            {
                Name = name,
                NormalizedName = name.ToUpperInvariant(),
                Description = description,
                RoleCode = roleCode
            };

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Role creation failed: {errors}");
            }

            return role;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating role: {RoleName}", name);
            throw;
        }
    }

    public async Task<bool> UpdateRoleAsync(Guid roleId, string name, string description, string roleCode)
    {
        try
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                return false;
            }

            role.Name = name;
            role.NormalizedName = name.ToUpperInvariant();
            role.Description = description;
            role.RoleCode = roleCode;

            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error updating role: {RoleId}", roleId);
            throw;
        }
    }

    public async Task<bool> DeleteRoleAsync(Guid roleId)
    {
        try
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                return false;
            }

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting role: {RoleId}", roleId);
            return false;
        }
    }

    public async Task<IEnumerable<AppUser>> GetUsersInRoleAsync(Guid roleId)
    {
        try
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                return Enumerable.Empty<AppUser>();
            }

            return await _userManager.GetUsersInRoleAsync(role.Name!);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting users in role: {RoleId}", roleId);
            return Enumerable.Empty<AppUser>();
        }
    }

    public async Task<IEnumerable<Permission>> GetRolePermissionsAsync(Guid roleId)
    {
        try
        {
            var role = await _roleManager.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            return role?.Permissions ?? Enumerable.Empty<Permission>();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting permissions for role: {RoleId}", roleId);
            return Enumerable.Empty<Permission>();
        }
    }

    public async Task<bool> GrantPermissionToRoleAsync(Guid roleId, string functionId, string actionId)
    {
        try
        {
            var permission = await _permissionManagementService.CreatePermissionAsync(roleId, functionId, actionId);
            return permission != null;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error granting permission to role: {RoleId}, function: {FunctionId}, action: {ActionId}", roleId, functionId, actionId);
            return false;
        }
    }

    public async Task<bool> RevokePermissionFromRoleAsync(Guid roleId, string functionId, string actionId)
    {
        try
        {
            return await _permissionManagementService.DeletePermissionAsync(roleId, functionId, actionId);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error revoking permission from role: {RoleId}, function: {FunctionId}, action: {ActionId}", roleId, functionId, actionId);
            return false;
        }
    }
}
