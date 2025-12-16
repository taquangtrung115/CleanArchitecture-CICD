using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Enumerations;

namespace DemoCICD.Contract.Services.V1.RentalRoom;

/// <summary>
/// Queries cho qu?n lý ng??i thuê phòng
/// </summary>
public static class ProfileQuery
{
    /// <summary>
    /// Query l?y danh sách ng??i thuê có phân trang
    /// </summary>
    public record GetProfilesQuery(
        string? SearchTerm,
        bool? IsActive,
        Guid? RoomId,
        string? SortColumn,
        SortOrder? SortOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<ProfileResponse.Response>>;

    /// <summary>
    /// Query l?y thông tin ng??i thuê theo ID
    /// </summary>
    public record GetProfileByIdQuery(Guid Id) : IQuery<ProfileResponse.Response>;

    /// <summary>
    /// Query l?y danh sách ng??i thuê theo phòng
    /// </summary>
    public record GetProfilesByRoomQuery(Guid RoomId) : IQuery<List<ProfileResponse.Response>>;
}
