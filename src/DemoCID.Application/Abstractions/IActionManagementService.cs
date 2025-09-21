namespace DemoCICD.Application.Abstractions;

public interface IActionManagementService
{
    Task<(string Id, string Name)?> CreateActionAsync(string id, string name, int? sortOrder = null, bool? isActive = true, CancellationToken cancellationToken = default);
    Task<(string Id, string Name)?> UpdateActionAsync(string id, string name, int? sortOrder = null, bool? isActive = true, CancellationToken cancellationToken = default);
    Task<bool> DeleteActionAsync(string id, CancellationToken cancellationToken = default);
    Task<object?> GetActionByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<(IEnumerable<object> Actions, int TotalCount)> GetActionsAsync(int page, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<object>> GetActiveActionsAsync(CancellationToken cancellationToken = default);
    Task<bool> ActionExistsAsync(string id, CancellationToken cancellationToken = default);
}