using DemoCICD.Domain.Entities.Identity;

namespace DemoCICD.Domain.Services.Identity;

public class PositionDomainService : IPositionDomainService
{
    private const int MinPositionLevel = 1;
    private const int MaxPositionLevel = 10;
    private const int MaxNameLength = 200;
    private const int MaxCodeLength = 100;
    private const int MaxDescriptionLength = 1000;

    public bool CanCreatePosition(string name, string code, int level)
    {
        var validationErrors = ValidatePositionParameters(name, code, level);
        return !validationErrors.Any();
    }

    public bool CanUpdatePosition(Position position, string newName, string newCode, int newLevel)
    {
        if (position == null)
            return false;

        var validationErrors = ValidatePositionParameters(newName, newCode, newLevel);
        return !validationErrors.Any();
    }

    public bool CanDeletePosition(Position position, bool hasAssignedUsers)
    {
        if (position == null)
            return false;

        // Business rule: Cannot delete a position that has users assigned to it
        if (hasAssignedUsers)
            return false;

        // Business rule: Cannot delete inactive positions (they should be marked as inactive instead)
        // This is a soft delete approach
        return position.IsActive;
    }

    public Position CreatePosition(string name, string description, string code, int level)
    {
        var validationErrors = ValidatePositionParameters(name, code, level);
        if (validationErrors.Any())
        {
            throw new ArgumentException($"Invalid position parameters: {string.Join(", ", validationErrors)}");
        }

        return new Position
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Description = description?.Trim(),
            Code = code.Trim().ToUpper(), // Business rule: Position codes are always uppercase
            Level = level,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdatePosition(Position position, string name, string description, string code, int level, bool isActive)
    {
        if (position == null)
            throw new ArgumentNullException(nameof(position));

        var validationErrors = ValidatePositionParameters(name, code, level);
        if (validationErrors.Any())
        {
            throw new ArgumentException($"Invalid position parameters: {string.Join(", ", validationErrors)}");
        }

        position.Name = name.Trim();
        position.Description = description?.Trim();
        position.Code = code.Trim().ToUpper(); // Business rule: Position codes are always uppercase
        position.Level = level;
        position.IsActive = isActive;
        position.UpdatedAt = DateTime.UtcNow;
    }

    public IEnumerable<string> ValidatePosition(Position position)
    {
        if (position == null)
        {
            yield return "Position cannot be null";
            yield break;
        }

        foreach (var error in ValidatePositionParameters(position.Name, position.Code, position.Level))
        {
            yield return error;
        }

        // Additional business rules specific to existing positions
        if (position.Id == Guid.Empty)
            yield return "Position must have a valid ID";

        if (position.CreatedAt == default)
            yield return "Position must have a valid creation date";
    }

    private IEnumerable<string> ValidatePositionParameters(string name, string code, int level)
    {
        // Validate name
        if (string.IsNullOrWhiteSpace(name))
            yield return "Position name is required";
        else if (name.Trim().Length > MaxNameLength)
            yield return $"Position name cannot exceed {MaxNameLength} characters";

        // Validate code
        if (string.IsNullOrWhiteSpace(code))
            yield return "Position code is required";
        else if (code.Trim().Length > MaxCodeLength)
            yield return $"Position code cannot exceed {MaxCodeLength} characters";
        else if (!IsValidPositionCode(code.Trim()))
            yield return "Position code can only contain letters, numbers, and underscores";

        // Validate level
        if (level < MinPositionLevel || level > MaxPositionLevel)
            yield return $"Position level must be between {MinPositionLevel} and {MaxPositionLevel}";
    }

    private bool IsValidPositionCode(string code)
    {
        // Business rule: Position codes can only contain alphanumeric characters and underscores
        return !string.IsNullOrEmpty(code) && code.All(c => char.IsLetterOrDigit(c) || c == '_');
    }
}
