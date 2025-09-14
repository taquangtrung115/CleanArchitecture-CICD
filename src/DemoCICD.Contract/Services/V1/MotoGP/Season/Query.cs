using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Enumerations;
using static DemoCICD.Contract.Services.V1.MotoGP.Season.Response;

namespace DemoCICD.Contract.Services.V1.MotoGP.Season;

public static class Query
{
    public record GetSeasonsQuery(
        string? SearchTerm = null,
        string? SortColumn = null,
        SortOrder? SortOrder = null,
        IDictionary<string, SortOrder>? SortColumnAndOrder = null,
        int PageIndex = 1,
        int PageSize = 10,
        bool? IsActive = null,
        int? Year = null) : IQuery<PagedResult<SeasonResponse>>;

    public record GetSeasonByIdQuery(Guid Id) : IQuery<SeasonResponse>;

    public record GetSeasonByYearQuery(int Year) : IQuery<SeasonResponse>;

    public record GetCurrentSeasonQuery() : IQuery<SeasonResponse>;

    public record GetSeasonWithRacesQuery(Guid Id) : IQuery<SeasonWithRacesResponse>;

    public record GetSeasonWithStandingsQuery(Guid Id) : IQuery<SeasonWithStandingsResponse>;

    public record GetSeasonWithFullDetailsQuery(Guid Id) : IQuery<SeasonWithFullDetailsResponse>;

    public record GetActiveSeasonQuery() : IQuery<SeasonResponse>;

    public record GetCompletedSeasonsQuery(
        int PageIndex = 1,
        int PageSize = 10) : IQuery<PagedResult<SeasonResponse>>;
}