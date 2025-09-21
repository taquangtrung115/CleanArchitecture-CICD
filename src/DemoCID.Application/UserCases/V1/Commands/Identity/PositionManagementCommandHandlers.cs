using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Services.V1.Identity;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Application.Abstractions;
using DemoCICD.Domain.Entities.Identity;
using DemoCICD.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Application.UserCases.V1.Commands.Identity;

// Create Position Command Handler
public sealed class CreatePositionCommandHandler : ICommandHandler<Command.CreatePosition, Response.PositionCreated>
{
    private readonly IPositionManagementService _positionManagementService;

    public CreatePositionCommandHandler(IPositionManagementService positionManagementService)
    {
        _positionManagementService = positionManagementService;
    }

    public async Task<Result<Response.PositionCreated>> Handle(Command.CreatePosition request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _positionManagementService.CreatePositionAsync(
                request.Name, 
                request.Description ?? string.Empty, 
                request.Code, 
                request.Level, 
                cancellationToken);

            if (result == null)
            {
                return Result.Failure<Response.PositionCreated>(
                    new Error("Position.CreateFailed", "Failed to create position"));
            }

            return Result.Success(new Response.PositionCreated(result.Value.PositionId, result.Value.Name, result.Value.Code));
        }
        catch (Exception ex)
        {
            return Result.Failure<Response.PositionCreated>(
                new Error("Position.CreateFailed", $"Failed to create position: {ex.Message}"));
        }
    }
}

// Update Position Command Handler
public sealed class UpdatePositionCommandHandler : ICommandHandler<Command.UpdatePosition, Response.PositionUpdated>
{
    private readonly IPositionManagementService _positionManagementService;

    public UpdatePositionCommandHandler(IPositionManagementService positionManagementService)
    {
        _positionManagementService = positionManagementService;
    }

    public async Task<Result<Response.PositionUpdated>> Handle(Command.UpdatePosition request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _positionManagementService.UpdatePositionAsync(
                request.PositionId,
                request.Name,
                request.Description ?? string.Empty,
                request.Code,
                request.Level,
                request.IsActive,
                cancellationToken);

            if (result == null)
            {
                return Result.Failure<Response.PositionUpdated>(
                    new Error("Position.UpdateFailed", "Failed to update position"));
            }

            return Result.Success(new Response.PositionUpdated(result.Value.PositionId, result.Value.Name, result.Value.Code));
        }
        catch (Exception ex)
        {
            return Result.Failure<Response.PositionUpdated>(
                new Error("Position.UpdateFailed", $"Failed to update position: {ex.Message}"));
        }
    }
}

// Delete Position Command Handler
public sealed class DeletePositionCommandHandler : ICommandHandler<Command.DeletePosition>
{
    private readonly IPositionManagementService _positionManagementService;

    public DeletePositionCommandHandler(IPositionManagementService positionManagementService)
    {
        _positionManagementService = positionManagementService;
    }

    public async Task<Result> Handle(Command.DeletePosition request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _positionManagementService.DeletePositionAsync(request.PositionId, cancellationToken);

            if (!success)
            {
                return Result.Failure(
                    new Error("Position.DeleteFailed", "Failed to delete position"));
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(
                new Error("Position.DeleteFailed", $"Failed to delete position: {ex.Message}"));
        }
    }
}