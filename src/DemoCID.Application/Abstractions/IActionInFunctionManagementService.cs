namespace DemoCICD.Application.Abstractions;

public interface IActionInFunctionManagementService
{
    Task<(string ActionId, string FunctionId)?> CreateActionInFunctionAsync(string actionId, string functionId, CancellationToken cancellationToken = default);
    Task<bool> DeleteActionInFunctionAsync(string actionId, string functionId, CancellationToken cancellationToken = default);
    Task<object?> GetActionInFunctionAsync(string actionId, string functionId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<object> ActionInFunctions, int TotalCount)> GetActionInFunctionsAsync(int page, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default);
    Task<bool> ActionInFunctionExistsAsync(string actionId, string functionId, CancellationToken cancellationToken = default);
}