using Carter;
using DemoCICD.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DemoCICD.Presentation.APIs.Identity;

public class PositionManagementApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/positions";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Position Management")
            .MapGroup(BaseUrl).HasApiVersion(1);

        // Position CRUD operations
        group1.MapPost(string.Empty, CreatePositionV1).RequireAuthorization();
        group1.MapGet(string.Empty, GetPositionsV1).RequireAuthorization();
        group1.MapGet("active", GetActivePositionsV1).RequireAuthorization();
        group1.MapGet("{positionId:guid}", GetPositionByIdV1).RequireAuthorization();
        group1.MapPut("{positionId:guid}", UpdatePositionV1).RequireAuthorization();
        group1.MapDelete("{positionId:guid}", DeletePositionV1).RequireAuthorization();
    }

    public static async Task<IResult> CreatePositionV1(ISender sender, [FromBody] DemoCICD.Contract.Services.V1.Identity.Command.CreatePosition command)
    {
        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Created($"/api/v1/positions/{result.Value.PositionId}", result.Value);
    }

    public static async Task<IResult> GetPositionsV1(
        ISender sender,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null)
    {
        var query = new DemoCICD.Contract.Services.V1.Identity.Query.GetPositions(page, pageSize, searchTerm);
        var result = await sender.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> GetActivePositionsV1(ISender sender)
    {
        var query = new DemoCICD.Contract.Services.V1.Identity.Query.GetActivePositions();
        var result = await sender.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> GetPositionByIdV1(ISender sender, [FromRoute] Guid positionId)
    {
        var query = new DemoCICD.Contract.Services.V1.Identity.Query.GetPositionById(positionId);
        var result = await sender.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> UpdatePositionV1(
        ISender sender,
        [FromRoute] Guid positionId,
        [FromBody] DemoCICD.Contract.Services.V1.Identity.Command.UpdatePosition command)
    {
        if (positionId != command.PositionId)
        {
            return Results.BadRequest("Position ID in route and body must match");
        }

        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> DeletePositionV1(ISender sender, [FromRoute] Guid positionId)
    {
        var command = new DemoCICD.Contract.Services.V1.Identity.Command.DeletePosition(positionId);
        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.NoContent();
    }
}