using DemoCICD.Contract.Abstractions.Message;

namespace DemoCICD.Contract.Services.V1.RentalRoom;

/// <summary>
/// Commands cho qu?n lý ??a ch? phòng tr?
/// </summary>
public static class LocationCommand
{
    /// <summary>
    /// Command t?o ??a ch? m?i
    /// </summary>
    public record CreateLocationCommand(
        string Street,
        string Ward,
        string District,
        string City,
        string? PostalCode = null,
        double? Latitude = null,
        double? Longitude = null,
        string? Notes = null) : ICommand<Guid>;

    /// <summary>
    /// Command c?p nh?t ??a ch?
    /// </summary>
    public record UpdateLocationCommand(
        Guid Id,
        string Street,
        string Ward,
        string District,
        string City,
        string? PostalCode = null,
        double? Latitude = null,
        double? Longitude = null,
        string? Notes = null) : ICommand;

    /// <summary>
    /// Command xóa ??a ch?
    /// </summary>
    public record DeleteLocationCommand(Guid Id) : ICommand;

    /// <summary>
    /// Command c?p nh?t t?a ?? GPS
    /// </summary>
    public record UpdateLocationCoordinatesCommand(
        Guid Id,
        double Latitude,
        double Longitude) : ICommand;
}
