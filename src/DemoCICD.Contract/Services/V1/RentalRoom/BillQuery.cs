using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Enumerations;

namespace DemoCICD.Contract.Services.V1.RentalRoom;

/// <summary>
/// Queries cho qu?n lý hóa ??n phòng tr?
/// </summary>
public static class BillQuery
{
    /// <summary>
    /// Query l?y danh sách hóa ??n có phân trang
    /// </summary>
    public record GetBillsQuery(
        string? SearchTerm,
        BillStatus? Status,
        Guid? RoomId,
        Guid? ProfileId,
        int? Month,
        int? Year,
        string? SortColumn,
        SortOrder? SortOrder,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<BillResponse.Response>>;

    /// <summary>
    /// Query l?y thông tin hóa ??n theo ID
    /// </summary>
    public record GetBillByIdQuery(Guid Id) : IQuery<BillResponse.DetailedResponse>;

    /// <summary>
    /// Query l?y danh sách hóa ??n theo phòng
    /// </summary>
    public record GetBillsByRoomQuery(Guid RoomId) : IQuery<List<BillResponse.Response>>;

    /// <summary>
    /// Query l?y danh sách hóa ??n theo ng??i thuê
    /// </summary>
    public record GetBillsByProfileQuery(Guid ProfileId) : IQuery<List<BillResponse.Response>>;

    /// <summary>
    /// Query l?y hóa ??n theo tháng/n?m
    /// </summary>
    public record GetBillsByMonthYearQuery(int Month, int Year) : IQuery<List<BillResponse.Response>>;
}
