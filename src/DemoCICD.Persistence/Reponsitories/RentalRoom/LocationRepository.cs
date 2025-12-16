using DemoCICD.Domain.Abstractions.Dappers.Repositories.RentalRoom;
using DemoCICD.Domain.Entities.RentalRoom.Locations;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Reponsitories.RentalRoom;

/// <summary>
/// EF Core Repository implementation cho Location entity
/// </summary>
public class LocationRepository : AuditableRepositoryBase<Location, Guid>, ILocationRepository
{
    private readonly ApplicationDbContext _context;

    public LocationRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Location>> GetLocationsByCityAsync(string city)
    {
        return await FindAll(l => l.City == city)
            .OrderBy(l => l.District)
            .ThenBy(l => l.Ward)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Location>> GetLocationsByDistrictAsync(string district)
    {
        return await FindAll(l => l.District == district)
            .OrderBy(l => l.Ward)
            .ToListAsync();
    }
}
