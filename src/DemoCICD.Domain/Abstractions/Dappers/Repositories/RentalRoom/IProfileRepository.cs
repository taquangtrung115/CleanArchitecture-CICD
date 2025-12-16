using DemoCICD.Domain.Entities.RentalRoom.Profiles;

namespace DemoCICD.Domain.Abstractions.Dappers.Repositories.RentalRoom;

/// <summary>
/// Repository interface cho Profile entity - EF Core version
/// </summary>
public interface IProfileRepository
{
    /// <summary>
    /// L?y danh sách ng??i thuê theo phòng
    /// </summary>
    Task<IReadOnlyList<Profile>> GetProfilesByRoomAsync(Guid roomId);

    /// <summary>
    /// L?y danh sách ng??i thuê ?ang ho?t ??ng
    /// </summary>
    Task<IReadOnlyList<Profile>> GetActiveProfilesAsync();

    /// <summary>
    /// L?y ng??i thuê theo CMND
    /// </summary>
    Task<Profile?> GetProfileByIdentityCardAsync(string identityCard);
}
