using DemoCICD.Contract.Abstractions.Message;

namespace DemoCICD.Contract.Services.V1.RentalRoom;

/// <summary>
/// Commands cho qu?n lý phòng tr?
/// </summary>
public static class RoomCommand
{
    /// <summary>
    /// Command t?o phòng m?i
    /// </summary>
    public record CreateRoomCommand(
        string RoomNumber,
        int Capacity,
        decimal PricePerNight,
        string Description,
        Guid? LocationId = null) : ICommand<Guid>;

    /// <summary>
    /// Command c?p nh?t thông tin phòng
    /// </summary>
    public record UpdateRoomCommand(
        Guid Id,
        string RoomNumber,
        int Capacity,
        decimal PricePerNight,
        string Description,
        Guid? LocationId = null) : ICommand;

    /// <summary>
    /// Command xóa phòng
    /// </summary>
    public record DeleteRoomCommand(Guid Id) : ICommand;

    /// <summary>
    /// Command c?p nh?t tr?ng thái phòng
    /// </summary>
    public record UpdateRoomAvailabilityCommand(Guid Id, bool IsAvailable) : ICommand;
}
