using DemoCICD.Domain.Abstractions.Dappers.Repositories.RentalRoom;
using DemoCICD.Domain.Entities.RentalRoom.Rooms;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Reponsitories.RentalRoom;

/// <summary>
/// EF Core Repository implementation cho Room entity
/// </summary>
public class RoomRepository : AuditableRepositoryBase<Room, Guid>, IRoomRepository
{
    private readonly ApplicationDbContext _context;

    public RoomRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Room>> GetRoomsByLocationAsync(Guid locationId)
    {
        return await FindAll(r => r.LocationId == locationId)
            .OrderBy(r => r.RoomNumber)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Room>> GetRoomsByAvailabilityAsync(bool isAvailable)
    {
        return await FindAll(r => r.IsAvailable == isAvailable)
            .OrderBy(r => r.RoomNumber)
            .ToListAsync();
    }
}
