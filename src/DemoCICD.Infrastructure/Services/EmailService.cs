using DemoCICD.Application.Abstractions;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace DemoCICD.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IConfiguration _configuration;

    public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<bool> SendPasswordResetCodeAsync(string email, string resetCode, string userName)
    {
        var subject = "Password Reset Code";
        var body = $@"
            <html>
            <body>
                <h2>Password Reset Request</h2>
                <p>Hello {userName},</p>
                <p>You have requested to reset your password. Please use the following verification code:</p>
                <h3 style='color: #007bff; font-size: 24px; letter-spacing: 3px;'>{resetCode}</h3>
                <p>This code will expire in 15 minutes.</p>
                <p>If you did not request this password reset, please ignore this email.</p>
                <br>
                <p>Best regards,<br>DemoCICD Team</p>
            </body>
            </html>";

        return await SendEmailAsync(email, subject, body);
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            // For demo purposes, we'll log the email instead of actually sending it
            // In production, you would configure SMTP settings and send real emails
            _logger.LogInformation("Email would be sent to: {Email}", to);
            _logger.LogInformation("Subject: {Subject}", subject);
            _logger.LogInformation("Body: {Body}", body);
            
            // Simulate email sending delay
            await Task.Delay(100);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to: {Email}", to);
            return false;
        }
    }
}