using DemoCICD.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace DemoCICD.Infrastructure.Email;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> SendPasswordResetCodeAsync(string email, string code, CancellationToken cancellationToken = default)
    {
        try
        {
            // For now, this is a mock implementation
            // In a real application, you would integrate with an email service like SendGrid, SES, etc.
            _logger.LogInformation("Sending password reset code {Code} to email: {Email}", code, email);
            
            // Simulate sending email
            await Task.Delay(100, cancellationToken);
            
            // For demo purposes, always return true
            // In production, you would handle actual email sending and return the result
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset email to {Email}", email);
            return false;
        }
    }
}