using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Services.V1.Identity;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Application.Abstractions;

namespace DemoCICD.Application.UserCases.V1.Commands.Identity;

// Create Action Command Handler
public sealed class CreateActionCommandHandler : ICommandHandler<Command.CreateAction, Response.ActionCreated>
{
    private readonly IActionManagementService _actionManagementService;

    public CreateActionCommandHandler(IActionManagementService actionManagementService)
    {
        _actionManagementService = actionManagementService;
    }

    public async Task<Result<Response.ActionCreated>> Handle(Command.CreateAction request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if action already exists
            var exists = await _actionManagementService.ActionExistsAsync(request.Id, cancellationToken);
            if (exists)
            {
                return Result.Failure<Response.ActionCreated>(
                    new Error("Action.AlreadyExists", $"Action with ID '{request.Id}' already exists"));
            }

            var result = await _actionManagementService.CreateActionAsync(
                request.Id,
                request.Name, 
                request.SortOrder, 
                request.IsActive, 
                cancellationToken);

            if (result == null)
            {
                return Result.Failure<Response.ActionCreated>(
                    new Error("Action.CreateFailed", "Failed to create action"));
            }

            return Result.Success(new Response.ActionCreated(result.Value.Id, result.Value.Name));
        }
        catch (Exception ex)
        {
            return Result.Failure<Response.ActionCreated>(
                new Error("Action.CreateFailed", $"Failed to create action: {ex.Message}"));
        }
    }
}

// Update Action Command Handler
public sealed class UpdateActionCommandHandler : ICommandHandler<Command.UpdateAction, Response.ActionUpdated>
{
    private readonly IActionManagementService _actionManagementService;

    public UpdateActionCommandHandler(IActionManagementService actionManagementService)
    {
        _actionManagementService = actionManagementService;
    }

    public async Task<Result<Response.ActionUpdated>> Handle(Command.UpdateAction request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if action exists
            var exists = await _actionManagementService.ActionExistsAsync(request.Id, cancellationToken);
            if (!exists)
            {
                return Result.Failure<Response.ActionUpdated>(
                    new Error("Action.NotFound", $"Action with ID '{request.Id}' not found"));
            }

            var result = await _actionManagementService.UpdateActionAsync(
                request.Id,
                request.Name, 
                request.SortOrder, 
                request.IsActive, 
                cancellationToken);

            if (result == null)
            {
                return Result.Failure<Response.ActionUpdated>(
                    new Error("Action.UpdateFailed", "Failed to update action"));
            }

            return Result.Success(new Response.ActionUpdated(result.Value.Id, result.Value.Name));
        }
        catch (Exception ex)
        {
            return Result.Failure<Response.ActionUpdated>(
                new Error("Action.UpdateFailed", $"Failed to update action: {ex.Message}"));
        }
    }
}

// Delete Action Command Handler
public sealed class DeleteActionCommandHandler : ICommandHandler<Command.DeleteAction>
{
    private readonly IActionManagementService _actionManagementService;

    public DeleteActionCommandHandler(IActionManagementService actionManagementService)
    {
        _actionManagementService = actionManagementService;
    }

    public async Task<Result> Handle(Command.DeleteAction request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if action exists
            var exists = await _actionManagementService.ActionExistsAsync(request.Id, cancellationToken);
            if (!exists)
            {
                return Result.Failure(
                    new Error("Action.NotFound", $"Action with ID '{request.Id}' not found"));
            }

            var result = await _actionManagementService.DeleteActionAsync(request.Id, cancellationToken);
            if (!result)
            {
                return Result.Failure(
                    new Error("Action.DeleteFailed", "Failed to delete action"));
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(
                new Error("Action.DeleteFailed", $"Failed to delete action: {ex.Message}"));
        }
    }
}