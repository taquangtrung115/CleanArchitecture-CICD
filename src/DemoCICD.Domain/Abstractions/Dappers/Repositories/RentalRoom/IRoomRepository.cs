using DemoCICD.Domain.Entities.RentalRoom.Rooms;

namespace DemoCICD.Domain.Abstractions.Dappers.Repositories.RentalRoom;

/// <summary>
/// Repository interface cho Room entity - EF Core version
/// </summary>
public interface IRoomRepository
{
    /// <summary>
    /// L?y danh sách phòng theo ??a ch?
    /// </summary>
    Task<IReadOnlyList<Room>> GetRoomsByLocationAsync(Guid locationId);

    /// <summary>
    /// L?y danh sách phòng theo tr?ng thái
    /// </summary>
    Task<IReadOnlyList<Room>> GetRoomsByAvailabilityAsync(bool isAvailable);
}
