using DemoCICD.Application.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Infrastructure.Email;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetToken, string userName)
    {
        try
        {
            // For demonstration purposes, we'll just log the email content
            // In a real implementation, you would use an email service like SendGrid, AWS SES, etc.
            
            var subject = "Password Reset Request";
            var body = GeneratePasswordResetEmailBody(userName, resetToken);
            
            _logger.LogInformation("Sending password reset email to {Email} for user {UserName}", toEmail, userName);
            _logger.LogInformation("Reset token: {Token}", resetToken);
            _logger.LogInformation("Email body: {Body}", body);
            
            // Simulate sending email
            await Task.Delay(100);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending password reset email to {Email}", toEmail);
            return false;
        }
    }

    public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            _logger.LogInformation("Sending email to {Email} with subject {Subject}", toEmail, subject);
            _logger.LogInformation("Email body: {Body}", body);
            
            // Simulate sending email
            await Task.Delay(100);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {Email}", toEmail);
            return false;
        }
    }

    private static string GeneratePasswordResetEmailBody(string userName, string resetToken)
    {
        return $@"
Dear {userName},

You have requested to reset your password. Please use the following token to reset your password:

Reset Token: {resetToken}

If you did not request this password reset, please ignore this email.

This token will expire in 1 hour for security reasons.

Best regards,
The CleanArchitecture Team";
    }
}