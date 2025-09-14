using DemoCICD.Domain.Abstractions.Reponsitories;
using DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;

namespace DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;

public interface ITeamRepository : IRepositoryBase<Team, Guid>
{
    Task<IEnumerable<Team>> GetActiveTeamsAsync(CancellationToken cancellationToken = default);
    Task<Team?> GetTeamWithRidersAsync(Guid teamId, CancellationToken cancellationToken = default);
    Task<Team?> GetTeamWithBikesAsync(Guid teamId, CancellationToken cancellationToken = default);
    Task<Team?> GetTeamWithRidersAndBikesAsync(Guid teamId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Team>> GetTeamsByCountryAsync(string countryCode, CancellationToken cancellationToken = default);
    Task<Team?> GetTeamByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Team?> GetTeamByShortNameAsync(string shortName, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, Guid? excludeTeamId = null, CancellationToken cancellationToken = default);
}