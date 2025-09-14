using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Reponsitories.MotoGP;

public class RiderRepository : AuditableRepositoryBase<Rider, Guid>, IRiderRepository
{
    private readonly ApplicationDbContext _context;

    public RiderRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Rider>> GetActiveRidersAsync(CancellationToken cancellationToken = default)
    {
        return await FindAll(r => r.IsActive)
            .OrderBy(r => r.LastName)
            .ThenBy(r => r.FirstName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Rider?> GetRiderWithTeamHistoryAsync(Guid riderId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Rider>()
            .Where(r => r.Id == riderId && !r.IsDeleted)
            .Include(r => r.TeamHistory.Where(h => !h.IsDeleted))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Rider>> GetRidersByTeamAsync(Guid teamId, CancellationToken cancellationToken = default)
    {
        return await FindAll(r => r.CurrentTeamId == teamId)
            .OrderBy(r => r.RacingNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Rider>> GetRidersByNationalityAsync(string countryCode, CancellationToken cancellationToken = default)
    {
        return await FindAll(r => r.Nationality.Code == countryCode)
            .OrderBy(r => r.LastName)
            .ThenBy(r => r.FirstName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Rider?> GetRiderByRacingNumberAsync(int racingNumber, CancellationToken cancellationToken = default)
    {
        return await FindAll(r => r.RacingNumber == racingNumber)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Rider>> GetRetiredRidersAsync(CancellationToken cancellationToken = default)
    {
        return await FindAll(r => !r.IsActive && r.RetirementDate.HasValue)
            .OrderByDescending(r => r.RetirementDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Rider>> GetRidersWithoutTeamAsync(CancellationToken cancellationToken = default)
    {
        return await FindAll(r => r.IsActive && r.CurrentTeamId == null)
            .OrderBy(r => r.LastName)
            .ThenBy(r => r.FirstName)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsRacingNumberAvailableAsync(int racingNumber, Guid? excludeRiderId = null, CancellationToken cancellationToken = default)
    {
        var query = FindAll(r => r.RacingNumber == racingNumber && r.IsActive);
        if (excludeRiderId.HasValue)
            query = query.Where(r => r.Id != excludeRiderId.Value);
        
        return !await query.AnyAsync(cancellationToken);
    }
}