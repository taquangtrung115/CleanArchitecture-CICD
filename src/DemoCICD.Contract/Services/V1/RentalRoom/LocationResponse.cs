namespace DemoCICD.Contract.Services.V1.RentalRoom;

/// <summary>
/// Responses cho qu?n lý ??a ch? phòng tr?
/// </summary>
public static class LocationResponse
{
    /// <summary>
    /// Response thông tin ??a ch?
    /// </summary>
    public record Response(
        Guid Id,
        string Street,
        string Ward,
        string District,
        string City,
        string FullAddress,
        string? PostalCode,
        double? Latitude,
        double? Longitude,
        string? Notes,
        DateTime CreatedAt,
        DateTime? UpdatedAt);
}
