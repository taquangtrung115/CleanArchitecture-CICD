using DemoCICD.Domain.Abstractions.Reponsitories;
using DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;

namespace DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;

public interface IBikeRepository : IRepositoryBase<Bike, Guid>
{
    Task<IEnumerable<Bike>> GetActiveeBikesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Bike>> GetBikesByTeamAsync(Guid teamId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Bike>> GetBikesByManufacturerAsync(string manufacturer, CancellationToken cancellationToken = default);
    Task<IEnumerable<Bike>> GetBikesByYearAsync(int year, CancellationToken cancellationToken = default);
    Task<IEnumerable<Bike>> GetUnassignedBikesAsync(CancellationToken cancellationToken = default);
    Task<Bike?> GetBikeByChassisNumberAsync(string chassisNumber, CancellationToken cancellationToken = default);
    Task<Bike?> GetBikeByEngineNumberAsync(string engineNumber, CancellationToken cancellationToken = default);
}