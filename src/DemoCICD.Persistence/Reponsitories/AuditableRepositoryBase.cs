using DemoCICD.Domain.Abstractions.Entities;
using DemoCICD.Domain.Abstractions.Reponsitories;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Reponsitories;

public class AuditableRepositoryBase<TEntity, TKey> : IRepositoryBase<TEntity, TKey>, IDisposable
    where TEntity : AuditableEntity<TKey>
{
    private readonly ApplicationDbContext _context;

    public AuditableRepositoryBase(ApplicationDbContext context)
        => _context = context;

    public void Dispose()
        => _context?.Dispose();

    public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate = null,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> items = _context.Set<TEntity>().AsNoTracking(); // Importance Always include AsNoTracking for Query Side
        
        // Apply soft delete filter automatically
        items = items.Where(e => !e.IsDeleted);
        
        if (includeProperties != null)
            foreach (var includeProperty in includeProperties)
                items = items.Include(includeProperty);

        if (predicate is not null)
            items = items.Where(predicate);

        return items;
    }

    public IQueryable<TEntity> FindAllIncludingDeleted(Expression<Func<TEntity, bool>>? predicate = null,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> items = _context.Set<TEntity>().AsNoTracking();
        
        if (includeProperties != null)
            foreach (var includeProperty in includeProperties)
                items = items.Include(includeProperty);

        if (predicate is not null)
            items = items.Where(predicate);

        return items;
    }

    public async Task<TEntity?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
        => await FindAll(null, includeProperties).AsTracking().SingleOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken);

    public async Task<TEntity?> FindSingleAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
        => await FindAll(predicate, includeProperties).AsTracking().SingleOrDefaultAsync(cancellationToken);

    public void Add(TEntity entity)
    {
        entity.SetCreatedAudit(); // Set audit fields on add
        _context.Add(entity);
    }

    public void Update(TEntity entity)
    {
        entity.SetUpdatedAudit(); // Set audit fields on update
        _context.Set<TEntity>().Update(entity);
    }

    public void Remove(TEntity entity)
    {
        // Soft delete instead of hard delete
        entity.SetDeletedAudit();
        _context.Set<TEntity>().Update(entity);
    }

    public void RemoveMultiple(List<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.SetDeletedAudit();
        }
        _context.Set<TEntity>().UpdateRange(entities);
    }

    public void HardDelete(TEntity entity)
        => _context.Set<TEntity>().Remove(entity);

    public void HardDeleteMultiple(List<TEntity> entities)
        => _context.Set<TEntity>().RemoveRange(entities);
}