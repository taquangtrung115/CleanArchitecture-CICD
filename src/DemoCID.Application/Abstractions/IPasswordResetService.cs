namespace DemoCICD.Application.Abstractions;

public interface IPasswordResetService
{
    Task<string> GenerateResetCodeAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> VerifyResetCodeAsync(string email, string code, CancellationToken cancellationToken = default);
    Task<bool> InvalidateResetCodeAsync(string email, CancellationToken cancellationToken = default);
}