using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Reponsitories.MotoGP;

public class TeamRepository : AuditableRepositoryBase<Team, Guid>, ITeamRepository
{
    private readonly ApplicationDbContext _context;

    public TeamRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Team>> GetActiveTeamsAsync(CancellationToken cancellationToken = default)
    {
        return await FindAll(t => t.IsActive)
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Team?> GetTeamWithRidersAsync(Guid teamId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Team>()
            .Where(t => t.Id == teamId && !t.IsDeleted)
            .Include(t => t.Riders.Where(r => !r.IsDeleted))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Team?> GetTeamWithBikesAsync(Guid teamId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Team>()
            .Where(t => t.Id == teamId && !t.IsDeleted)
            .Include(t => t.Bikes.Where(b => !b.IsDeleted))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Team?> GetTeamWithRidersAndBikesAsync(Guid teamId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Team>()
            .Where(t => t.Id == teamId && !t.IsDeleted)
            .Include(t => t.Riders.Where(r => !r.IsDeleted))
            .Include(t => t.Bikes.Where(b => !b.IsDeleted))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Team>> GetTeamsByCountryAsync(string countryCode, CancellationToken cancellationToken = default)
    {
        return await FindAll(t => t.Country.Code == countryCode)
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Team?> GetTeamByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await FindAll(t => t.Name == name)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Team?> GetTeamByShortNameAsync(string shortName, CancellationToken cancellationToken = default)
    {
        return await FindAll(t => t.ShortName == shortName)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, Guid? excludeTeamId = null, CancellationToken cancellationToken = default)
    {
        var query = FindAll(t => t.Name == name);
        if (excludeTeamId.HasValue)
            query = query.Where(t => t.Id != excludeTeamId.Value);
        
        return await query.AnyAsync(cancellationToken);
    }
}