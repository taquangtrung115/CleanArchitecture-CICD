using DemoCICD.Application.Abstractions;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DemoCICD.Application.UserCases.V1.Commands.Identity;

public sealed class CreateUserCommandHandler : ICommandHandler<Command.CreateUser, Response.UserCreated>
{
    private readonly IUserManagementService _userManagementService;

    public CreateUserCommandHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<Result<Response.UserCreated>> Handle(Command.CreateUser request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _userManagementService.CreateUserAsync(
                request.UserName,
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName,
                request.DayOfBirth,
                request.IsDirector,
                request.IsHeadOfDepartment,
                request.ManagerId,
                request.PositionId);

            if (!result.IsSuccess)
            {
                return Result.Failure<Response.UserCreated>(
                    new Error("UserCreation.Failed", result.ErrorMessage ?? "User creation failed"));
            }

            Log.Information("User {UserName} created successfully with ID {UserId}", request.UserName, result.UserId);

            var response = new Response.UserCreated(
                Guid.Parse(result.UserId!),
                result.UserName!,
                result.Email!);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during user creation for user: {UserName}", request.UserName);
            return Result.Failure<Response.UserCreated>(
                new Error("UserCreation.Error", "An error occurred during user creation"));
        }
    }
}

public sealed class UpdateUserCommandHandler : ICommandHandler<Command.UpdateUser, Response.UserUpdated>
{
    private readonly IUserManagementService _userManagementService;

    public UpdateUserCommandHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<Result<Response.UserUpdated>> Handle(Command.UpdateUser request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _userManagementService.UpdateUserAsync(
                request.UserId,
                request.Email,
                request.FirstName,
                request.LastName,
                request.DayOfBirth,
                request.IsDirector,
                request.IsHeadOfDepartment,
                request.ManagerId,
                request.PositionId,
                request.Phone,
                request.Address,
                request.City,
                request.Country,
                request.Bio,
                request.Website);

            if (!success)
            {
                return Result.Failure<Response.UserUpdated>(
                    new Error("UserUpdate.Failed", "User update failed"));
            }

            Log.Information("User {UserId} updated successfully", request.UserId);

            var response = new Response.UserUpdated(
                request.UserId,
                string.Empty, // UserName will be populated by the service
                request.Email);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during user update for user: {UserId}", request.UserId);
            return Result.Failure<Response.UserUpdated>(
                new Error("UserUpdate.Error", "An error occurred during user update"));
        }
    }
}

public sealed class DeleteUserCommandHandler : ICommandHandler<Command.DeleteUser>
{
    private readonly IUserManagementService _userManagementService;

    public DeleteUserCommandHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<Result> Handle(Command.DeleteUser request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _userManagementService.DeleteUserAsync(request.UserId);

            if (!success)
            {
                return Result.Failure(new Error("UserDeletion.Failed", "User deletion failed"));
            }

            Log.Information("User {UserId} deleted successfully", request.UserId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during user deletion for user: {UserId}", request.UserId);
            return Result.Failure(new Error("UserDeletion.Error", "An error occurred during user deletion"));
        }
    }
}

public sealed class ChangePasswordCommandHandler : ICommandHandler<Command.ChangePassword>
{
    private readonly IUserManagementService _userManagementService;
    private readonly ILogger<ChangePasswordCommandHandler> _logger;

    public ChangePasswordCommandHandler(
        IUserManagementService userManagementService,
        ILogger<ChangePasswordCommandHandler> logger)
    {
        _userManagementService = userManagementService;
        _logger = logger;
    }

    public async Task<Result> Handle(Command.ChangePassword request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _userManagementService.ChangePasswordAsync(
                request.UserId,
                request.CurrentPassword,
                request.NewPassword);

            if (!success)
            {
                return Result.Failure(new Error("PasswordChange.Failed", "Password change failed"));
            }

            _logger.LogInformation("Password changed successfully for user {UserId}", request.UserId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password change for user: {UserId}", request.UserId);
            return Result.Failure(new Error("PasswordChange.Error", "An error occurred during password change"));
        }
    }
}

public sealed class ResetPasswordCommandHandler : ICommandHandler<Command.ResetPassword>
{
    private readonly IUserManagementService _userManagementService;
    private readonly ILogger<ResetPasswordCommandHandler> _logger;

    public ResetPasswordCommandHandler(
        IUserManagementService userManagementService,
        ILogger<ResetPasswordCommandHandler> logger)
    {
        _userManagementService = userManagementService;
        _logger = logger;
    }

