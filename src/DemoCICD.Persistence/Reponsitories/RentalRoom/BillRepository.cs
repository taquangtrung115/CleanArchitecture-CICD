using DemoCICD.Contract.Enumerations;
using DemoCICD.Domain.Abstractions.Dappers.Repositories.RentalRoom;
using DemoCICD.Domain.Entities.RentalRoom.Bills;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Reponsitories.RentalRoom;

/// <summary>
/// EF Core Repository implementation cho Bill entity
/// </summary>
public class BillRepository : AuditableRepositoryBase<Bill, Guid>, IBillRepository
{
    private readonly ApplicationDbContext _context;

    public BillRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Bill>> GetBillsByRoomAsync(Guid roomId)
    {
        return await FindAll(b => b.RoomId == roomId)
            .OrderByDescending(b => b.Year)
            .ThenByDescending(b => b.Month)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Bill>> GetBillsByProfileAsync(Guid profileId)
    {
        return await FindAll(b => b.ProfileId == profileId)
            .OrderByDescending(b => b.Year)
            .ThenByDescending(b => b.Month)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Bill>> GetBillsByMonthYearAsync(int month, int year)
    {
        return await FindAll(b => b.Month == month && b.Year == year)
            .OrderBy(b => b.BillNumber)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Bill>> GetBillsByStatusAsync(BillStatus status)
    {
        return await FindAll(b => b.Status == status)
            .OrderByDescending(b => b.DueDate)
            .ToListAsync();
    }

    public async Task<Bill?> GetBillWithDetailsAsync(Guid billId)
    {
        return await _context.Set<Bill>()
            .Where(b => b.Id == billId && !b.IsDeleted)
            .Include(b => b.BillDetails.Where(d => !d.IsDeleted))
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
}
