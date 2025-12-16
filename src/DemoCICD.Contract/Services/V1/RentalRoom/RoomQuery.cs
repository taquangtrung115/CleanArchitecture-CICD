using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Enumerations;

namespace DemoCICD.Contract.Services.V1.RentalRoom;

/// <summary>
/// Queries cho qu?n lý phòng tr?
/// </summary>
public static class RoomQuery
{
    /// <summary>
    /// Query l?y danh sách phòng có phân trang
    /// </summary>
    public record GetRoomsQuery(
        string? SearchTerm,
        bool? IsAvailable,
        decimal? MinPrice,
        decimal? MaxPrice,
        int? MinCapacity,
        string? SortColumn,
        SortOrder? SortOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<RoomResponse.Response>>;

    /// <summary>
    /// Query l?y thông tin phòng theo ID
    /// </summary>
    public record GetRoomByIdQuery(Guid Id) : IQuery<RoomResponse.Response>;

    /// <summary>
    /// Query l?y danh sách phòng theo ??a ch?
    /// </summary>
    public record GetRoomsByLocationQuery(Guid LocationId) : IQuery<List<RoomResponse.Response>>;
}
