using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carter;
using DemoCICD.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DemoCICD.Presentation.APIs.Identity;

public class ActionInFunctionManagementApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/action-in-functions";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("ActionInFunction Management")
            .MapGroup(BaseUrl).HasApiVersion(1);

        // ActionInFunction CRUD operations
        group1.MapPost(string.Empty, CreateActionInFunctionV1).RequireAuthorization();
        group1.MapGet(string.Empty, GetActionInFunctionsV1).RequireAuthorization();
        group1.MapGet("{actionId}/{functionId}", GetActionInFunctionV1).RequireAuthorization();
        group1.MapDelete("{actionId}/{functionId}", DeleteActionInFunctionV1).RequireAuthorization();
    }

    public static async Task<IResult> CreateActionInFunctionV1(ISender sender, [FromBody] DemoCICD.Contract.Services.V1.Identity.Command.CreateActionInFunction command)
    {
        var result = await sender.Send(command);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> GetActionInFunctionsV1(
        ISender sender,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null)
    {
        var query = new DemoCICD.Contract.Services.V1.Identity.Query.GetActionInFunctions(page, pageSize, searchTerm);
        var result = await sender.Send(query);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> GetActionInFunctionV1(ISender sender, [FromRoute] string actionId, [FromRoute] string functionId)
    {
        var query = new DemoCICD.Contract.Services.V1.Identity.Query.GetActionInFunction(actionId, functionId);
        var result = await sender.Send(query);

        if (result.IsFailure)
        {
            return Results.NotFound(result.Error);
        }

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> DeleteActionInFunctionV1(ISender sender, [FromRoute] string actionId, [FromRoute] string functionId)
    {
        var command = new DemoCICD.Contract.Services.V1.Identity.Command.DeleteActionInFunction(actionId, functionId);
        var result = await sender.Send(command);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.NoContent();
    }
}