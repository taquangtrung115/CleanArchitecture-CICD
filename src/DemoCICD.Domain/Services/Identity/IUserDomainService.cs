using DemoCICD.Domain.Entities.Identity;

namespace DemoCICD.Domain.Services.Identity;

public interface IUserDomainService
{
    /// <summary>
    /// Validates if a user can be created with the given parameters
    /// </summary>
    /// <param name="email">User email</param>
    /// <param name="firstName">First name</param>
    /// <param name="lastName">Last name</param>
    /// <param name="dayOfBirth">Date of birth</param>
    /// <returns>True if user can be created, false otherwise</returns>
    bool CanCreateUser(string email, string firstName, string lastName, DateTime? dayOfBirth);

    /// <summary>
    /// Validates if a user can be updated with the given parameters
    /// </summary>
    /// <param name="user">Current user</param>
    /// <param name="email">New email</param>
    /// <param name="firstName">New first name</param>
    /// <param name="lastName">New last name</param>
    /// <param name="dayOfBirth">New date of birth</param>
    /// <returns>True if user can be updated, false otherwise</returns>
    bool CanUpdateUser(AppUser user, string email, string firstName, string lastName, DateTime? dayOfBirth);

    /// <summary>
    /// Validates if a user can be deleted
    /// </summary>
    /// <param name="user">User to delete</param>
    /// <returns>True if user can be deleted, false otherwise</returns>
    bool CanDeleteUser(AppUser user);

    /// <summary>
    /// Validates if a user can be assigned to a position
    /// </summary>
    /// <param name="user">User to assign</param>
    /// <param name="position">Position to assign</param>
    /// <returns>True if user can be assigned, false otherwise</returns>
    bool CanAssignToPosition(AppUser user, Position position);

    /// <summary>
    /// Validates if a user can be assigned to a role
    /// </summary>
    /// <param name="user">User to assign</param>
    /// <param name="role">Role to assign</param>
    /// <returns>True if user can be assigned, false otherwise</returns>
    bool CanAssignToRole(AppUser user, AppRole role);

    /// <summary>
    /// Validates if a user's password can be changed
    /// </summary>
    /// <param name="user">User whose password will be changed</param>
    /// <param name="currentPassword">Current password</param>
    /// <param name="newPassword">New password</param>
    /// <returns>True if password can be changed, false otherwise</returns>
    bool CanChangePassword(AppUser user, string currentPassword, string newPassword);

    /// <summary>
    /// Validates if a user can be locked/unlocked
    /// </summary>
    /// <param name="user">User to lock/unlock</param>
    /// <param name="lockUser">True to lock, false to unlock</param>
    /// <returns>True if user can be locked/unlocked, false otherwise</returns>
    bool CanLockUser(AppUser user, bool lockUser);

    /// <summary>
    /// Creates a new user with business rules applied
    /// </summary>
    /// <param name="email">User email</param>
    /// <param name="firstName">First name</param>
    /// <param name="lastName">Last name</param>
    /// <param name="dayOfBirth">Date of birth</param>
    /// <returns>New user instance</returns>
    AppUser CreateUser(string email, string firstName, string lastName, DateTime? dayOfBirth);

    /// <summary>
    /// Updates an existing user with business rules applied
    /// </summary>
    /// <param name="user">User to update</param>
    /// <param name="email">New email</param>
    /// <param name="firstName">New first name</param>
    /// <param name="lastName">New last name</param>
    /// <param name="dayOfBirth">New date of birth</param>
    /// <param name="phone">New phone</param>
    /// <param name="address">New address</param>
    void UpdateUser(AppUser user, string email, string firstName, string lastName, DateTime? dayOfBirth, string? phone = null, string? address = null);

    /// <summary>
    /// Validates user business rules
    /// </summary>
    /// <param name="user">User to validate</param>
    /// <returns>List of validation errors</returns>
    IEnumerable<string> ValidateUser(AppUser user);

    /// <summary>
    /// Validates password strength
    /// </summary>
    /// <param name="password">Password to validate</param>
    /// <returns>List of validation errors</returns>
    IEnumerable<string> ValidatePassword(string password);
}
