using DemoCICD.Domain.Entities.Identity;

namespace DemoCICD.Domain.Services.Identity;

public class RoleDomainService : IRoleDomainService
{
    private const int MaxNameLength = 256;
    private const int MaxRoleCodeLength = 100;
    private const int MaxDescriptionLength = 1000;

    public bool CanCreateRole(string name, string roleCode, string description)
    {
        var validationErrors = ValidateRoleParameters(name, roleCode, description);
        return !validationErrors.Any();
    }

    public bool CanUpdateRole(AppRole role, string newName, string newRoleCode, string newDescription)
    {
        if (role == null)
            return false;

        var validationErrors = ValidateRoleParameters(newName, newRoleCode, newDescription);
        return !validationErrors.Any();
    }

    public bool CanDeleteRole(AppRole role, bool hasAssignedUsers)
    {
        if (role == null)
            return false;

        // Business rule: Cannot delete a role that has users assigned to it
        if (hasAssignedUsers)
            return false;

        // Business rule: Cannot delete system roles (e.g., Admin, SuperAdmin)
        if (IsSystemRole(role.RoleCode))
            return false;

        return true;
    }

    public AppRole CreateRole(string name, string description, string roleCode)
    {
        var validationErrors = ValidateRoleParameters(name, roleCode, description);
        if (validationErrors.Any())
        {
            throw new ArgumentException($"Invalid role parameters: {string.Join(", ", validationErrors)}");
        }

        return new AppRole
        {
            Id = Guid.NewGuid(),
            RoleId = Guid.NewGuid(),
            Name = name.Trim(),
            NormalizedName = name.Trim().ToUpper(),
            Description = description?.Trim() ?? string.Empty,
            RoleCode = roleCode.Trim().ToUpper(), // Business rule: Role codes are always uppercase
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };
    }

    public void UpdateRole(AppRole role, string name, string description, string roleCode)
    {
        if (role == null)
            throw new ArgumentNullException(nameof(role));

        var validationErrors = ValidateRoleParameters(name, roleCode, description);
        if (validationErrors.Any())
        {
            throw new ArgumentException($"Invalid role parameters: {string.Join(", ", validationErrors)}");
        }

        role.Name = name.Trim();
        role.NormalizedName = name.Trim().ToUpper();
        role.Description = description?.Trim() ?? string.Empty;
        role.RoleCode = roleCode.Trim().ToUpper(); // Business rule: Role codes are always uppercase
        role.ConcurrencyStamp = Guid.NewGuid().ToString();
    }

    public IEnumerable<string> ValidateRole(AppRole role)
    {
        if (role == null)
        {
            yield return "Role cannot be null";
            yield break;
        }

        foreach (var error in ValidateRoleParameters(role.Name, role.RoleCode, role.Description))
        {
            yield return error;
        }

        // Additional business rules specific to existing roles
        if (role.Id == Guid.Empty)
            yield return "Role must have a valid ID";

        if (role.RoleId == Guid.Empty)
            yield return "Role must have a valid RoleId";
    }

    public bool CanGrantPermission(AppRole role, string functionId, string actionId)
    {
        if (role == null || string.IsNullOrWhiteSpace(functionId) || string.IsNullOrWhiteSpace(actionId))
            return false;

        // Business rule: Cannot modify permissions of system roles
        if (IsSystemRole(role.RoleCode))
            return false;

        // Business rule: Check if permission already exists
        return role.Permissions?.Any(p => p.FunctionId == functionId && p.ActionId == actionId) != true;
    }

    public bool CanRevokePermission(AppRole role, string functionId, string actionId)
    {
        if (role == null || string.IsNullOrWhiteSpace(functionId) || string.IsNullOrWhiteSpace(actionId))
            return false;

        // Business rule: Cannot modify permissions of system roles
        if (IsSystemRole(role.RoleCode))
            return false;

        // Business rule: Check if permission exists
        return role.Permissions?.Any(p => p.FunctionId == functionId && p.ActionId == actionId) == true;
    }

    private IEnumerable<string> ValidateRoleParameters(string name, string roleCode, string description)
    {
        // Validate name
        if (string.IsNullOrWhiteSpace(name))
            yield return "Role name is required";
        else if (name.Trim().Length > MaxNameLength)
            yield return $"Role name cannot exceed {MaxNameLength} characters";

        // Validate role code
        if (string.IsNullOrWhiteSpace(roleCode))
            yield return "Role code is required";
        else if (roleCode.Trim().Length > MaxRoleCodeLength)
            yield return $"Role code cannot exceed {MaxRoleCodeLength} characters";
        else if (!IsValidRoleCode(roleCode.Trim()))
            yield return "Role code can only contain letters, numbers, and underscores";

        // Validate description
        if (!string.IsNullOrEmpty(description) && description.Trim().Length > MaxDescriptionLength)
            yield return $"Role description cannot exceed {MaxDescriptionLength} characters";
    }

    private bool IsValidRoleCode(string roleCode)
    {
        // Business rule: Role codes can only contain alphanumeric characters and underscores
        return !string.IsNullOrEmpty(roleCode) && roleCode.All(c => char.IsLetterOrDigit(c) || c == '_');
    }

    private bool IsSystemRole(string roleCode)
    {
        // Business rule: System roles that cannot be deleted or modified
        var systemRoles = new[] { "ADMIN", "SUPERADMIN", "SYSTEM" };
        return systemRoles.Contains(roleCode?.ToUpper());
    }
}
