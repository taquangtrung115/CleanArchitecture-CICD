namespace DemoCICD.Application.Abstractions;

public interface IEmailService
{
    Task<bool> SendPasswordResetCodeAsync(string email, string code, CancellationToken cancellationToken = default);
}