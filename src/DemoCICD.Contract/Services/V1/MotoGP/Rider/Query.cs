using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Enumerations;
using static DemoCICD.Contract.Services.V1.MotoGP.Rider.Response;

namespace DemoCICD.Contract.Services.V1.MotoGP.Rider;

public static class Query
{
    public record GetRidersQuery(
        string? SearchTerm = null,
        string? SortColumn = null,
        SortOrder? SortOrder = null,
        IDictionary<string, SortOrder>? SortColumnAndOrder = null,
        int PageIndex = 1,
        int PageSize = 10,
        bool? IsActive = null,
        string? CountryCode = null,
        Guid? TeamId = null) : IQuery<PagedResult<RiderResponse>>;

    public record GetRiderByIdQuery(Guid Id) : IQuery<RiderResponse>;

    public record GetRiderByRacingNumberQuery(int RacingNumber) : IQuery<RiderResponse>;

    public record GetRidersByTeamQuery(
        Guid TeamId,
        int PageIndex = 1,
        int PageSize = 10) : IQuery<PagedResult<RiderResponse>>;

    public record GetRidersByNationalityQuery(
        string CountryCode,
        int PageIndex = 1,
        int PageSize = 10) : IQuery<PagedResult<RiderResponse>>;

    public record GetActiveRidersQuery(
        int PageIndex = 1,
        int PageSize = 10) : IQuery<PagedResult<RiderResponse>>;

    public record GetRetiredRidersQuery(
        int PageIndex = 1,
        int PageSize = 10) : IQuery<PagedResult<RiderResponse>>;

    public record GetRidersWithoutTeamQuery(
        int PageIndex = 1,
        int PageSize = 10) : IQuery<PagedResult<RiderResponse>>;

    public record GetRiderWithTeamHistoryQuery(Guid Id) : IQuery<RiderWithHistoryResponse>;
}