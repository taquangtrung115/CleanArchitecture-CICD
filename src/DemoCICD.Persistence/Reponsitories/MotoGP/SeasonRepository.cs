using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Domain.Entities.MotoGP.RaceManagement;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Reponsitories.MotoGP;

public class SeasonRepository : AuditableRepositoryBase<Season, Guid>, ISeasonRepository
{
    private readonly ApplicationDbContext _context;

    public SeasonRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Season?> GetCurrentSeasonAsync(CancellationToken cancellationToken = default)
    {
        return await FindAll(s => s.IsCurrentSeason).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Season?> GetSeasonByYearAsync(int year, CancellationToken cancellationToken = default)
    {
        return await FindAll(s => s.Year == year).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Season>> GetSeasonsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await FindAll()
            .OrderByDescending(s => s.Year)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Season?> GetSeasonWithRacesAsync(Guid seasonId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Season>()
            .Where(s => s.Id == seasonId && !s.IsDeleted)
            .Include(s => s.Races.Where(r => !r.IsDeleted))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int year, CancellationToken cancellationToken = default)
    {
        return await FindAll(s => s.Year == year).AnyAsync(cancellationToken);
    }
}