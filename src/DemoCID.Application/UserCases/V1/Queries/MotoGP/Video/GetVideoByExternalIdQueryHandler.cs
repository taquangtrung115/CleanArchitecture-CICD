using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Video;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using MediatR;

namespace DemoCICD.Application.UserCases.V1.Queries.MotoGP.Video;

public sealed class GetVideoByExternalIdQueryHandler : IRequestHandler<Query.GetVideoByExternalIdQuery, Result<Response.VideoResponse>>
{
    private readonly IVideoRepository _videoRepository;

    public GetVideoByExternalIdQueryHandler(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository;
    }

    public async Task<Result<Response.VideoResponse>> Handle(Query.GetVideoByExternalIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var video = await _videoRepository.GetVideoByExternalIdAsync(request.ExternalVideoId, cancellationToken);
            
            if (video == null)
            {
                return Result.Failure<Response.VideoResponse>(Error.NotFound("Video.NotFound", "Video not found"));
            }

            var videoResponse = new Response.VideoResponse(
                video.Id,
                video.Title,
                video.Description,
                video.Type.ToString(),
                video.Status.ToString(),
                video.VideoUrl,
                video.ThumbnailUrl,
                video.FormattedDuration,
                video.PublishedDate,
                video.ViewCount,
                video.IsFeatured,
                video.Platform,
                video.ExternalVideoId,
                video.RelatedSeasonId,
                video.RelatedRaceId,
                video.RelatedTeamId,
                video.RelatedRiderId,
                video.Tags.ToList(),
                video.CreatedAt,
                video.UpdatedAt
            );

            return Result.Success(videoResponse);
        }
        catch (Exception ex)
        {
            return Result.Failure<Response.VideoResponse>(Error.Failure("GetVideoByExternalId.Error", ex.Message));
        }
    }
}