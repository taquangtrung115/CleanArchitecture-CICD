using DemoCICD.Application.Abstractions;
using DemoCICD.Domain.Entities.Identity;
using DemoCICD.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DemoCICD.Infrastructure.Authentication;

public class ActionInFunctionManagementService : IActionInFunctionManagementService
{
    private readonly ApplicationDbContext _context;

    public ActionInFunctionManagementService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(string ActionId, string FunctionId)?> CreateActionInFunctionAsync(string actionId, string functionId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if ActionInFunction already exists
            var existingActionInFunction = await _context.ActionInFunctions
                .FirstOrDefaultAsync(af => af.ActionId == actionId && af.FunctionId == functionId, cancellationToken);

            if (existingActionInFunction != null)
            {
                Log.Warning("ActionInFunction creation failed - combination '{ActionId}'-'{FunctionId}' already exists", actionId, functionId);
                return null;
            }

            // Verify that Action and Function exist
            var actionExists = await _context.Actions.AnyAsync(a => a.Id == actionId, cancellationToken);
            var functionExists = await _context.Functions.AnyAsync(f => f.Id == functionId, cancellationToken);

            if (!actionExists)
            {
                Log.Warning("ActionInFunction creation failed - Action '{ActionId}' does not exist", actionId);
                return null;
            }

            if (!functionExists)
            {
                Log.Warning("ActionInFunction creation failed - Function '{FunctionId}' does not exist", functionId);
                return null;
            }

            var actionInFunction = new ActionInFunction
            {
                ActionId = actionId,
                FunctionId = functionId
            };

            _context.ActionInFunctions.Add(actionInFunction);
            await _context.SaveChangesAsync(cancellationToken);

            Log.Information("ActionInFunction created successfully: {ActionId} - {FunctionId}", actionId, functionId);
            return (actionInFunction.ActionId, actionInFunction.FunctionId);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating ActionInFunction: {ActionId} - {FunctionId}", actionId, functionId);
            throw;
        }
    }

    public async Task<bool> DeleteActionInFunctionAsync(string actionId, string functionId, CancellationToken cancellationToken = default)
    {
        try
        {
            var actionInFunction = await _context.ActionInFunctions
                .FirstOrDefaultAsync(af => af.ActionId == actionId && af.FunctionId == functionId, cancellationToken);

            if (actionInFunction == null)
            {
                Log.Warning("ActionInFunction deletion failed - not found: {ActionId} - {FunctionId}", actionId, functionId);
                return false;
            }

            _context.ActionInFunctions.Remove(actionInFunction);
            await _context.SaveChangesAsync(cancellationToken);

            Log.Information("ActionInFunction deleted successfully: {ActionId} - {FunctionId}", actionId, functionId);
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting ActionInFunction: {ActionId} - {FunctionId}", actionId, functionId);
            throw;
        }
    }

    public async Task<object?> GetActionInFunctionAsync(string actionId, string functionId, CancellationToken cancellationToken = default)
    {
        try
        {
            var actionInFunction = await _context.ActionInFunctions
                .Include(af => af.Action)
                .Include(af => af.Function)
                .Where(af => af.ActionId == actionId && af.FunctionId == functionId)
                .Select(af => new
                {
                    af.ActionId,
                    af.FunctionId,
                    ActionName = af.Action.Name,
                    FunctionName = af.Function.Name
                })
                .FirstOrDefaultAsync(cancellationToken);

            return actionInFunction;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting ActionInFunction: {ActionId} - {FunctionId}", actionId, functionId);
            throw;
        }
    }

    public async Task<(IEnumerable<object> ActionInFunctions, int TotalCount)> GetActionInFunctionsAsync(int page, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _context.ActionInFunctions
                .Include(af => af.Action)
                .Include(af => af.Function)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(af => 
                    af.Action.Name.Contains(searchTerm) || 
                    af.Function.Name.Contains(searchTerm) ||
                    af.ActionId.Contains(searchTerm) ||
                    af.FunctionId.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var actionInFunctions = await query
                .OrderBy(af => af.FunctionId)
                .ThenBy(af => af.ActionId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(af => new
                {
                    af.ActionId,
                    af.FunctionId,
                    ActionName = af.Action.Name,
                    FunctionName = af.Function.Name
                })
                .ToListAsync(cancellationToken);

            return (actionInFunctions, totalCount);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting ActionInFunctions with pagination");
            throw;
        }
    }

    public async Task<bool> ActionInFunctionExistsAsync(string actionId, string functionId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.ActionInFunctions
                .AnyAsync(af => af.ActionId == actionId && af.FunctionId == functionId, cancellationToken);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error checking ActionInFunction existence: {ActionId} - {FunctionId}", actionId, functionId);
            throw;
        }
    }
}
