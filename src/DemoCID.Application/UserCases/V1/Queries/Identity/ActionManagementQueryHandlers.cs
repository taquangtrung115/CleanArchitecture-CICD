using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Services.V1.Identity;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Application.Abstractions;

namespace DemoCICD.Application.UserCases.V1.Queries.Identity;

// Get Actions Query Handler
public sealed class GetActionsQueryHandler : IQueryHandler<Query.GetActions, Response.ActionList>
{
    private readonly IActionManagementService _actionManagementService;

    public GetActionsQueryHandler(IActionManagementService actionManagementService)
    {
        _actionManagementService = actionManagementService;
    }

    public async Task<Result<Response.ActionList>> Handle(Query.GetActions request, CancellationToken cancellationToken)
    {
        try
        {
            var (actions, totalCount) = await _actionManagementService.GetActionsAsync(
                request.Page, 
                request.PageSize, 
                request.SearchTerm, 
                cancellationToken);

            var actionSummaries = actions.Cast<dynamic>().Select(a => new Response.ActionSummary(
                a.Id,
                a.Name,
                a.SortOrder,
                a.IsActive
            ));

            return Result.Success(new Response.ActionList(actionSummaries, totalCount, request.Page, request.PageSize));
        }
        catch (Exception ex)
        {
            return Result.Failure<Response.ActionList>(
                new Error("Action.GetListFailed", $"Failed to get actions: {ex.Message}"));
        }
    }
}

// Get Action By Id Query Handler
public sealed class GetActionByIdQueryHandler : IQueryHandler<Query.GetActionById, Response.ActionDetails>
{
    private readonly IActionManagementService _actionManagementService;

    public GetActionByIdQueryHandler(IActionManagementService actionManagementService)
    {
        _actionManagementService = actionManagementService;
    }

    public async Task<Result<Response.ActionDetails>> Handle(Query.GetActionById request, CancellationToken cancellationToken)
    {
        try
        {
            var action = await _actionManagementService.GetActionByIdAsync(request.Id, cancellationToken);
            if (action == null)
            {
                return Result.Failure<Response.ActionDetails>(
                    new Error("Action.NotFound", $"Action with ID '{request.Id}' not found"));
            }

            var actionData = (dynamic)action;
            return Result.Success(new Response.ActionDetails(
                actionData.Id,
                actionData.Name,
                actionData.SortOrder,
                actionData.IsActive
            ));
        }
        catch (Exception ex)
        {
            return Result.Failure<Response.ActionDetails>(
                new Error("Action.GetFailed", $"Failed to get action: {ex.Message}"));
        }
    }
}

// Get Active Actions Query Handler
public sealed class GetActiveActionsQueryHandler : IQueryHandler<Query.GetActiveActions, Response.ActionList>
{
    private readonly IActionManagementService _actionManagementService;

    public GetActiveActionsQueryHandler(IActionManagementService actionManagementService)
    {
        _actionManagementService = actionManagementService;
    }

    public async Task<Result<Response.ActionList>> Handle(Query.GetActiveActions request, CancellationToken cancellationToken)
    {
        try
        {
            var actions = await _actionManagementService.GetActiveActionsAsync(cancellationToken);

            var actionSummaries = actions.Cast<dynamic>().Select(a => new Response.ActionSummary(
                a.Id,
                a.Name,
                a.SortOrder,
                a.IsActive
            ));

            return Result.Success(new Response.ActionList(actionSummaries, actionSummaries.Count(), 1, actionSummaries.Count()));
        }
        catch (Exception ex)
        {
            return Result.Failure<Response.ActionList>(
                new Error("Action.GetActiveFailed", $"Failed to get active actions: {ex.Message}"));
        }
    }
}

// Get Active Functions Query Handler
public sealed class GetActiveFunctionsQueryHandler : IQueryHandler<Query.GetActiveFunctions, Response.FunctionList>
{
    private readonly IFunctionManagementService _functionManagementService;

    public GetActiveFunctionsQueryHandler(IFunctionManagementService functionManagementService)
    {
        _functionManagementService = functionManagementService;
    }

    public async Task<Result<Response.FunctionList>> Handle(Query.GetActiveFunctions request, CancellationToken cancellationToken)
    {
        try
        {
            var functions = await _functionManagementService.GetActiveFunctionsAsync(cancellationToken);

            var functionSummaries = functions.Cast<dynamic>().Select(f => new Response.FunctionSummary(
                f.Id,
                f.Name,
                f.IsActive
            ));

            return Result.Success(new Response.FunctionList(functionSummaries, functionSummaries.Count(), 1, functionSummaries.Count()));
        }
        catch (Exception ex)
        {
            return Result.Failure<Response.FunctionList>(
                new Error("Function.GetActiveFailed", $"Failed to get active functions: {ex.Message}"));
        }
    }
}