namespace DemoCICD.Contract.Services.V1.RentalRoom;

/// <summary>
/// Responses cho qu?n lý ng??i thuê phòng
/// </summary>
public static class ProfileResponse
{
    /// <summary>
    /// Response thông tin ng??i thuê
    /// </summary>
    public record Response(
        Guid Id,
        string FullName,
        string PhoneNumber,
        string IdentityCard,
        string? Email,
        DateTime? DateOfBirth,
        string? PermanentAddress,
        string? Occupation,
        Guid? RoomId,
        string? RoomNumber,
        DateTime? RentStartDate,
        DateTime? RentEndDate,
        decimal? DepositAmount,
        bool IsActive,
        DateTime CreatedAt,
        DateTime? UpdatedAt);
}
