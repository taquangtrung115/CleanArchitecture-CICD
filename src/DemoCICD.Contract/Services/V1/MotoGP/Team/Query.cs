using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Enumerations;
using static DemoCICD.Contract.Services.V1.MotoGP.Team.Response;

namespace DemoCICD.Contract.Services.V1.MotoGP.Team;

public static class Query
{
    public record GetTeamsQuery(
        string? SearchTerm = null,
        string? SortColumn = null,
        SortOrder? SortOrder = null,
        IDictionary<string, SortOrder>? SortColumnAndOrder = null,
        int PageIndex = 1,
        int PageSize = 10,
        bool? IsActive = null,
        string? CountryCode = null) : IQuery<PagedResult<TeamResponse>>;

    public record GetTeamByIdQuery(Guid Id) : IQuery<TeamResponse>;

    public record GetTeamWithRidersQuery(Guid Id) : IQuery<TeamWithRidersResponse>;

    public record GetTeamWithBikesQuery(Guid Id) : IQuery<TeamWithBikesResponse>;

    public record GetTeamWithFullDetailsQuery(Guid Id) : IQuery<TeamWithFullDetailsResponse>;

    public record GetActiveTeamsQuery(
        int PageIndex = 1,
        int PageSize = 10) : IQuery<PagedResult<TeamResponse>>;

    public record GetTeamsByCountryQuery(
        string CountryCode,
        int PageIndex = 1,
        int PageSize = 10) : IQuery<PagedResult<TeamResponse>>;

    public record GetTeamsWithAvailableSlotsQuery(
        int PageIndex = 1,
        int PageSize = 10) : IQuery<PagedResult<TeamResponse>>;
}