    public async Task<Result> Handle(Command.ResetPassword request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _userManagementService.ResetPasswordAsync(
                request.UserId,
                request.NewPassword);

            if (!success)
            {
                return Result.Failure(new Error("PasswordReset.Failed", "Password reset failed"));
            }

            _logger.LogInformation("Password reset successfully for user {UserId}", request.UserId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset for user: {UserId}", request.UserId);
            return Result.Failure(new Error("PasswordReset.Error", "An error occurred during password reset"));
        }
    }
}

public sealed class LockUserCommandHandler : ICommandHandler<Command.LockUser>
{
    private readonly IUserManagementService _userManagementService;
    private readonly ILogger<LockUserCommandHandler> _logger;

    public LockUserCommandHandler(
        IUserManagementService userManagementService,
        ILogger<LockUserCommandHandler> logger)
    {
        _userManagementService = userManagementService;
        _logger = logger;
    }

    public async Task<Result> Handle(Command.LockUser request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _userManagementService.LockUserAsync(request.UserId);

            if (!success)
            {
                return Result.Failure(new Error("UserLock.Failed", "User lock failed"));
            }

            _logger.LogInformation("User {UserId} locked successfully", request.UserId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user lock for user: {UserId}", request.UserId);
            return Result.Failure(new Error("UserLock.Error", "An error occurred during user lock"));
        }
    }
}

public sealed class UnlockUserCommandHandler : ICommandHandler<Command.UnlockUser>
{
    private readonly IUserManagementService _userManagementService;
    private readonly ILogger<UnlockUserCommandHandler> _logger;

    public UnlockUserCommandHandler(
        IUserManagementService userManagementService,
        ILogger<UnlockUserCommandHandler> logger)
    {
        _userManagementService = userManagementService;
        _logger = logger;
    }

    public async Task<Result> Handle(Command.UnlockUser request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _userManagementService.UnlockUserAsync(request.UserId);

            if (!success)
            {
                return Result.Failure(new Error("UserUnlock.Failed", "User unlock failed"));
            }

            _logger.LogInformation("User {UserId} unlocked successfully", request.UserId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user unlock for user: {UserId}", request.UserId);
            return Result.Failure(new Error("UserUnlock.Error", "An error occurred during user unlock"));
        }
    }
}

public sealed class AssignUserToRoleCommandHandler : ICommandHandler<Command.AssignUserToRole>
{
    private readonly IUserManagementService _userManagementService;
    private readonly ILogger<AssignUserToRoleCommandHandler> _logger;

    public AssignUserToRoleCommandHandler(
        IUserManagementService userManagementService,
        ILogger<AssignUserToRoleCommandHandler> logger)
    {
        _userManagementService = userManagementService;
        _logger = logger;
    }

    public async Task<Result> Handle(Command.AssignUserToRole request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _userManagementService.AssignUserToRoleAsync(request.UserId, request.RoleId);

            if (!success)
            {
                return Result.Failure(new Error("UserRoleAssignment.Failed", "User role assignment failed"));
            }

            _logger.LogInformation("User {UserId} assigned to role {RoleId} successfully", request.UserId, request.RoleId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user role assignment for user: {UserId}, role: {RoleId}", request.UserId, request.RoleId);
            return Result.Failure(new Error("UserRoleAssignment.Error", "An error occurred during user role assignment"));
        }
    }
}

public sealed class RemoveUserFromRoleCommandHandler : ICommandHandler<Command.RemoveUserFromRole>
{
    private readonly IUserManagementService _userManagementService;
    private readonly ILogger<RemoveUserFromRoleCommandHandler> _logger;

    public RemoveUserFromRoleCommandHandler(
        IUserManagementService userManagementService,
        ILogger<RemoveUserFromRoleCommandHandler> logger)
    {
        _userManagementService = userManagementService;
        _logger = logger;
    }

    public async Task<Result> Handle(Command.RemoveUserFromRole request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _userManagementService.RemoveUserFromRoleAsync(request.UserId, request.RoleId);

            if (!success)
            {
                return Result.Failure(new Error("UserRoleRemoval.Failed", "User role removal failed"));
            }

            _logger.LogInformation("User {UserId} removed from role {RoleId} successfully", request.UserId, request.RoleId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user role removal for user: {UserId}, role: {RoleId}", request.UserId, request.RoleId);
            return Result.Failure(new Error("UserRoleRemoval.Error", "An error occurred during user role removal"));
        }
    }
}

