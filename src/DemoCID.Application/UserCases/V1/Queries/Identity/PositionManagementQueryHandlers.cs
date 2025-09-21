using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Services.V1.Identity;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Application.Abstractions;

namespace DemoCICD.Application.UserCases.V1.Queries.Identity;

// Get Positions Query Handler
public sealed class GetPositionsQueryHandler : IQueryHandler<Query.GetPositions, Response.PositionList>
{
    private readonly IPositionManagementService _positionManagementService;

    public GetPositionsQueryHandler(IPositionManagementService positionManagementService)
    {
        _positionManagementService = positionManagementService;
    }

    public async Task<Result<Response.PositionList>> Handle(Query.GetPositions request, CancellationToken cancellationToken)
    {
        try
        {
            var (positions, totalCount) = await _positionManagementService.GetPositionsAsync(
                request.Page, 
                request.PageSize, 
                request.SearchTerm, 
                cancellationToken);

            var positionSummaries = positions.Cast<Response.PositionSummary>();

            return Result.Success(new Response.PositionList(
                positionSummaries,
                totalCount,
                request.Page,
                request.PageSize));
        }
        catch (Exception ex)
        {
            return Result.Failure<Response.PositionList>(
                new Error("Position.GetFailed", $"Failed to get positions: {ex.Message}"));
        }
    }
}

// Get Position By Id Query Handler
public sealed class GetPositionByIdQueryHandler : IQueryHandler<Query.GetPositionById, Response.PositionDetails>
{
    private readonly IPositionManagementService _positionManagementService;

    public GetPositionByIdQueryHandler(IPositionManagementService positionManagementService)
    {
        _positionManagementService = positionManagementService;
    }

    public async Task<Result<Response.PositionDetails>> Handle(Query.GetPositionById request, CancellationToken cancellationToken)
    {
        try
        {
            var position = await _positionManagementService.GetPositionByIdAsync(request.PositionId, cancellationToken);

            if (position == null)
            {
                return Result.Failure<Response.PositionDetails>(
                    new Error("Position.NotFound", $"Position with ID '{request.PositionId}' not found"));
            }

            var details = (Response.PositionDetails)position;
            return Result.Success(details);
        }
        catch (Exception ex)
        {
            return Result.Failure<Response.PositionDetails>(
                new Error("Position.GetFailed", $"Failed to get position: {ex.Message}"));
        }
    }
}

// Get Active Positions Query Handler
public sealed class GetActivePositionsQueryHandler : IQueryHandler<Query.GetActivePositions, Response.PositionList>
{
    private readonly IPositionManagementService _positionManagementService;

    public GetActivePositionsQueryHandler(IPositionManagementService positionManagementService)
    {
        _positionManagementService = positionManagementService;
    }

    public async Task<Result<Response.PositionList>> Handle(Query.GetActivePositions request, CancellationToken cancellationToken)
    {
        try
        {
            var positions = await _positionManagementService.GetActivePositionsAsync(cancellationToken);
            var positionSummaries = positions.Cast<Response.PositionSummary>();

            return Result.Success(new Response.PositionList(
                positionSummaries,
                positionSummaries.Count(),
                1,
                positionSummaries.Count()));
        }
        catch (Exception ex)
        {
            return Result.Failure<Response.PositionList>(
                new Error("Position.GetActiveFailed", $"Failed to get active positions: {ex.Message}"));
        }
    }
}

// Get Users For Manager Selection Query Handler
public sealed class GetUsersForManagerSelectionQueryHandler : IQueryHandler<Query.GetUsersForManagerSelection, Response.UserList>
{
    private readonly IUserManagementService _userManagementService;

    public GetUsersForManagerSelectionQueryHandler(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<Result<Response.UserList>> Handle(Query.GetUsersForManagerSelection request, CancellationToken cancellationToken)
    {
        try
        {
            // Use existing GetUsersAsync but filter for directors and heads of department
            var (users, totalCount) = await _userManagementService.GetUsersAsync(1, 1000, null);
            
            var managerUsers = users
                .Where(u => u.IsDirector == true || u.IsHeadOfDepartment == true)
                .Select(u => new Response.UserSummary(
                    u.Id,
                    u.UserName ?? string.Empty,
                    u.Email ?? string.Empty,
                    u.FullName,
                    u.LockoutEnd.HasValue && u.LockoutEnd > DateTimeOffset.UtcNow))
                .ToList();

            return Result.Success(new Response.UserList(
                managerUsers,
                managerUsers.Count,
                1,
                managerUsers.Count));
        }
        catch (Exception ex)
        {
            return Result.Failure<Response.UserList>(
                new Error("User.GetManagersFailed", $"Failed to get users for manager selection: {ex.Message}"));
        }
    }
}