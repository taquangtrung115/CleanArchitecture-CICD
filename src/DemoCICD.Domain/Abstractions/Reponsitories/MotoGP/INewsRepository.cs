using DemoCICD.Domain.Abstractions.Reponsitories;
using DemoCICD.Domain.Entities.MotoGP.MediaNews;

namespace DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;

public interface INewsRepository : IRepositoryBase<News, Guid>
{
    Task<IEnumerable<News>> GetPublishedNewsAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<News>> GetNewsByCategoryAsync(NewsCategory category, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<News>> GetFeaturedNewsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<News>> GetBreakingNewsAsync(CancellationToken cancellationToken = default);
    Task<News?> GetNewsBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<News>> GetNewsByAuthorAsync(Guid authorId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<News>> GetRelatedNewsAsync(Guid? seasonId = null, Guid? raceId = null, Guid? teamId = null, Guid? riderId = null, int limit = 5, CancellationToken cancellationToken = default);
    Task<IEnumerable<News>> SearchNewsAsync(string searchTerm, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<News>> GetNewsByTagAsync(string tag, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<bool> SlugExistsAsync(string slug, Guid? excludeNewsId = null, CancellationToken cancellationToken = default);
}