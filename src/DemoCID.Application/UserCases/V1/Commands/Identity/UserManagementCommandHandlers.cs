using DemoCICD.Application.Abstractions;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using Microsoft.Extensions.Logging;

namespace DemoCICD.Application.UserCases.V1.Commands.Identity;

public class CreateUserCommandHandler : ICommandHandler<Command.CreateUser, Response.UserCreated>
{
    private readonly IUserManagementService _userManagementService;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(
        IUserManagementService userManagementService,
        ILogger<CreateUserCommandHandler> logger)
    {
        _userManagementService = userManagementService;
        _logger = logger;
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

            _logger.LogInformation("User {UserName} created successfully with ID {UserId}", request.UserName, result.UserId);

            var response = new Response.UserCreated(
                Guid.Parse(result.UserId!),
                result.UserName!,
                result.Email!);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user creation for user: {UserName}", request.UserName);
            return Result.Failure<Response.UserCreated>(
                new Error("UserCreation.Error", "An error occurred during user creation"));
        }
    }
}

public class UpdateUserCommandHandler : ICommandHandler<Command.UpdateUser, Response.UserUpdated>
{
    private readonly IUserManagementService _userManagementService;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(
        IUserManagementService userManagementService,
        ILogger<UpdateUserCommandHandler> logger)
    {
        _userManagementService = userManagementService;
        _logger = logger;
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
                request.PositionId);

            if (!success)
            {
                return Result.Failure<Response.UserUpdated>(
                    new Error("UserUpdate.Failed", "User update failed"));
            }

            _logger.LogInformation("User {UserId} updated successfully", request.UserId);

            var response = new Response.UserUpdated(
                request.UserId,
                string.Empty, // UserName will be populated by the service
                request.Email);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user update for user: {UserId}", request.UserId);
            return Result.Failure<Response.UserUpdated>(
                new Error("UserUpdate.Error", "An error occurred during user update"));
        }
    }
}

public sealed class DeleteUserCommandHandler : ICommandHandler<Command.DeleteUser>
{
    private readonly IUserManagementService _userManagementService;
    private readonly ILogger<DeleteUserCommandHandler> _logger;

    public DeleteUserCommandHandler(
        IUserManagementService userManagementService,
        ILogger<DeleteUserCommandHandler> logger)
    {
        _userManagementService = userManagementService;
        _logger = logger;
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

            _logger.LogInformation("User {UserId} deleted successfully", request.UserId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user deletion for user: {UserId}", request.UserId);
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
