using DemoCICD.Application.Abstractions;
using DemoCICD.Domain.Entities.Identity;
using DemoCICD.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DemoCICD.Infrastructure.Authentication;

public class FunctionManagementService : IFunctionManagementService
{
    private readonly ApplicationDbContext _context;

    public FunctionManagementService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<object>> GetActiveFunctionsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var functions = await _context.Functions
                .Where(f => f.IsActive == true)
                .OrderBy(f => f.SortOrder)
                .ThenBy(f => f.Name)
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.IsActive
                })
                .ToListAsync(cancellationToken);

            return functions.Cast<object>();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting active functions");
            throw;
        }
    }

    public async Task<(IEnumerable<object> Functions, int TotalCount)> GetFunctionsAsync(int page, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _context.Functions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(f => f.Name.Contains(searchTerm) || f.Id.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var functions = await query
                .OrderBy(f => f.SortOrder)
                .ThenBy(f => f.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.IsActive
                })
                .ToListAsync(cancellationToken);

            return (functions.Cast<object>(), totalCount);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting functions with pagination. Page: {Page}, PageSize: {PageSize}", page, pageSize);
            throw;
        }
    }

    public async Task<object?> GetFunctionByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var function = await _context.Functions
                .Where(f => f.Id == id)
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken);

            return function;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting function by ID: {Id}", id);
            throw;
        }
    }

    public async Task<bool> FunctionExistsAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Functions
                .AnyAsync(f => f.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error checking if function exists: {Id}", id);
            throw;
        }
    }
}