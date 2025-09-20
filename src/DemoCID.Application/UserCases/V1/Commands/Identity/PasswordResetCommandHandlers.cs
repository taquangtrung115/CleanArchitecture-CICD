using DemoCICD.Application.Abstractions;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using Microsoft.Extensions.Logging;

namespace DemoCICD.Application.UserCases.V1.Commands.Identity;

public sealed class ForgotPasswordCommandHandler : ICommandHandler<Command.ForgotPassword, Response.ForgotPasswordSent>
{
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly IPasswordResetService _passwordResetService;
    private readonly IEmailService _emailService;
    private readonly ILogger<ForgotPasswordCommandHandler> _logger;

    public ForgotPasswordCommandHandler(
        IUserAuthenticationService userAuthenticationService,
        IPasswordResetService passwordResetService,
        IEmailService emailService,
        ILogger<ForgotPasswordCommandHandler> logger)
    {
        _userAuthenticationService = userAuthenticationService;
        _passwordResetService = passwordResetService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<Result<Response.ForgotPasswordSent>> Handle(Command.ForgotPassword request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if user exists with the provided email
            var userExists = await _userAuthenticationService.UserExistsByEmailAsync(request.Email);
            if (!userExists)
            {
                // For security reasons, we still return success even if user doesn't exist
                // This prevents email enumeration attacks
                _logger.LogWarning("Password reset requested for non-existent email: {Email}", request.Email);
                return Result.Success(new Response.ForgotPasswordSent(request.Email, "If an account with this email exists, a reset code has been sent."));
            }

            // Generate reset code
            var resetCode = await _passwordResetService.GenerateResetCodeAsync(request.Email, cancellationToken);

            // Send email with reset code
            var emailSent = await _emailService.SendPasswordResetCodeAsync(request.Email, resetCode, cancellationToken);

            if (!emailSent)
            {
                _logger.LogError("Failed to send password reset email to {Email}", request.Email);
                return Result.Failure<Response.ForgotPasswordSent>(new Error("Email.SendFailed", "Failed to send reset email"));
            }

            _logger.LogInformation("Password reset code sent to email: {Email}", request.Email);
            return Result.Success(new Response.ForgotPasswordSent(request.Email, "A password reset code has been sent to your email."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during forgot password for email: {Email}", request.Email);
            return Result.Failure<Response.ForgotPasswordSent>(new Error("ForgotPassword.Error", "An error occurred while processing your request"));
        }
    }
}

public sealed class VerifyResetCodeCommandHandler : ICommandHandler<Command.VerifyResetCode, Response.ResetCodeVerified>
{
    private readonly IPasswordResetService _passwordResetService;
    private readonly ILogger<VerifyResetCodeCommandHandler> _logger;

    public VerifyResetCodeCommandHandler(
        IPasswordResetService passwordResetService,
        ILogger<VerifyResetCodeCommandHandler> logger)
    {
        _passwordResetService = passwordResetService;
        _logger = logger;
    }

    public async Task<Result<Response.ResetCodeVerified>> Handle(Command.VerifyResetCode request, CancellationToken cancellationToken)
    {
        try
        {
            var isValid = await _passwordResetService.VerifyResetCodeAsync(request.Email, request.Code, cancellationToken);

            if (isValid)
            {
                _logger.LogInformation("Reset code verified successfully for email: {Email}", request.Email);
            }
            else
            {
                _logger.LogWarning("Invalid reset code provided for email: {Email}", request.Email);
            }

            return Result.Success(new Response.ResetCodeVerified(request.Email, isValid));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during reset code verification for email: {Email}", request.Email);
            return Result.Failure<Response.ResetCodeVerified>(new Error("VerifyCode.Error", "An error occurred while verifying the reset code"));
        }
    }
}

public sealed class ResetPasswordWithCodeCommandHandler : ICommandHandler<Command.ResetPasswordWithCode>
{
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly IPasswordResetService _passwordResetService;
    private readonly ILogger<ResetPasswordWithCodeCommandHandler> _logger;

    public ResetPasswordWithCodeCommandHandler(
        IUserAuthenticationService userAuthenticationService,
        IPasswordResetService passwordResetService,
        ILogger<ResetPasswordWithCodeCommandHandler> logger)
    {
        _userAuthenticationService = userAuthenticationService;
        _passwordResetService = passwordResetService;
        _logger = logger;
    }

    public async Task<Result> Handle(Command.ResetPasswordWithCode request, CancellationToken cancellationToken)
    {
        try
        {
            // Verify the reset code first
            var isCodeValid = await _passwordResetService.VerifyResetCodeAsync(request.Email, request.Code, cancellationToken);
            if (!isCodeValid)
            {
                _logger.LogWarning("Invalid or expired reset code for email: {Email}", request.Email);
                return Result.Failure(new Error("ResetCode.Invalid", "Invalid or expired reset code"));
            }

            // Reset the password
            var resetSuccess = await _userAuthenticationService.ResetPasswordByEmailAsync(request.Email, request.NewPassword);
            if (!resetSuccess)
            {
                _logger.LogError("Failed to reset password for email: {Email}", request.Email);
                return Result.Failure(new Error("PasswordReset.Failed", "Failed to reset password"));
            }

            // Invalidate the reset code
            await _passwordResetService.InvalidateResetCodeAsync(request.Email, cancellationToken);

            _logger.LogInformation("Password reset successfully for email: {Email}", request.Email);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset with code for email: {Email}", request.Email);
            return Result.Failure(new Error("ResetPasswordWithCode.Error", "An error occurred while resetting the password"));
        }
    }
}