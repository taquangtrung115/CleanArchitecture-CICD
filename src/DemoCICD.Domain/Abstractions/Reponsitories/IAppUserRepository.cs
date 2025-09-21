using System.Linq.Expressions;
using DemoCICD.Domain.Entities.Identity;

namespace DemoCICD.Domain.Abstractions.Reponsitories;

public interface IAppUserRepository
{
    Task<AppUser?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<AppUser, object>>[] includeProperties);
    Task<AppUser?> FindSingleAsync(Expression<Func<AppUser, bool>>? predicate = null, CancellationToken cancellationToken = default, params Expression<Func<AppUser, object>>[] includeProperties);
    IQueryable<AppUser> FindAll(Expression<Func<AppUser, bool>>? predicate = null, params Expression<Func<AppUser, object>>[] includeProperties);
    void Add(AppUser entity);
    void Update(AppUser entity);
    void Remove(AppUser entity);
    void RemoveMultiple(List<AppUser> entities);
}