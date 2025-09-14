using DemoCICD.Domain.Abstractions.Reponsitories;
using DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;

namespace DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;

public interface IRiderRepository : IRepositoryBase<Rider, Guid>
{
    Task<IEnumerable<Rider>> GetActiveRidersAsync(CancellationToken cancellationToken = default);
    Task<Rider?> GetRiderWithTeamHistoryAsync(Guid riderId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Rider>> GetRidersByTeamAsync(Guid teamId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Rider>> GetRidersByNationalityAsync(string countryCode, CancellationToken cancellationToken = default);
    Task<Rider?> GetRiderByRacingNumberAsync(int racingNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Rider>> GetRetiredRidersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Rider>> GetRidersWithoutTeamAsync(CancellationToken cancellationToken = default);
    Task<bool> IsRacingNumberAvailableAsync(int racingNumber, Guid? excludeRiderId = null, CancellationToken cancellationToken = default);
}