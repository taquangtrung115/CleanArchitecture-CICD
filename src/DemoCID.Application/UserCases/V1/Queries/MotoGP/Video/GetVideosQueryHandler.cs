using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Video;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Domain.Entities.MotoGP.MediaNews;
using MediatR;

namespace DemoCICD.Application.UserCases.V1.Queries.MotoGP.Video;

public sealed class GetVideosQueryHandler : IRequestHandler<Query.GetVideosQuery, Result<PagedResult<Response.VideoResponse>>>
{
    private readonly IVideoRepository _videoRepository;

    public GetVideosQueryHandler(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository;
    }

    public async Task<Result<PagedResult<Response.VideoResponse>>> Handle(Query.GetVideosQuery request, CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<Domain.Entities.MotoGP.MediaNews.Video> videos;

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                videos = await _videoRepository.SearchVideosAsync(request.SearchTerm, request.PageIndex, request.PageSize, cancellationToken);
            }
            else if (!string.IsNullOrEmpty(request.Type) && Enum.TryParse<VideoType>(request.Type, out var type))
            {
                videos = await _videoRepository.GetVideosByTypeAsync(type, request.PageIndex, request.PageSize, cancellationToken);
            }
            else if (!string.IsNullOrEmpty(request.Platform))
            {
                videos = await _videoRepository.GetVideosByPlatformAsync(request.Platform, request.PageIndex, request.PageSize, cancellationToken);
            }
            else if (request.IsFeatured == true)
            {
                var featuredVideos = await _videoRepository.GetFeaturedVideosAsync(cancellationToken);
                videos = featuredVideos.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize);
            }
            else
            {
                videos = await _videoRepository.GetPublishedVideosAsync(request.PageIndex, request.PageSize, cancellationToken);
            }

            var videoResponses = videos.Select(v => new Response.VideoResponse(
                v.Id,
                v.Title,
                v.Description,
                v.Type.ToString(),
                v.Status.ToString(),
                v.VideoUrl,
                v.ThumbnailUrl,
                v.FormattedDuration,
                v.PublishedDate,
                v.ViewCount,
                v.IsFeatured,
                v.Platform,
                v.ExternalVideoId,
                v.RelatedSeasonId,
                v.RelatedRaceId,
                v.RelatedTeamId,
                v.RelatedRiderId,
                v.Tags.ToList(),
                v.CreatedAt,
                v.UpdatedAt
            ));

            var totalCount = videos.Count();
            
            var result = PagedResult<Response.VideoResponse>.Create(
                videoResponses.ToList(),
                request.PageIndex,
                request.PageSize,
                totalCount
            );

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<PagedResult<Response.VideoResponse>>(Error.Failure("GetVideos.Error", ex.Message));
        }
    }
}