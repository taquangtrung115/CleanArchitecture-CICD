namespace DemoCICD.Application.Abstractions;

public interface IAiChatService
{
    Task<string> ProcessChatMessageAsync(string message, CancellationToken cancellationToken = default);
    Task<bool> CanPerformActionAsync(string action, CancellationToken cancellationToken = default);
}