using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Domain.Entities.MotoGP.MediaNews;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Reponsitories.MotoGP;

public class NewsRepository : AuditableRepositoryBase<News, Guid>, INewsRepository
{
    private readonly ApplicationDbContext _context;

    public NewsRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<News>> GetPublishedNewsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await FindAll(n => n.Status == NewsStatus.Published)
            .OrderByDescending(n => n.PublishedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<News>> GetNewsByCategoryAsync(NewsCategory category, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await FindAll(n => n.Category == category && n.Status == NewsStatus.Published)
            .OrderByDescending(n => n.PublishedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<News>> GetFeaturedNewsAsync(CancellationToken cancellationToken = default)
    {
        return await FindAll(n => n.IsFeatured && n.Status == NewsStatus.Published)
            .OrderByDescending(n => n.PublishedDate)
            .Take(5)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<News>> GetBreakingNewsAsync(CancellationToken cancellationToken = default)
    {
        return await FindAll(n => n.IsBreaking && n.Status == NewsStatus.Published)
            .OrderByDescending(n => n.PublishedDate)
            .Take(3)
            .ToListAsync(cancellationToken);
    }

    public async Task<News?> GetNewsBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await FindAll(n => n.Slug == slug && n.Status == NewsStatus.Published)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<News>> GetNewsByAuthorAsync(Guid authorId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await FindAll(n => n.AuthorId == authorId && n.Status == NewsStatus.Published)
            .OrderByDescending(n => n.PublishedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<News>> GetRelatedNewsAsync(Guid? seasonId = null, Guid? raceId = null, Guid? teamId = null, Guid? riderId = null, int limit = 5, CancellationToken cancellationToken = default)
    {
        var query = FindAll(n => n.Status == NewsStatus.Published);

        if (seasonId.HasValue)
            query = query.Where(n => n.RelatedSeasonId == seasonId.Value);
        if (raceId.HasValue)
            query = query.Where(n => n.RelatedRaceId == raceId.Value);
        if (teamId.HasValue)
            query = query.Where(n => n.RelatedTeamId == teamId.Value);
        if (riderId.HasValue)
            query = query.Where(n => n.RelatedRiderId == riderId.Value);

        return await query
            .OrderByDescending(n => n.PublishedDate)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<News>> SearchNewsAsync(string searchTerm, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetPublishedNewsAsync(page, pageSize, cancellationToken);

        return await FindAll(n => n.Status == NewsStatus.Published && 
                               (n.Title.Contains(searchTerm) || 
                                n.Summary.Contains(searchTerm) || 
                                n.Content.Contains(searchTerm)))
            .OrderByDescending(n => n.PublishedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<News>> GetNewsByTagAsync(string tag, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tag))
            return Enumerable.Empty<News>();

        var normalizedTag = tag.Trim().ToLowerInvariant();
        return await FindAll(n => n.Status == NewsStatus.Published && n.Tags.Contains(normalizedTag))
            .OrderByDescending(n => n.PublishedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> SlugExistsAsync(string slug, Guid? excludeNewsId = null, CancellationToken cancellationToken = default)
    {
        var query = FindAll(n => n.Slug == slug);
        if (excludeNewsId.HasValue)
            query = query.Where(n => n.Id != excludeNewsId.Value);
        
        return await query.AnyAsync(cancellationToken);
    }
}