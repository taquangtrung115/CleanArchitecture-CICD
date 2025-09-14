using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Domain.Entities.MotoGP.MediaNews;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Reponsitories.MotoGP;

public class VideoRepository : AuditableRepositoryBase<Video, Guid>, IVideoRepository
{
    private readonly ApplicationDbContext _context;

    public VideoRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Video>> GetPublishedVideosAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await FindAll(v => v.Status == VideoStatus.Published)
            .OrderByDescending(v => v.PublishedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Video>> GetVideosByTypeAsync(VideoType type, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await FindAll(v => v.Type == type && v.Status == VideoStatus.Published)
            .OrderByDescending(v => v.PublishedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Video>> GetFeaturedVideosAsync(CancellationToken cancellationToken = default)
    {
        return await FindAll(v => v.IsFeatured && v.Status == VideoStatus.Published)
            .OrderByDescending(v => v.PublishedDate)
            .Take(6)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Video>> GetVideosByPlatformAsync(string platform, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await FindAll(v => v.Platform == platform && v.Status == VideoStatus.Published)
            .OrderByDescending(v => v.PublishedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Video>> GetRelatedVideosAsync(Guid? seasonId = null, Guid? raceId = null, Guid? teamId = null, Guid? riderId = null, int limit = 5, CancellationToken cancellationToken = default)
    {
        var query = FindAll(v => v.Status == VideoStatus.Published);

        if (seasonId.HasValue)
            query = query.Where(v => v.RelatedSeasonId == seasonId.Value);
        if (raceId.HasValue)
            query = query.Where(v => v.RelatedRaceId == raceId.Value);
        if (teamId.HasValue)
            query = query.Where(v => v.RelatedTeamId == teamId.Value);
        if (riderId.HasValue)
            query = query.Where(v => v.RelatedRiderId == riderId.Value);

        return await query
            .OrderByDescending(v => v.PublishedDate)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Video>> SearchVideosAsync(string searchTerm, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetPublishedVideosAsync(page, pageSize, cancellationToken);

        return await FindAll(v => v.Status == VideoStatus.Published && 
                               (v.Title.Contains(searchTerm) || 
                                v.Description.Contains(searchTerm)))
            .OrderByDescending(v => v.PublishedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Video>> GetVideosByTagAsync(string tag, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tag))
            return Enumerable.Empty<Video>();

        var normalizedTag = tag.Trim().ToLowerInvariant();
        return await FindAll(v => v.Status == VideoStatus.Published && v.Tags.Contains(normalizedTag))
            .OrderByDescending(v => v.PublishedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Video?> GetVideoByExternalIdAsync(string externalVideoId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(externalVideoId))
            return null;

        return await FindAll(v => v.ExternalVideoId == externalVideoId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}