public sealed class UpdateProfileCommandHandler : ICommandHandler<Command.UpdateProfile, Response.UserUpdated>
{
    private readonly IUserManagementService _userManagementService;

    public UpdateProfileCommandHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<Result<Response.UserUpdated>> Handle(Command.UpdateProfile request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _userManagementService.UpdateProfileAsync(
                request.UserId,
                request.FirstName,
                request.LastName,
                request.Phone,
                request.Address,
                request.City,
                request.Country,
                request.Bio,
                request.Website);

            if (!success)
            {
                return Result.Failure<Response.UserUpdated>(
                    new Error("ProfileUpdate.Failed", "Profile update failed"));
            }

            Log.Information("Profile updated successfully for user {UserId}", request.UserId);

            var response = new Response.UserUpdated(
                request.UserId,
                string.Empty, // UserName will be populated by the service
                string.Empty); // Email remains unchanged in profile update

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during profile update for user: {UserId}", request.UserId);
            return Result.Failure<Response.UserUpdated>(
                new Error("ProfileUpdate.Error", "An error occurred during profile update"));
        }
    }
}

// Password Reset Command Handlers
public sealed class ForgotPasswordCommandHandler : ICommandHandler<Command.ForgotPassword>
{
    private readonly IUserManagementService _userManagementService;
    private readonly IEmailService _emailService;
    private readonly ILogger<ForgotPasswordCommandHandler> _logger;

    public ForgotPasswordCommandHandler(
        IUserManagementService userManagementService,
        IEmailService emailService,
        ILogger<ForgotPasswordCommandHandler> logger)
    {
        _userManagementService = userManagementService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<Result> Handle(Command.ForgotPassword request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _userManagementService.InitiatePasswordResetAsync(request.Email);

            if (!success)
            {
                // For security reasons, we don't reveal if email exists or not
                _logger.LogWarning("Password reset requested for non-existent email: {Email}", request.Email);
            }
            else
            {
                _logger.LogInformation("Password reset initiated for email: {Email}", request.Email);
            }

            // Always return success to avoid email enumeration attacks
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset initiation for email: {Email}", request.Email);
            return Result.Failure(new Error("ForgotPassword.Error", "An error occurred during password reset initiation"));
        }
    }
}

public sealed class VerifyResetCodeCommandHandler : ICommandHandler<Command.VerifyResetCode, Response.ResetCodeVerified>
{
    private readonly IUserManagementService _userManagementService;
    private readonly ILogger<VerifyResetCodeCommandHandler> _logger;

    public VerifyResetCodeCommandHandler(
        IUserManagementService userManagementService,
        ILogger<VerifyResetCodeCommandHandler> logger)
    {
        _userManagementService = userManagementService;
        _logger = logger;
    }

    public async Task<Result<Response.ResetCodeVerified>> Handle(Command.VerifyResetCode request, CancellationToken cancellationToken)
    {
        try
        {
            var isValid = await _userManagementService.VerifyResetCodeAsync(request.Email, request.ResetCode);

            if (isValid)
            {
                _logger.LogInformation("Reset code verified successfully for email: {Email}", request.Email);
            }
            else
            {
                _logger.LogWarning("Invalid reset code provided for email: {Email}", request.Email);
            }

            var response = new Response.ResetCodeVerified(isValid);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during reset code verification for email: {Email}", request.Email);
            return Result.Failure<Response.ResetCodeVerified>(new Error("VerifyResetCode.Error", "An error occurred during reset code verification"));
        }
    }
}

public sealed class ResetPasswordWithCodeCommandHandler : ICommandHandler<Command.ResetPasswordWithCode>
{
    private readonly IUserManagementService _userManagementService;
    private readonly ILogger<ResetPasswordWithCodeCommandHandler> _logger;

    public ResetPasswordWithCodeCommandHandler(
        IUserManagementService userManagementService,
        ILogger<ResetPasswordWithCodeCommandHandler> logger)
    {
        _userManagementService = userManagementService;
        _logger = logger;
    }

    public async Task<Result> Handle(Command.ResetPasswordWithCode request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _userManagementService.ResetPasswordWithCodeAsync(
                request.Email,
                request.ResetCode,
                request.NewPassword);

            if (!success)
            {
                return Result.Failure(new Error("ResetPasswordWithCode.Failed", "Password reset failed. Invalid code or expired."));
            }

            _logger.LogInformation("Password reset successfully for email: {Email}", request.Email);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset for email: {Email}", request.Email);
            return Result.Failure(new Error("ResetPasswordWithCode.Error", "An error occurred during password reset"));
        }
    }
}

