using Carter;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Rider;
using DemoCICD.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DemoCICD.Presentation.APIs.MotoGP;

public class RiderApi : ApiEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/motogp/riders")
            .WithTags("MotoGP - Riders")
            ;

        group.MapPost("", CreateRider)
            .WithName("CreateRider")
            .WithSummary("Create a new MotoGP rider")
            .WithDescription("Creates a new MotoGP rider with the specified details")
            .Produces<Result>(StatusCodes.Status201Created)
            .ProducesValidationProblem().RequireAuthorization();

        group.MapGet("", GetRiders)
            .WithName("GetRiders")
            .WithSummary("Get paginated list of MotoGP riders")
            .WithDescription("Retrieves a paginated list of MotoGP riders with optional filtering and sorting")
            .Produces<PagedResult<Response.RiderResponse>>(StatusCodes.Status200OK);

        group.MapGet("{id:guid}", GetRiderById)
            .WithName("GetRiderById")
            .WithSummary("Get a MotoGP rider by ID")
            .WithDescription("Retrieves a specific MotoGP rider by their unique identifier")
            .Produces<Response.RiderResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("racing-number/{racingNumber:int}", GetRiderByRacingNumber)
            .WithName("GetRiderByRacingNumber")
            .WithSummary("Get a MotoGP rider by racing number")
            .WithDescription("Retrieves a specific MotoGP rider by their racing number")
            .Produces<Response.RiderResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("{id:guid}/personal-info", UpdateRiderPersonalInfo).RequireAuthorization()
            .WithName("UpdateRiderPersonalInfo")
            .WithSummary("Update rider personal information")
            .WithDescription("Updates a rider's personal information such as name, height, weight, etc.")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem();

        group.MapPut("{id:guid}/transfer", TransferRiderToTeam).RequireAuthorization()
            .WithName("TransferRiderToTeam")
            .WithSummary("Transfer rider to a team")
            .WithDescription("Transfers a rider to a specific team for a given season")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem();

        group.MapPut("{id:guid}/retire", RetireRider).RequireAuthorization()
            .WithName("RetireRider")
            .WithSummary("Retire a rider")
            .WithDescription("Retires a rider from active competition")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("{id:guid}/comeback", RiderComeback).RequireAuthorization()
            .WithName("RiderComeback")
            .WithSummary("Bring rider back from retirement")
            .WithDescription("Brings a retired rider back to active competition")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("{id:guid}", DeleteRider).RequireAuthorization()
            .WithName("DeleteRider")
            .WithSummary("Delete a rider")
            .WithDescription("Permanently deletes a rider from the system")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> CreateRider(ISender sender, [FromBody] Command.CreateRiderCommand command)
    {
        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Created($"/api/v1/motogp/riders", result);
    }

    public static async Task<IResult> GetRiders(
        ISender sender,
        string? searchTerm = null,
        string? sortColumn = null,
        string? sortOrder = null,
        int pageIndex = 1,
        int pageSize = 10,
        bool? isActive = null,
        string? countryCode = null,
        Guid? teamId = null)
    {
        var query = new Query.GetRidersQuery(
            searchTerm,
            sortColumn,
            ParseSortOrder(sortOrder),
            null,
            pageIndex,
            pageSize,
            isActive,
            countryCode,
            teamId);

        var result = await sender.Send(query);
        return Results.Ok(result);
    }

    public static async Task<IResult> GetRiderById(ISender sender, Guid id)
    {
        var result = await sender.Send(new Query.GetRiderByIdQuery(id));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> GetRiderByRacingNumber(ISender sender, int racingNumber)
    {
        var result = await sender.Send(new Query.GetRiderByRacingNumberQuery(racingNumber));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> UpdateRiderPersonalInfo(
        ISender sender,
        Guid id,
        [FromBody] UpdateRiderPersonalInfoRequest request)
    {
        var command = new Command.UpdateRiderPersonalInfoCommand(
            id,
            request.FirstName,
            request.LastName,
            request.Nickname,
            request.Height,
            request.Weight);

        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> TransferRiderToTeam(
        ISender sender,
        Guid id,
        [FromBody] TransferRiderRequest request)
    {
        var command = new Command.TransferRiderToTeamCommand(
            id,
            request.TeamId,
            request.SeasonId,
            request.JoinDate);

        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> RetireRider(
        ISender sender,
        Guid id,
        [FromBody] RetireRiderRequest request)
    {
        var command = new Command.RetireRiderCommand(id, request.RetirementDate);
        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> RiderComeback(ISender sender, Guid id)
    {
        var command = new Command.RiderComebackCommand(id);
        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> DeleteRider(ISender sender, Guid id)
    {
        var command = new Command.DeleteRiderCommand(id);
        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    private static Contract.Enumerations.SortOrder? ParseSortOrder(string? sortOrder) =>
        sortOrder?.ToLower() switch
        {
            "desc" or "descending" => Contract.Enumerations.SortOrder.Descending,
            "asc" or "ascending" => Contract.Enumerations.SortOrder.Ascending,
            _ => null
        };

    public record UpdateRiderPersonalInfoRequest(
        string FirstName,
        string LastName,
        string? Nickname,
        decimal Height,
        decimal Weight);

    public record TransferRiderRequest(
        Guid TeamId,
        Guid SeasonId,
        DateTime JoinDate);

    public record RetireRiderRequest(DateTime RetirementDate);
}
