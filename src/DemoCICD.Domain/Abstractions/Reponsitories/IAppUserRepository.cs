using System.Linq.Expressions;
using DemoCICD.Domain.Entities.Identity;

namespace DemoCICD.Domain.Abstractions.Reponsitories;

public interface IAppUserRepository : IRepositoryBase<AppUser, Guid>
{
   
}