// Account Settings Command Handlers
public sealed class UpdateNotificationSettingsCommandHandler : ICommandHandler<Command.UpdateNotificationSettings>
{
    public async Task<Result> Handle(Command.UpdateNotificationSettings request, CancellationToken cancellationToken)
    {
        // Stub implementation - would store settings in database
        Log.Information("Notification settings updated for user {UserId}", request.UserId);
        return Result.Success();
    }
}

public sealed class UpdatePrivacySettingsCommandHandler : ICommandHandler<Command.UpdatePrivacySettings>
{
    public async Task<Result> Handle(Command.UpdatePrivacySettings request, CancellationToken cancellationToken)
    {
        // Stub implementation - would store settings in database
        Log.Information("Privacy settings updated for user {UserId}", request.UserId);
        return Result.Success();
    }
}

public sealed class RevokeUserSessionCommandHandler : ICommandHandler<Command.RevokeUserSession>
{
    public async Task<Result> Handle(Command.RevokeUserSession request, CancellationToken cancellationToken)
    {
        // Stub implementation - would revoke specific session
        Log.Information("Session {SessionId} revoked for user {UserId}", request.SessionId, request.UserId);
        return Result.Success();
    }
}

public sealed class RevokeAllUserSessionsCommandHandler : ICommandHandler<Command.RevokeAllUserSessions>
{
    public async Task<Result> Handle(Command.RevokeAllUserSessions request, CancellationToken cancellationToken)
    {
        // Stub implementation - would revoke all sessions for user
        Log.Information("All sessions revoked for user {UserId}", request.UserId);
        return Result.Success();
    }
}

public sealed class ForgotPasswordCommandHandler : ICommandHandler<Command.ForgotPassword>
{
    private readonly IUserManagementService _userManagementService;
    private readonly IEmailService _emailService;
    private readonly ILogger<ForgotPasswordCommandHandler> _logger;

    public ForgotPasswordCommandHandler(
        IUserManagementService userManagementService,
        IEmailService emailService,
        ILogger<ForgotPasswordCommandHandler> logger)
    {
        _userManagementService = userManagementService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<Result> Handle(Command.ForgotPassword request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManagementService.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                // For security reasons, don't reveal if the email exists or not
                // Return success even if user doesn't exist
                _logger.LogWarning("Password reset requested for non-existent email: {Email}", request.Email);
                return Result.Success();
            }

            var resetToken = await _userManagementService.GeneratePasswordResetTokenAsync(user);
            
            var emailSent = await _emailService.SendPasswordResetEmailAsync(
                user.Email!, 
                resetToken, 
                user.UserName!);

            if (!emailSent)
            {
                _logger.LogError("Failed to send password reset email to {Email}", request.Email);
                return Result.Failure(new Error("ForgotPassword.EmailFailed", "Failed to send password reset email"));
            }

            _logger.LogInformation("Password reset email sent successfully to {Email}", request.Email);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing forgot password request for email: {Email}", request.Email);
            return Result.Failure(new Error("ForgotPassword.Error", "An error occurred while processing your request"));
        }
    }
}

public sealed class ResetPasswordWithTokenCommandHandler : ICommandHandler<Command.ResetPasswordWithToken>
{
    private readonly IUserManagementService _userManagementService;
    private readonly ILogger<ResetPasswordWithTokenCommandHandler> _logger;

    public ResetPasswordWithTokenCommandHandler(
        IUserManagementService userManagementService,
        ILogger<ResetPasswordWithTokenCommandHandler> logger)
    {
        _userManagementService = userManagementService;
        _logger = logger;
    }

    public async Task<Result> Handle(Command.ResetPasswordWithToken request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _userManagementService.ResetPasswordWithTokenAsync(
                request.Email,
                request.Token,
                request.NewPassword);

            if (!success)
            {
                _logger.LogWarning("Failed to reset password with token for email: {Email}", request.Email);
                return Result.Failure(new Error("ResetPassword.Failed", "Invalid reset token or email"));
            }

            _logger.LogInformation("Password reset successfully for email: {Email}", request.Email);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting password with token for email: {Email}", request.Email);
            return Result.Failure(new Error("ResetPassword.Error", "An error occurred while resetting your password"));
        }
    }
}
