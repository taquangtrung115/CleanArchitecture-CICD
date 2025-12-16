using DemoCICD.Contract.Enumerations;

namespace DemoCICD.Contract.Services.V1.RentalRoom;

/// <summary>
/// Responses cho qu?n lý hóa ??n phòng tr?
/// </summary>
public static class BillResponse
{
    /// <summary>
    /// Response thông tin hóa ??n (không bao g?m chi ti?t)
    /// </summary>
    public record Response(
        Guid Id,
        string BillNumber,
        Guid RoomId,
        string RoomNumber,
        Guid ProfileId,
        string ProfileName,
        int Month,
        int Year,
        DateTime IssueDate,
        DateTime DueDate,
        decimal RoomPrice,
        decimal ServiceTotal,
        decimal TotalAmount,
        decimal PaidAmount,
        decimal RemainingAmount,
        BillStatus Status,
        DateTime? PaymentDate,
        string? PaymentMethod,
        string? Notes,
        DateTime CreatedAt,
        DateTime? UpdatedAt);

    /// <summary>
    /// Response thông tin hóa ??n chi ti?t (bao g?m c? chi ti?t d?ch v?)
    /// </summary>
    public record DetailedResponse(
        Guid Id,
        string BillNumber,
        Guid RoomId,
        string RoomNumber,
        Guid ProfileId,
        string ProfileName,
        int Month,
        int Year,
        DateTime IssueDate,
        DateTime DueDate,
        decimal RoomPrice,
        decimal ServiceTotal,
        decimal TotalAmount,
        decimal PaidAmount,
        decimal RemainingAmount,
        BillStatus Status,
        DateTime? PaymentDate,
        string? PaymentMethod,
        string? Notes,
        List<DetailItemResponse> BillDetails,
        DateTime CreatedAt,
        DateTime? UpdatedAt);

    /// <summary>
    /// Response chi ti?t d?ch v? trong hóa ??n
    /// </summary>
    public record DetailItemResponse(
        Guid Id,
        ServiceType ServiceType,
        string ServiceName,
        string Unit,
        decimal? OldIndex,
        decimal? NewIndex,
        decimal Quantity,
        decimal UnitPrice,
        decimal TotalPrice,
        string? Notes);
}
