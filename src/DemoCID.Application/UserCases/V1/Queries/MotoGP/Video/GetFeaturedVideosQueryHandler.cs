using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Video;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using MediatR;

namespace DemoCICD.Application.UserCases.V1.Queries.MotoGP.Video;

public sealed class GetFeaturedVideosQueryHandler : IRequestHandler<Query.GetFeaturedVideosQuery, Result<IEnumerable<Response.VideoResponse>>>
{
    private readonly IVideoRepository _videoRepository;

    public GetFeaturedVideosQueryHandler(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository;
    }

    public async Task<Result<IEnumerable<Response.VideoResponse>>> Handle(Query.GetFeaturedVideosQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var videos = await _videoRepository.GetFeaturedVideosAsync(cancellationToken);

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

            return Result.Success(videoResponses);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<Response.VideoResponse>>(Error.Failure("GetFeaturedVideos.Error", ex.Message));
        }
    }
}