using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Services.V1.Identity;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Application.Abstractions;

namespace DemoCICD.Application.UserCases.V1.Queries.Identity;

// Get ActionInFunctions Query Handler
public sealed class GetActionInFunctionsQueryHandler : IQueryHandler<Query.GetActionInFunctions, Response.ActionInFunctionList>
{
    private readonly IActionInFunctionManagementService _actionInFunctionManagementService;

    public GetActionInFunctionsQueryHandler(IActionInFunctionManagementService actionInFunctionManagementService)
    {
        _actionInFunctionManagementService = actionInFunctionManagementService;
    }

    public async Task<Result<Response.ActionInFunctionList>> Handle(Query.GetActionInFunctions request, CancellationToken cancellationToken)
    {
        try
        {
            var (actionInFunctions, totalCount) = await _actionInFunctionManagementService.GetActionInFunctionsAsync(
                request.Page, 
                request.PageSize, 
                request.SearchTerm, 
                cancellationToken);

            var actionInFunctionSummaries = actionInFunctions.Cast<dynamic>().Select(af => new Response.ActionInFunctionSummary(
                af.ActionId,
                af.FunctionId,
                af.ActionName,
                af.FunctionName
            ));

            return Result.Success(new Response.ActionInFunctionList(actionInFunctionSummaries, totalCount, request.Page, request.PageSize));
        }
        catch (Exception ex)
        {
            return Result.Failure<Response.ActionInFunctionList>(
                new Error("ActionInFunction.GetListFailed", $"Failed to get ActionInFunctions: {ex.Message}"));
        }
    }
}

// Get ActionInFunction By Id Query Handler
public sealed class GetActionInFunctionQueryHandler : IQueryHandler<Query.GetActionInFunction, Response.ActionInFunctionDetails>
{
    private readonly IActionInFunctionManagementService _actionInFunctionManagementService;

    public GetActionInFunctionQueryHandler(IActionInFunctionManagementService actionInFunctionManagementService)
    {
        _actionInFunctionManagementService = actionInFunctionManagementService;
    }

    public async Task<Result<Response.ActionInFunctionDetails>> Handle(Query.GetActionInFunction request, CancellationToken cancellationToken)
    {
        try
        {
            var actionInFunction = await _actionInFunctionManagementService.GetActionInFunctionAsync(request.ActionId, request.FunctionId, cancellationToken);

            if (actionInFunction == null)
            {
                return Result.Failure<Response.ActionInFunctionDetails>(
                    new Error("ActionInFunction.NotFound", $"ActionInFunction with ActionId '{request.ActionId}' and FunctionId '{request.FunctionId}' not found"));
            }

            dynamic af = actionInFunction;
            return Result.Success(new Response.ActionInFunctionDetails(af.ActionId, af.FunctionId, af.ActionName, af.FunctionName));
        }
        catch (Exception ex)
        {
            return Result.Failure<Response.ActionInFunctionDetails>(
                new Error("ActionInFunction.GetDetailsFailed", $"Failed to get ActionInFunction details: {ex.Message}"));
        }
    }
}