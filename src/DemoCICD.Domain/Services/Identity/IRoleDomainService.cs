using DemoCICD.Domain.Entities.Identity;

namespace DemoCICD.Domain.Services.Identity;

public interface IRoleDomainService
{
    /// <summary>
    /// Validates if a role can be created with the given parameters
    /// </summary>
    /// <param name="name">Role name</param>
    /// <param name="roleCode">Role code</param>
    /// <param name="description">Role description</param>
    /// <returns>True if role can be created, false otherwise</returns>
    bool CanCreateRole(string name, string roleCode, string description);

    /// <summary>
    /// Validates if a role can be updated with the given parameters
    /// </summary>
    /// <param name="role">Current role</param>
    /// <param name="newName">New role name</param>
    /// <param name="newRoleCode">New role code</param>
    /// <param name="newDescription">New role description</param>
    /// <returns>True if role can be updated, false otherwise</returns>
    bool CanUpdateRole(AppRole role, string newName, string newRoleCode, string newDescription);

    /// <summary>
    /// Validates if a role can be deleted
    /// </summary>
    /// <param name="role">Role to delete</param>
    /// <param name="hasAssignedUsers">Whether the role has users assigned to it</param>
    /// <returns>True if role can be deleted, false otherwise</returns>
    bool CanDeleteRole(AppRole role, bool hasAssignedUsers);

    /// <summary>
    /// Creates a new role with business rules applied
    /// </summary>
    /// <param name="name">Role name</param>
    /// <param name="description">Role description</param>
    /// <param name="roleCode">Role code</param>
    /// <returns>New role instance</returns>
    AppRole CreateRole(string name, string description, string roleCode);

    /// <summary>
    /// Updates an existing role with business rules applied
    /// </summary>
    /// <param name="role">Role to update</param>
    /// <param name="name">New name</param>
    /// <param name="description">New description</param>
    /// <param name="roleCode">New role code</param>
    void UpdateRole(AppRole role, string name, string description, string roleCode);

    /// <summary>
    /// Validates role business rules
    /// </summary>
    /// <param name="role">Role to validate</param>
    /// <returns>List of validation errors</returns>
    IEnumerable<string> ValidateRole(AppRole role);

    /// <summary>
    /// Validates if a permission can be granted to a role
    /// </summary>
    /// <param name="role">Target role</param>
    /// <param name="functionId">Function ID</param>
    /// <param name="actionId">Action ID</param>
    /// <returns>True if permission can be granted, false otherwise</returns>
    bool CanGrantPermission(AppRole role, string functionId, string actionId);

    /// <summary>
    /// Validates if a permission can be revoked from a role
    /// </summary>
    /// <param name="role">Target role</param>
    /// <param name="functionId">Function ID</param>
    /// <param name="actionId">Action ID</param>
    /// <returns>True if permission can be revoked, false otherwise</returns>
    bool CanRevokePermission(AppRole role, string functionId, string actionId);
}
