using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Domain.Entities.MotoGP.RaceManagement;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Reponsitories.MotoGP;

public class RaceRepository : AuditableRepositoryBase<Race, Guid>, IRaceRepository
{
    private readonly ApplicationDbContext _context;

    public RaceRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Race>> GetRacesBySeasonAsync(Guid seasonId, CancellationToken cancellationToken = default)
    {
        return await FindAll(r => r.SeasonId == seasonId)
            .OrderBy(r => r.RoundNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<Race?> GetRaceWithEntriesAsync(Guid raceId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Race>()
            .Where(r => r.Id == raceId && !r.IsDeleted)
            .Include(r => r.RaceEntries.Where(e => !e.IsDeleted))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Race?> GetNextRaceAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await FindAll(r => r.RaceDate > now && r.Status == RaceStatus.Upcoming)
            .OrderBy(r => r.RaceDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Race>> GetRacesByStatusAsync(RaceStatus status, CancellationToken cancellationToken = default)
    {
        return await FindAll(r => r.Status == status)
            .OrderBy(r => r.RaceDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Race?> GetRaceBySeasonAndRoundAsync(Guid seasonId, int roundNumber, CancellationToken cancellationToken = default)
    {
        return await FindAll(r => r.SeasonId == seasonId && r.RoundNumber == roundNumber)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Race>> GetRacesByCountryAsync(string countryCode, CancellationToken cancellationToken = default)
    {
        return await FindAll(r => r.Country.Code == countryCode)
            .OrderBy(r => r.RaceDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Race>> GetRacesInDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await FindAll(r => r.RaceDate >= startDate && r.RaceDate <= endDate)
            .OrderBy(r => r.RaceDate)
            .ToListAsync(cancellationToken);
    }
}