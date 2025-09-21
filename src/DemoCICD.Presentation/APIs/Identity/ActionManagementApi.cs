using Carter;
using DemoCICD.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DemoCICD.Presentation.APIs.Identity;

public class ActionManagementApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/actions";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Action Management")
            .MapGroup(BaseUrl).HasApiVersion(1);

        // Action CRUD operations
        group1.MapPost(string.Empty, CreateActionV1).RequireAuthorization();
        group1.MapGet(string.Empty, GetActionsV1).RequireAuthorization();
        group1.MapGet("active", GetActiveActionsV1).RequireAuthorization();
        group1.MapGet("{id}", GetActionByIdV1).RequireAuthorization();
        group1.MapPut("{id}", UpdateActionV1).RequireAuthorization();
        group1.MapDelete("{id}", DeleteActionV1).RequireAuthorization();

        // Functions endpoint for dropdown support
        var functionsGroup = app.NewVersionedApi("Function Management")
            .MapGroup("/api/v{version:apiVersion}/functions").HasApiVersion(1);
        functionsGroup.MapGet("active", GetActiveFunctionsV1).RequireAuthorization();
    }

    public static async Task<IResult> CreateActionV1(ISender sender, [FromBody] DemoCICD.Contract.Services.V1.Identity.Command.CreateAction command)
    {
        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Created($"/api/v1/actions/{result.Value.Id}", result.Value);
    }

    public static async Task<IResult> GetActionsV1(
        ISender sender,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null)
    {
        var query = new DemoCICD.Contract.Services.V1.Identity.Query.GetActions(page, pageSize, searchTerm);
        var result = await sender.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> GetActiveActionsV1(ISender sender)
    {
        var query = new DemoCICD.Contract.Services.V1.Identity.Query.GetActiveActions();
        var result = await sender.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> GetActionByIdV1(ISender sender, [FromRoute] string id)
    {
        var query = new DemoCICD.Contract.Services.V1.Identity.Query.GetActionById(id);
        var result = await sender.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> UpdateActionV1(ISender sender, [FromRoute] string id, [FromBody] DemoCICD.Contract.Services.V1.Identity.Command.UpdateAction command)
    {
        if (id != command.Id)
        {
            return Results.BadRequest("Route ID does not match command ID");
        }

        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> DeleteActionV1(ISender sender, [FromRoute] string id)
    {
        var command = new DemoCICD.Contract.Services.V1.Identity.Command.DeleteAction(id);
        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.NoContent();
    }

    public static async Task<IResult> GetActiveFunctionsV1(ISender sender)
    {
        var query = new DemoCICD.Contract.Services.V1.Identity.Query.GetActiveFunctions();
        var result = await sender.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}