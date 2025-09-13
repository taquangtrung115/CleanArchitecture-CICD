using DemoCICD.Application.Abstractions;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using Microsoft.Extensions.Logging;

namespace DemoCICD.Application.UserCases.V1.Commands.Identity;

public sealed class CreatePermissionCommandHandler : ICommandHandler<Command.CreatePermission, Response.PermissionCreated>
{
    private readonly IPermissionManagementService _permissionManagementService;
    private readonly ILogger<CreatePermissionCommandHandler> _logger;

    public CreatePermissionCommandHandler(
        IPermissionManagementService permissionManagementService,
        ILogger<CreatePermissionCommandHandler> logger)
    {
        _permissionManagementService = permissionManagementService;
        _logger = logger;
    }

    public async Task<Result<Response.PermissionCreated>> Handle(Command.CreatePermission request, CancellationToken cancellationToken)
    {
        try
        {
            var permission = await _permissionManagementService.CreatePermissionAsync(
                request.RoleId,
                request.FunctionId,
                request.ActionId);

            _logger.LogInformation("Permission created successfully for role {RoleId}, function {FunctionId}, action {ActionId}", 
                request.RoleId, request.FunctionId, request.ActionId);

            var response = new Response.PermissionCreated(
                permission.RoleId,
                permission.FunctionId,
                permission.ActionId);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during permission creation for role: {RoleId}, function: {FunctionId}, action: {ActionId}", 
                request.RoleId, request.FunctionId, request.ActionId);
            return Result.Failure<Response.PermissionCreated>(
                new Error("PermissionCreation.Error", "An error occurred during permission creation"));
        }
    }
}

public sealed class DeletePermissionCommandHandler : ICommandHandler<Command.DeletePermission>
{
    private readonly IPermissionManagementService _permissionManagementService;
    private readonly ILogger<DeletePermissionCommandHandler> _logger;

    public DeletePermissionCommandHandler(
        IPermissionManagementService permissionManagementService,
        ILogger<DeletePermissionCommandHandler> logger)
    {
        _permissionManagementService = permissionManagementService;
        _logger = logger;
    }

    public async Task<Result> Handle(Command.DeletePermission request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _permissionManagementService.DeletePermissionAsync(
                request.RoleId,
                request.FunctionId,
                request.ActionId);

            if (!success)
            {
                return Result.Failure(new Error("PermissionDeletion.Failed", "Permission deletion failed"));
            }

            _logger.LogInformation("Permission deleted successfully for role {RoleId}, function {FunctionId}, action {ActionId}", 
                request.RoleId, request.FunctionId, request.ActionId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during permission deletion for role: {RoleId}, function: {FunctionId}, action: {ActionId}", 
                request.RoleId, request.FunctionId, request.ActionId);
            return Result.Failure(new Error("PermissionDeletion.Error", "An error occurred during permission deletion"));
        }
    }
}
