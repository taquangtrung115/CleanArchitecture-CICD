using DemoCICD.Contract.Enumerations;
using DemoCICD.Domain.Entities.RentalRoom.Bills;

namespace DemoCICD.Domain.Abstractions.Dappers.Repositories.RentalRoom;

/// <summary>
/// Repository interface cho Bill entity - EF Core version
/// </summary>
public interface IBillRepository
{
    /// <summary>
    /// L?y danh sách hóa ??n theo phòng
    /// </summary>
    Task<IReadOnlyList<Bill>> GetBillsByRoomAsync(Guid roomId);

    /// <summary>
    /// L?y danh sách hóa ??n theo ng??i thuê
    /// </summary>
    Task<IReadOnlyList<Bill>> GetBillsByProfileAsync(Guid profileId);

    /// <summary>
    /// L?y danh sách hóa ??n theo tháng và n?m
    /// </summary>
    Task<IReadOnlyList<Bill>> GetBillsByMonthYearAsync(int month, int year);

    /// <summary>
    /// L?y danh sách hóa ??n theo tr?ng thái
    /// </summary>
    Task<IReadOnlyList<Bill>> GetBillsByStatusAsync(BillStatus status);

    /// <summary>
    /// L?y hóa ??n v?i chi ti?t
    /// </summary>
    Task<Bill?> GetBillWithDetailsAsync(Guid billId);
}
