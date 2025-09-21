using DemoCICD.Application.Abstractions;
using DemoCICD.Contract.Services.V1.Identity;
using DemoCICD.Domain.Entities.Identity;
using DemoCICD.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DemoCICD.Infrastructure.Authentication;

public class PositionManagementService : IPositionManagementService
{
    private readonly ApplicationDbContext _context;

    public PositionManagementService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(Guid PositionId, string Name, string Code)?> CreatePositionAsync(string name, string description, string code, int level = 1, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if position code already exists
            var existingPosition = await _context.Positions
                .FirstOrDefaultAsync(p => p.Code == code, cancellationToken);

            if (existingPosition != null)
            {
                Log.Warning("Position creation failed - code '{Code}' already exists", code);
                return null;
            }

            var position = new Position
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                Code = code,
                Level = level,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Positions.Add(position);
            await _context.SaveChangesAsync(cancellationToken);

            Log.Information("Position created successfully: {Name} ({Code})", name, code);
            return (position.Id, position.Name, position.Code);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating position: {Name} ({Code})", name, code);
            throw;
        }
    }

    public async Task<(Guid PositionId, string Name, string Code)?> UpdatePositionAsync(Guid positionId, string name, string description, string code, int level, bool isActive, CancellationToken cancellationToken = default)
    {
        try
        {
            var position = await _context.Positions
                .FirstOrDefaultAsync(p => p.Id == positionId, cancellationToken);

            if (position == null)
            {
                Log.Warning("Position update failed - position with ID '{PositionId}' not found", positionId);
                return null;
            }

            // Check if code already exists for another position
            var existingPosition = await _context.Positions
                .FirstOrDefaultAsync(p => p.Code == code && p.Id != positionId, cancellationToken);

            if (existingPosition != null)
            {
                Log.Warning("Position update failed - code '{Code}' already exists for another position", code);
                return null;
            }

            position.Name = name;
            position.Description = description;
            position.Code = code;
            position.Level = level;
            position.IsActive = isActive;
            position.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            Log.Information("Position updated successfully: {Name} ({Code})", name, code);
            return (position.Id, position.Name, position.Code);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error updating position: {PositionId}", positionId);
            throw;
        }
    }

    public async Task<bool> DeletePositionAsync(Guid positionId, CancellationToken cancellationToken = default)
    {
        try
        {
            var position = await _context.Positions
                .FirstOrDefaultAsync(p => p.Id == positionId, cancellationToken);

            if (position == null)
            {
                Log.Warning("Position deletion failed - position with ID '{PositionId}' not found", positionId);
                return false;
            }

            // Check if any users are assigned to this position
            var usersWithPosition = await _context.AppUses
                .AnyAsync(u => u.PositionId == positionId, cancellationToken);

            if (usersWithPosition)
            {
                Log.Warning("Position deletion failed - position '{PositionId}' is assigned to users", positionId);
                return false;
            }

            _context.Positions.Remove(position);
            await _context.SaveChangesAsync(cancellationToken);

            Log.Information("Position deleted successfully: {PositionId}", positionId);
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting position: {PositionId}", positionId);
            throw;
        }
    }

    public async Task<object?> GetPositionByIdAsync(Guid positionId, CancellationToken cancellationToken = default)
    {
        try
        {
            var position = await _context.Positions
                .FirstOrDefaultAsync(p => p.Id == positionId, cancellationToken);

            if (position == null)
                return null;

            return new Response.PositionDetails(
                position.Id,
                position.Name,
                position.Description ?? string.Empty,
                position.Code,
                position.Level,
                position.IsActive,
                position.CreatedAt,
                position.UpdatedAt);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting position by ID: {PositionId}", positionId);
            throw;
        }
    }

    public async Task<(IEnumerable<object> Positions, int TotalCount)> GetPositionsAsync(int page, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _context.Positions.AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTermLower = searchTerm.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(searchTermLower) ||
                                       p.Code.ToLower().Contains(searchTermLower) ||
                                       (p.Description != null && p.Description.ToLower().Contains(searchTermLower)));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var positions = await query
                .OrderBy(p => p.Level)
                .ThenBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new Response.PositionSummary(
                    p.Id,
                    p.Name,
                    p.Code,
                    p.Level,
                    p.IsActive))
                .ToListAsync(cancellationToken);

            return (positions.Cast<object>(), totalCount);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting positions");
            throw;
        }
    }

    public async Task<IEnumerable<object>> GetActivePositionsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var positions = await _context.Positions
                .Where(p => p.IsActive)
                .OrderBy(p => p.Level)
                .ThenBy(p => p.Name)
                .Select(p => new Response.PositionSummary(
                    p.Id,
                    p.Name,
                    p.Code,
                    p.Level,
                    p.IsActive))
                .ToListAsync(cancellationToken);

            return positions.Cast<object>();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting active positions");
            throw;
        }
    }

    public async Task<bool> PositionExistsAsync(Guid positionId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Positions
                .AnyAsync(p => p.Id == positionId, cancellationToken);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error checking if position exists: {PositionId}", positionId);
            throw;
        }
    }

    public async Task<bool> PositionCodeExistsAsync(string code, Guid? excludePositionId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _context.Positions.Where(p => p.Code == code);
            
            if (excludePositionId.HasValue)
            {
                query = query.Where(p => p.Id != excludePositionId.Value);
            }

            return await query.AnyAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error checking if position code exists: {Code}", code);
            throw;
        }
    }
}