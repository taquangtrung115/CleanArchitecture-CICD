using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Enumerations;

namespace DemoCICD.Contract.Services.V1.RentalRoom;

/// <summary>
/// Commands cho qu?n lý hóa ??n phòng tr?
/// </summary>
public static class BillCommand
{
    /// <summary>
    /// Command t?o hóa ??n m?i
    /// </summary>
    public record CreateBillCommand(
        string BillNumber,
        Guid RoomId,
        Guid ProfileId,
        int Month,
        int Year,
        DateTime IssueDate,
        DateTime DueDate,
        decimal RoomPrice) : ICommand<Guid>;

    /// <summary>
    /// Command c?p nh?t hóa ??n
    /// </summary>
    public record UpdateBillCommand(
        Guid Id,
        DateTime DueDate,
        string? Notes = null) : ICommand;

    /// <summary>
    /// Command xóa hóa ??n
    /// </summary>
    public record DeleteBillCommand(Guid Id) : ICommand;

    /// <summary>
    /// Command thêm chi ti?t d?ch v? vào hóa ??n
    /// </summary>
    public record AddBillDetailCommand(
        Guid BillId,
        ServiceType ServiceType,
        string ServiceName,
        string Unit,
        decimal Quantity,
        decimal UnitPrice,
        decimal? OldIndex = null,
        decimal? NewIndex = null,
        string? Notes = null) : ICommand<Guid>;

    /// <summary>
    /// Command c?p nh?t chi ti?t hóa ??n
    /// </summary>
    public record UpdateBillDetailCommand(
        Guid Id,
        decimal Quantity,
        decimal UnitPrice,
        decimal? OldIndex = null,
        decimal? NewIndex = null,
        string? Notes = null) : ICommand;

    /// <summary>
    /// Command xóa chi ti?t hóa ??n
    /// </summary>
    public record DeleteBillDetailCommand(Guid BillId, Guid BillDetailId) : ICommand;

    /// <summary>
    /// Command thanh toán hóa ??n
    /// </summary>
    public record MakePaymentCommand(
        Guid BillId,
        decimal Amount,
        string PaymentMethod,
        DateTime? PaymentDate = null) : ICommand;

    /// <summary>
    /// Command ?ánh d?u hóa ??n quá h?n
    /// </summary>
    public record MarkBillAsOverdueCommand(Guid BillId) : ICommand;
}
