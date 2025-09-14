using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Enumerations;
using static DemoCICD.Contract.Services.V1.MotoGP.Race.Response;

namespace DemoCICD.Contract.Services.V1.MotoGP.Race;

public static class Query
{
    public record GetRacesQuery(
        string? SearchTerm = null,
        string? SortColumn = null,
        SortOrder? SortOrder = null,
        IDictionary<string, SortOrder>? SortColumnAndOrder = null,
        int PageIndex = 1,
        int PageSize = 10,
        Guid? SeasonId = null,
        string? Status = null,
        string? CountryCode = null,
        DateTime? FromDate = null,
        DateTime? ToDate = null) : IQuery<PagedResult<RaceResponse>>;

    public record GetRaceByIdQuery(Guid Id) : IQuery<RaceResponse>;

    public record GetRaceWithEntriesQuery(Guid Id) : IQuery<RaceWithEntriesResponse>;

    public record GetRaceWithResultsQuery(Guid Id) : IQuery<RaceWithResultsResponse>;

    public record GetRacesBySeasonQuery(
        Guid SeasonId,
        int PageIndex = 1,
        int PageSize = 10) : IQuery<PagedResult<RaceResponse>>;

    public record GetUpcomingRacesQuery(
        int PageIndex = 1,
        int PageSize = 10) : IQuery<PagedResult<RaceResponse>>;

    public record GetCompletedRacesQuery(
        int PageIndex = 1,
        int PageSize = 10) : IQuery<PagedResult<RaceResponse>>;

    public record GetRacesByCountryQuery(
        string CountryCode,
        int PageIndex = 1,
        int PageSize = 10) : IQuery<PagedResult<RaceResponse>>;

    public record GetRacesByDateRangeQuery(
        DateTime FromDate,
        DateTime ToDate,
        int PageIndex = 1,
        int PageSize = 10) : IQuery<PagedResult<RaceResponse>>;

    public record GetCurrentSeasonRacesQuery(
        int PageIndex = 1,
        int PageSize = 10) : IQuery<PagedResult<RaceResponse>>;
}