using System.Linq.Expressions;
using DemoCICD.Domain.Abstractions.Reponsitories;
using DemoCICD.Domain.Entities.Identity;
using DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Reponsitories;

public class AppUserRepository : AuditableRepositoryBase<Bike, Guid>, IAppUserRepository
{
    private readonly ApplicationDbContext _context;

    public AppUserRepository(ApplicationDbContext context) : base(context)
        => _context = context;

    public void Dispose()
        => _context?.Dispose();

    public IQueryable<AppUser> FindAll(Expression<Func<AppUser, bool>>? predicate = null,
        params Expression<Func<AppUser, object>>[] includeProperties)
    {
        IQueryable<AppUser> items = _context.Set<AppUser>().AsNoTracking(); // Important: Always include AsNoTracking for Query Side
        if (includeProperties != null)
            foreach (var includeProperty in includeProperties)
                items = items.Include(includeProperty);

        if (predicate is not null)
            items = items.Where(predicate);

        return items;
    }

    public async Task<AppUser?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<AppUser, object>>[] includeProperties)
        => await FindAll(null, includeProperties).AsTracking().SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

    public async Task<AppUser?> FindSingleAsync(Expression<Func<AppUser, bool>>? predicate = null, CancellationToken cancellationToken = default, params Expression<Func<AppUser, object>>[] includeProperties)
        => await FindAll(null, includeProperties).AsTracking().SingleOrDefaultAsync(predicate, cancellationToken);

    public void Add(AppUser entity)
        => _context.Add(entity);

    public void Remove(AppUser entity)
        => _context.Set<AppUser>().Remove(entity);

    public void RemoveMultiple(List<AppUser> entities)
        => _context.Set<AppUser>().RemoveRange(entities);

    public void Update(AppUser entity)
        => _context.Set<AppUser>().Update(entity);
}
