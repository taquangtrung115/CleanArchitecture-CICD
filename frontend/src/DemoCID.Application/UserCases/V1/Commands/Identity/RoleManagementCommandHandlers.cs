using DemoCICD.Application.Abstractions;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using Serilog;

namespace DemoCICD.Application.UserCases.V1.Commands.Identity;

public sealed class CreateRoleCommandHandler : ICommandHandler<Command.CreateRole, Response.RoleCreated>
{
    private readonly IRoleManagementService _roleManagementService;

    public CreateRoleCommandHandler(
        IRoleManagementService roleManagementService)
    {
        _roleManagementService = roleManagementService;
    }

    public async Task<Result<Response.RoleCreated>> Handle(Command.CreateRole request, CancellationToken cancellationToken)
    {
        try
        {
            var role = await _roleManagementService.CreateRoleAsync(
                request.Name,
                request.Description,
                request.RoleCode);

            Log.Information("Role {RoleName} created successfully with ID {RoleId}", request.Name, role.Id);

            var response = new Response.RoleCreated(
                role.Id,
                role.Name!,
                role.RoleCode);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during role creation for role: {RoleName}", request.Name);
            return Result.Failure<Response.RoleCreated>(
                new Error("RoleCreation.Error", "An error occurred during role creation"));
        }
    }
}

public sealed class UpdateRoleCommandHandler : ICommandHandler<Command.UpdateRole, Response.RoleUpdated>
{
    private readonly IRoleManagementService _roleManagementService;

    public UpdateRoleCommandHandler(
        IRoleManagementService roleManagementService)
    {
        _roleManagementService = roleManagementService;
    }

    public async Task<Result<Response.RoleUpdated>> Handle(Command.UpdateRole request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _roleManagementService.UpdateRoleAsync(
                request.RoleId,
                request.Name,
                request.Description,
                request.RoleCode);

            if (!success)
            {
                return Result.Failure<Response.RoleUpdated>(
                    new Error("RoleUpdate.Failed", "Role update failed"));
            }

            Log.Information("Role {RoleId} updated successfully", request.RoleId);

            var response = new Response.RoleUpdated(
                request.RoleId,
                request.Name,
                request.RoleCode);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during role update for role: {RoleId}", request.RoleId);
            return Result.Failure<Response.RoleUpdated>(
                new Error("RoleUpdate.Error", "An error occurred during role update"));
        }
    }
}

public sealed class DeleteRoleCommandHandler : ICommandHandler<Command.DeleteRole>
{
    private readonly IRoleManagementService _roleManagementService;

    public DeleteRoleCommandHandler(
        IRoleManagementService roleManagementService)
    {
        _roleManagementService = roleManagementService;
    }

    public async Task<Result> Handle(Command.DeleteRole request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _roleManagementService.DeleteRoleAsync(request.RoleId);

            if (!success)
            {
                return Result.Failure(new Error("RoleDeletion.Failed", "Role deletion failed"));
            }

            Log.Information("Role {RoleId} deleted successfully", request.RoleId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during role deletion for role: {RoleId}", request.RoleId);
            return Result.Failure(new Error("RoleDeletion.Error", "An error occurred during role deletion"));
        }
    }
}

public sealed class GrantPermissionToRoleCommandHandler : ICommandHandler<Command.GrantPermissionToRole>
{
    private readonly IRoleManagementService _roleManagementService;

    public GrantPermissionToRoleCommandHandler(
        IRoleManagementService roleManagementService)
    {
        _roleManagementService = roleManagementService;
    }

    public async Task<Result> Handle(Command.GrantPermissionToRole request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _roleManagementService.GrantPermissionToRoleAsync(
                request.RoleId,
                request.FunctionId,
                request.ActionId);

            if (!success)
            {
                return Result.Failure(new Error("PermissionGrant.Failed", "Permission grant failed"));
            }

            Log.Information("Permission granted to role {RoleId} for function {FunctionId} and action {ActionId}", 
                request.RoleId, request.FunctionId, request.ActionId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during permission grant for role: {RoleId}, function: {FunctionId}, action: {ActionId}", 
                request.RoleId, request.FunctionId, request.ActionId);
            return Result.Failure(new Error("PermissionGrant.Error", "An error occurred during permission grant"));
        }
    }
}

public sealed class RevokePermissionFromRoleCommandHandler : ICommandHandler<Command.RevokePermissionFromRole>
{
    private readonly IRoleManagementService _roleManagementService;

    public RevokePermissionFromRoleCommandHandler(
        IRoleManagementService roleManagementService)
    {
        _roleManagementService = roleManagementService;
    }

    public async Task<Result> Handle(Command.RevokePermissionFromRole request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _roleManagementService.RevokePermissionFromRoleAsync(
                request.RoleId,
                request.FunctionId,
                request.ActionId);

            if (!success)
            {
                return Result.Failure(new Error("PermissionRevoke.Failed", "Permission revoke failed"));
            }

            Log.Information("Permission revoked from role {RoleId} for function {FunctionId} and action {ActionId}", 
                request.RoleId, request.FunctionId, request.ActionId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during permission revoke for role: {RoleId}, function: {FunctionId}, action: {ActionId}", 
                request.RoleId, request.FunctionId, request.ActionId);
            return Result.Failure(new Error("PermissionRevoke.Error", "An error occurred during permission revoke"));
        }
    }
}

// For other command handlers (DeleteRoleCommandHandler, GrantPermissionToRoleCommandHandler, RevokePermissionFromRoleCommandHandler),
// remove ILogger fields/parameters and use Log.Error/Log.Information as above.
