using DemoCICD.Application.Abstractions;
using DemoCICD.Domain.Entities.Identity;
using DemoCICD.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DemoCICD.Infrastructure.Authentication;

public class ActionManagementService : IActionManagementService
{
    private readonly ApplicationDbContext _context;

    public ActionManagementService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(string Id, string Name)?> CreateActionAsync(string id, string name, int? sortOrder = null, bool? isActive = true, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if action ID already exists
            var existingAction = await _context.Actions
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if (existingAction != null)
            {
                Log.Warning("Action creation failed - ID '{Id}' already exists", id);
                return null;
            }

            var action = new DemoCICD.Domain.Entities.Identity.Action
            {
                Id = id,
                Name = name,
                SortOrder = sortOrder,
                IsActive = isActive
            };

            _context.Actions.Add(action);
            await _context.SaveChangesAsync(cancellationToken);

            Log.Information("Action created successfully: {Name} ({Id})", name, id);
            return (action.Id, action.Name);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating action: {Name} ({Id})", name, id);
            throw;
        }
    }

    public async Task<(string Id, string Name)?> UpdateActionAsync(string id, string name, int? sortOrder = null, bool? isActive = true, CancellationToken cancellationToken = default)
    {
        try
        {
            var action = await _context.Actions
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if (action == null)
            {
                Log.Warning("Action update failed - ID '{Id}' not found", id);
                return null;
            }

            action.Name = name;
            action.SortOrder = sortOrder;
            action.IsActive = isActive;

            await _context.SaveChangesAsync(cancellationToken);

            Log.Information("Action updated successfully: {Name} ({Id})", name, id);
            return (action.Id, action.Name);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error updating action: {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteActionAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var action = await _context.Actions
                .Include(a => a.Permissions)
                .Include(a => a.ActionInFunctions)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if (action == null)
            {
                Log.Warning("Action deletion failed - ID '{Id}' not found", id);
                return false;
            }

            // Check if action is referenced by permissions or action-in-functions
            if (action.Permissions?.Any() == true || action.ActionInFunctions?.Any() == true)
            {
                Log.Warning("Action deletion failed - ID '{Id}' is referenced by permissions or functions", id);
                return false;
            }

            _context.Actions.Remove(action);
            var rowsAffected = await _context.SaveChangesAsync(cancellationToken);

            Log.Information("Action deleted successfully: {Id}", id);
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting action: {Id}", id);
            return false;
        }
    }

    public async Task<object?> GetActionByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var action = await _context.Actions
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.SortOrder,
                    a.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken);

            return action;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting action by ID: {Id}", id);
            throw;
        }
    }

    public async Task<(IEnumerable<object> Actions, int TotalCount)> GetActionsAsync(int page, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _context.Actions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(a => a.Name.Contains(searchTerm) || a.Id.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var actions = await query
                .OrderBy(a => a.SortOrder)
                .ThenBy(a => a.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.SortOrder,
                    a.IsActive
                })
                .ToListAsync(cancellationToken);

            return (actions.Cast<object>(), totalCount);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting actions with pagination. Page: {Page}, PageSize: {PageSize}", page, pageSize);
            throw;
        }
    }

    public async Task<IEnumerable<object>> GetActiveActionsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var actions = await _context.Actions
                .Where(a => a.IsActive == true)
                .OrderBy(a => a.SortOrder)
                .ThenBy(a => a.Name)
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.SortOrder,
                    a.IsActive
                })
                .ToListAsync(cancellationToken);

            return actions.Cast<object>();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting active actions");
            throw;
        }
    }

    public async Task<bool> ActionExistsAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Actions
                .AnyAsync(a => a.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error checking if action exists: {Id}", id);
            throw;
        }
    }
}