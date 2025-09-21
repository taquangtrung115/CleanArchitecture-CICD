
using Microsoft.AspNetCore.Identity;

namespace DemoCICD.Domain.Entities.Identity;

public class AppRole : IdentityRole<Guid>
{
    public Guid RoleId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string RoleCode { get; set; } = string.Empty;
    public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; } = new List<IdentityUserClaim<Guid>>();
    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; } = new List<IdentityUserRole<Guid>>();
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    // Domain methods
    public bool HasPermission(string functionId, string actionId)
    {
        return Permissions?.Any(p => p.FunctionId == functionId && p.ActionId == actionId) == true;
    }

    public bool HasAssignedUsers()
    {
        return UserRoles?.Any() == true;
    }

    public bool IsSystemRole()
    {
        var systemRoles = new[] { "ADMIN", "SUPERADMIN", "SYSTEM" };
        return systemRoles.Contains(RoleCode?.ToUpper());
    }

    public int GetUserCount()
    {
        return UserRoles?.Count ?? 0;
    }

    public int GetPermissionCount()
    {
        return Permissions?.Count ?? 0;
    }
}
