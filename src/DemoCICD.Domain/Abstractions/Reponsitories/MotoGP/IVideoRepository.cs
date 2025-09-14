using DemoCICD.Domain.Abstractions.Reponsitories;
using DemoCICD.Domain.Entities.MotoGP.MediaNews;

namespace DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;

public interface IVideoRepository : IRepositoryBase<Video, Guid>
{
    Task<IEnumerable<Video>> GetPublishedVideosAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Video>> GetVideosByTypeAsync(VideoType type, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Video>> GetFeaturedVideosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Video>> GetVideosByPlatformAsync(string platform, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Video>> GetRelatedVideosAsync(Guid? seasonId = null, Guid? raceId = null, Guid? teamId = null, Guid? riderId = null, int limit = 5, CancellationToken cancellationToken = default);
    Task<IEnumerable<Video>> SearchVideosAsync(string searchTerm, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Video>> GetVideosByTagAsync(string tag, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Video?> GetVideoByExternalIdAsync(string externalVideoId, CancellationToken cancellationToken = default);
}