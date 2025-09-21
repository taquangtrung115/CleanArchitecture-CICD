using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Services.V1.Identity;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Application.Abstractions;

namespace DemoCICD.Application.UserCases.V1.Commands.Identity;

// Create ActionInFunction Command Handler
public sealed class CreateActionInFunctionCommandHandler : ICommandHandler<Command.CreateActionInFunction, Response.ActionInFunctionCreated>
{
    private readonly IActionInFunctionManagementService _actionInFunctionManagementService;

    public CreateActionInFunctionCommandHandler(IActionInFunctionManagementService actionInFunctionManagementService)
    {
        _actionInFunctionManagementService = actionInFunctionManagementService;
    }

    public async Task<Result<Response.ActionInFunctionCreated>> Handle(Command.CreateActionInFunction request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if ActionInFunction already exists
            var exists = await _actionInFunctionManagementService.ActionInFunctionExistsAsync(request.ActionId, request.FunctionId, cancellationToken);
            if (exists)
            {
                return Result.Failure<Response.ActionInFunctionCreated>(
                    new Error("ActionInFunction.AlreadyExists", $"ActionInFunction with ActionId '{request.ActionId}' and FunctionId '{request.FunctionId}' already exists"));
            }

            var result = await _actionInFunctionManagementService.CreateActionInFunctionAsync(
                request.ActionId,
                request.FunctionId, 
                cancellationToken);

            if (result == null)
            {
                return Result.Failure<Response.ActionInFunctionCreated>(
                    new Error("ActionInFunction.CreateFailed", "Failed to create ActionInFunction"));
            }

            return Result.Success(new Response.ActionInFunctionCreated(result.Value.ActionId, result.Value.FunctionId));
        }
        catch (Exception ex)
        {
            return Result.Failure<Response.ActionInFunctionCreated>(
                new Error("ActionInFunction.CreateFailed", $"Failed to create ActionInFunction: {ex.Message}"));
        }
    }
}

// Delete ActionInFunction Command Handler
public sealed class DeleteActionInFunctionCommandHandler : ICommandHandler<Command.DeleteActionInFunction>
{
    private readonly IActionInFunctionManagementService _actionInFunctionManagementService;

    public DeleteActionInFunctionCommandHandler(IActionInFunctionManagementService actionInFunctionManagementService)
    {
        _actionInFunctionManagementService = actionInFunctionManagementService;
    }

    public async Task<Result> Handle(Command.DeleteActionInFunction request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _actionInFunctionManagementService.DeleteActionInFunctionAsync(request.ActionId, request.FunctionId, cancellationToken);
            
            if (!success)
            {
                return Result.Failure(new Error("ActionInFunction.NotFound", $"ActionInFunction with ActionId '{request.ActionId}' and FunctionId '{request.FunctionId}' not found"));
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("ActionInFunction.DeleteFailed", $"Failed to delete ActionInFunction: {ex.Message}"));
        }
    }
}