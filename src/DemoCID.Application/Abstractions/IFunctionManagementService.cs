namespace DemoCICD.Application.Abstractions;

public interface IFunctionManagementService
{
    Task<IEnumerable<object>> GetActiveFunctionsAsync(CancellationToken cancellationToken = default);
    Task<(IEnumerable<object> Functions, int TotalCount)> GetFunctionsAsync(int page, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default);
    Task<object?> GetFunctionByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> FunctionExistsAsync(string id, CancellationToken cancellationToken = default);
}