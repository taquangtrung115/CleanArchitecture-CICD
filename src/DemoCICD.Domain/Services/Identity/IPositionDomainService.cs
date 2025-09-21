using DemoCICD.Domain.Entities.Identity;

namespace DemoCICD.Domain.Services.Identity;

public interface IPositionDomainService
{
    /// <summary>
    /// Validates if a position can be created with the given parameters
    /// </summary>
    /// <param name="name">Position name</param>
    /// <param name="code">Position code</param>
    /// <param name="level">Position level</param>
    /// <returns>True if position can be created, false otherwise</returns>
    bool CanCreatePosition(string name, string code, int level);

    /// <summary>
    /// Validates if a position can be updated with the given parameters
    /// </summary>
    /// <param name="position">Current position</param>
    /// <param name="newName">New position name</param>
    /// <param name="newCode">New position code</param>
    /// <param name="newLevel">New position level</param>
    /// <returns>True if position can be updated, false otherwise</returns>
    bool CanUpdatePosition(Position position, string newName, string newCode, int newLevel);

    /// <summary>
    /// Validates if a position can be deleted
    /// </summary>
    /// <param name="position">Position to delete</param>
    /// <param name="hasAssignedUsers">Whether the position has users assigned to it</param>
    /// <returns>True if position can be deleted, false otherwise</returns>
    bool CanDeletePosition(Position position, bool hasAssignedUsers);

    /// <summary>
    /// Creates a new position with business rules applied
    /// </summary>
    /// <param name="name">Position name</param>
    /// <param name="description">Position description</param>
    /// <param name="code">Position code</param>
    /// <param name="level">Position level</param>
    /// <returns>New position instance</returns>
    Position CreatePosition(string name, string description, string code, int level);

    /// <summary>
    /// Updates an existing position with business rules applied
    /// </summary>
    /// <param name="position">Position to update</param>
    /// <param name="name">New name</param>
    /// <param name="description">New description</param>
    /// <param name="code">New code</param>
    /// <param name="level">New level</param>
    /// <param name="isActive">New active status</param>
    void UpdatePosition(Position position, string name, string description, string code, int level, bool isActive);

    /// <summary>
    /// Validates position business rules
    /// </summary>
    /// <param name="position">Position to validate</param>
    /// <returns>List of validation errors</returns>
    IEnumerable<string> ValidatePosition(Position position);
}
