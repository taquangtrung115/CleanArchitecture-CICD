namespace DemoCICD.Application.Abstractions;

public interface IPositionManagementService
{
    Task<(Guid PositionId, string Name, string Code)?> CreatePositionAsync(string name, string description, string code, int level = 1, CancellationToken cancellationToken = default);
    Task<(Guid PositionId, string Name, string Code)?> UpdatePositionAsync(Guid positionId, string name, string description, string code, int level, bool isActive, CancellationToken cancellationToken = default);
    Task<bool> DeletePositionAsync(Guid positionId, CancellationToken cancellationToken = default);
    Task<object?> GetPositionByIdAsync(Guid positionId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<object> Positions, int TotalCount)> GetPositionsAsync(int page, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<object>> GetActivePositionsAsync(CancellationToken cancellationToken = default);
    Task<bool> PositionExistsAsync(Guid positionId, CancellationToken cancellationToken = default);
    Task<bool> PositionCodeExistsAsync(string code, Guid? excludePositionId = null, CancellationToken cancellationToken = default);
}