namespace DemoCICD.Contract.Services.V1.RentalRoom;

/// <summary>
/// Responses cho qu?n lý phòng tr?
/// </summary>
public static class RoomResponse
{
    /// <summary>
    /// Response thông tin phòng
    /// </summary>
    public record Response(
        Guid Id,
        string RoomNumber,
        int Capacity,
        decimal PricePerNight,
        string Description,
        bool IsAvailable,
        Guid? LocationId,
        string? LocationAddress,
        DateTime CreatedAt,
        DateTime? UpdatedAt);
}
