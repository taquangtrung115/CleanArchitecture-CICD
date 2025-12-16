using DemoCICD.Domain.Abstractions.Dappers.Repositories.RentalRoom;
using DemoCICD.Domain.Entities.RentalRoom.Profiles;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Reponsitories.RentalRoom;

/// <summary>
/// EF Core Repository implementation cho Profile entity
/// </summary>
public class ProfileRepository : AuditableRepositoryBase<Profile, Guid>, IProfileRepository
{
    private readonly ApplicationDbContext _context;

    public ProfileRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Profile>> GetProfilesByRoomAsync(Guid roomId)
    {
        return await FindAll(p => p.RoomId == roomId)
            .OrderBy(p => p.FullName)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Profile>> GetActiveProfilesAsync()
    {
        return await FindAll(p => p.IsActive)
            .OrderBy(p => p.FullName)
            .ToListAsync();
    }

    public async Task<Profile?> GetProfileByIdentityCardAsync(string identityCard)
    {
        return await FindAll(p => p.IdentityCard == identityCard)
            .FirstOrDefaultAsync();
    }
}
