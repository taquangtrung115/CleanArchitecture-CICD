using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Enumerations;

namespace DemoCICD.Contract.Services.V1.RentalRoom;

/// <summary>
/// Queries cho qu?n lý ??a ch? phòng tr?
/// </summary>
public static class LocationQuery
{
    /// <summary>
    /// Query l?y danh sách ??a ch? có phân trang
    /// </summary>
    public record GetLocationsQuery(
        string? SearchTerm,
        string? City,
        string? District,
        string? SortColumn,
        SortOrder? SortOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<LocationResponse.Response>>;

    /// <summary>
    /// Query l?y thông tin ??a ch? theo ID
    /// </summary>
    public record GetLocationByIdQuery(Guid Id) : IQuery<LocationResponse.Response>;
}
