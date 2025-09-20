using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;

namespace DemoCICD.Contract.Services.V1.MotoGP.Video;

public static class Query
{
    public record GetVideosQuery(
        int PageIndex = 1,
        int PageSize = 10,
        string? Type = null,
        string? SearchTerm = null,
        string? Platform = null,
        bool? IsFeatured = null
    ) : IQuery<PagedResult<Response.VideoResponse>>;

    public record GetVideoByIdQuery(Guid Id) : IQuery<Response.VideoResponse>;

    public record GetVideoByExternalIdQuery(string ExternalVideoId) : IQuery<Response.VideoResponse>;

    public record GetVideosByTypeQuery(
        string Type,
        int PageIndex = 1,
        int PageSize = 10
    ) : IQuery<PagedResult<Response.VideoResponse>>;

    public record GetFeaturedVideosQuery() : IQuery<IEnumerable<Response.VideoResponse>>;

    public record GetVideosByPlatformQuery(
        string Platform,
        int PageIndex = 1,
        int PageSize = 10
    ) : IQuery<PagedResult<Response.VideoResponse>>;

    public record GetRelatedVideosQuery(
        Guid? SeasonId = null,
        Guid? RaceId = null,
        Guid? TeamId = null,
        Guid? RiderId = null,
        int Limit = 5
    ) : IQuery<IEnumerable<Response.VideoResponse>>;

    public record SearchVideosQuery(
        string SearchTerm,
        int PageIndex = 1,
        int PageSize = 10
    ) : IQuery<PagedResult<Response.VideoResponse>>;

    public record GetVideosByTagQuery(
        string Tag,
        int PageIndex = 1,
        int PageSize = 10
    ) : IQuery<PagedResult<Response.VideoResponse>>;
}