using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Reponsitories.MotoGP;

public class BikeRepository : AuditableRepositoryBase<Bike, Guid>, IBikeRepository
{
    private readonly ApplicationDbContext _context;

    public BikeRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Bike>> GetActiveeBikesAsync(CancellationToken cancellationToken = default)
    {
        return await FindAll(b => b.IsActive)
            .OrderBy(b => b.Manufacturer)
            .ThenBy(b => b.Model)
            .ThenByDescending(b => b.Year)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Bike>> GetBikesByTeamAsync(Guid teamId, CancellationToken cancellationToken = default)
    {
        return await FindAll(b => b.TeamId == teamId)
            .OrderBy(b => b.Model)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Bike>> GetBikesByManufacturerAsync(string manufacturer, CancellationToken cancellationToken = default)
    {
        return await FindAll(b => b.Manufacturer == manufacturer)
            .OrderBy(b => b.Model)
            .ThenByDescending(b => b.Year)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Bike>> GetBikesByYearAsync(int year, CancellationToken cancellationToken = default)
    {
        return await FindAll(b => b.Year == year)
            .OrderBy(b => b.Manufacturer)
            .ThenBy(b => b.Model)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Bike>> GetUnassignedBikesAsync(CancellationToken cancellationToken = default)
    {
        return await FindAll(b => b.TeamId == null && b.IsActive)
            .OrderBy(b => b.Manufacturer)
            .ThenBy(b => b.Model)
            .ToListAsync(cancellationToken);
    }

    public async Task<Bike?> GetBikeByChassisNumberAsync(string chassisNumber, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(chassisNumber))
            return null;

        return await FindAll(b => b.ChassisNumber == chassisNumber)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Bike?> GetBikeByEngineNumberAsync(string engineNumber, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(engineNumber))
            return null;

        return await FindAll(b => b.EngineNumber == engineNumber)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
