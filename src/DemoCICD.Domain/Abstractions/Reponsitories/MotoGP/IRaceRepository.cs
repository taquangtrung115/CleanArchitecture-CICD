using DemoCICD.Domain.Abstractions.Reponsitories;
using DemoCICD.Domain.Entities.MotoGP.RaceManagement;

namespace DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;

public interface IRaceRepository : IRepositoryBase<Race, Guid>
{
    Task<IEnumerable<Race>> GetRacesBySeasonAsync(Guid seasonId, CancellationToken cancellationToken = default);
    Task<Race?> GetRaceWithEntriesAsync(Guid raceId, CancellationToken cancellationToken = default);
    Task<Race?> GetNextRaceAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Race>> GetRacesByStatusAsync(RaceStatus status, CancellationToken cancellationToken = default);
    Task<Race?> GetRaceBySeasonAndRoundAsync(Guid seasonId, int roundNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Race>> GetRacesByCountryAsync(string countryCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<Race>> GetRacesInDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}