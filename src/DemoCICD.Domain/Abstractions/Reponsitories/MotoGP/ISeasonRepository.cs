using DemoCICD.Domain.Abstractions.Reponsitories;
using DemoCICD.Domain.Entities.MotoGP.RaceManagement;

namespace DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;

public interface ISeasonRepository : IRepositoryBase<Season, Guid>
{
    Task<Season?> GetCurrentSeasonAsync(CancellationToken cancellationToken = default);
    Task<Season?> GetSeasonByYearAsync(int year, CancellationToken cancellationToken = default);
    Task<IEnumerable<Season>> GetSeasonsAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Season?> GetSeasonWithRacesAsync(Guid seasonId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int year, CancellationToken cancellationToken = default);
}