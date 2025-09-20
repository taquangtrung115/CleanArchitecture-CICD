using Carter;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Race;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DemoCICD.Presentation.APIs.MotoGP;

public class RaceApi : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/motogp/races")
            .WithTags("MotoGP - Races")
            .RequireAuthorization();

        group.MapGet("", GetRaces)
            .WithName("GetRaces")
            .WithSummary("Get paginated list of MotoGP races")
            .WithDescription("Retrieves a paginated list of MotoGP races with optional filtering and sorting")
            .Produces<PagedResult<Response.RaceResponse>>(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}", GetRaceById)
            .WithName("GetRaceById")
            .WithSummary("Get MotoGP race by ID")
            .WithDescription("Retrieves a specific MotoGP race by its unique identifier")
            .Produces<Response.RaceResponse>(StatusCodes.Status200OK)
            .Produces<Error>(StatusCodes.Status404NotFound);

        group.MapGet("/{id:guid}/results", GetRaceWithResults)
            .WithName("GetRaceWithResults")
            .WithSummary("Get MotoGP race with results")
            .WithDescription("Retrieves a specific MotoGP race with its classification results")
            .Produces<Response.RaceWithResultsResponse>(StatusCodes.Status200OK)
            .Produces<Error>(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> GetRaces(
        ISender sender,
        [AsParameters] Query.GetRacesQuery query)
    {
        var result = await sender.Send(query);

        return result.IsFailure 
            ? Results.NotFound(result.Error)
            : Results.Ok(result.Value);
    }

    public static async Task<IResult> GetRaceById(
        ISender sender,
        Guid id)
    {
        var query = new Query.GetRaceByIdQuery(id);
        var result = await sender.Send(query);

        return result.IsFailure 
            ? Results.NotFound(result.Error)
            : Results.Ok(result.Value);
    }

    public static async Task<IResult> GetRaceWithResults(
        ISender sender,
        Guid id)
    {
        var query = new Query.GetRaceWithResultsQuery(id);
        var result = await sender.Send(query);

        return result.IsFailure 
            ? Results.NotFound(result.Error)
            : Results.Ok(result.Value);
    }
}