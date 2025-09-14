using Carter;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Team;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DemoCICD.Presentation.APIs.MotoGP;

public class TeamApi : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/carter/v1/motogp/teams")
            .WithTags("MotoGP - Teams")
            .RequireAuthorization();

        group.MapPost("", CreateTeam)
            .WithName("CreateTeam")
            .WithSummary("Create a new MotoGP team")
            .WithDescription("Creates a new MotoGP team with the specified details")
            .Produces<Result>(StatusCodes.Status201Created)
            .ProducesValidationProblem();

        group.MapGet("", GetTeams)
            .WithName("GetTeams")
            .WithSummary("Get paginated list of MotoGP teams")
            .WithDescription("Retrieves a paginated list of MotoGP teams with optional filtering and sorting")
            .Produces<PagedResult<Response.TeamResponse>>(StatusCodes.Status200OK);

        group.MapGet("{id:guid}", GetTeamById)
            .WithName("GetTeamById")
            .WithSummary("Get a MotoGP team by ID")
            .WithDescription("Retrieves a specific MotoGP team by their unique identifier")
            .Produces<Response.TeamResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("{id:guid}/with-riders", GetTeamWithRiders)
            .WithName("GetTeamWithRiders")
            .WithSummary("Get a MotoGP team with riders")
            .WithDescription("Retrieves a specific MotoGP team with their current riders")
            .Produces<Response.TeamWithRidersResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> CreateTeam(ISender sender, [FromBody] Command.CreateTeamCommand command)
    {
        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandleFailure(result);

        return Results.Created($"/api/carter/v1/motogp/teams", result);
    }

    public static async Task<IResult> GetTeams(
        ISender sender,
        string? searchTerm = null,
        string? sortColumn = null,
        string? sortOrder = null,
        int pageIndex = 1,
        int pageSize = 10,
        bool? isActive = null,
        string? countryCode = null)
    {
        var query = new Query.GetTeamsQuery(
            searchTerm,
            sortColumn,
            ParseSortOrder(sortOrder),
            null,
            pageIndex,
            pageSize,
            isActive,
            countryCode);

        var result = await sender.Send(query);
        return Results.Ok(result);
    }

    public static async Task<IResult> GetTeamById(ISender sender, Guid id)
    {
        var result = await sender.Send(new Query.GetTeamByIdQuery(id));

        if (result.IsFailure)
            return HandleFailure(result);

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> GetTeamWithRiders(ISender sender, Guid id)
    {
        var result = await sender.Send(new Query.GetTeamWithRidersQuery(id));

        if (result.IsFailure)
            return HandleFailure(result);

        return Results.Ok(result.Value);
    }

    private static IResult HandleFailure(Result result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult =>
                Results.BadRequest(
                    CreateProblemDetails(
                        "Validation Error", StatusCodes.Status400BadRequest,
                        result.Error,
                        validationResult.Errors)),
            _ =>
                Results.BadRequest(
                    CreateProblemDetails(
                        "Bad Request", StatusCodes.Status400BadRequest,
                        result.Error))
        };

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null) =>
        new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Extensions = { ["errors"] = errors }
        };

    private static Contract.Enumerations.SortOrder? ParseSortOrder(string? sortOrder) =>
        sortOrder?.ToLower() switch
        {
            "desc" or "descending" => Contract.Enumerations.SortOrder.Descending,
            "asc" or "ascending" => Contract.Enumerations.SortOrder.Ascending,
            _ => null
        };
}