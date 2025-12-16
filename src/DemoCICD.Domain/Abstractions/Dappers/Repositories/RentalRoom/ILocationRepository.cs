using DemoCICD.Domain.Entities.RentalRoom.Locations;

namespace DemoCICD.Domain.Abstractions.Dappers.Repositories.RentalRoom;

/// <summary>
/// Repository interface cho Location entity - EF Core version
/// </summary>
public interface ILocationRepository
{
    /// <summary>
    /// L?y danh sách ??a ch? theo thành ph?
    /// </summary>
    Task<IReadOnlyList<Location>> GetLocationsByCityAsync(string city);

    /// <summary>
    /// L?y danh sách ??a ch? theo qu?n
    /// </summary>
    Task<IReadOnlyList<Location>> GetLocationsByDistrictAsync(string district);
}
