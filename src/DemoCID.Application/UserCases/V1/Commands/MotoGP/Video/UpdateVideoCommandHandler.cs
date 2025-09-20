using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Video;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Domain.Entities.MotoGP.MediaNews;
using DemoCICD.Persistence;
using MediatR;

namespace DemoCICD.Application.UserCases.V1.Commands.MotoGP.Video;

public sealed class UpdateVideoCommandHandler : IRequestHandler<Command.UpdateVideoCommand, Result>
{
    private readonly IVideoRepository _videoRepository;
    private readonly ApplicationDbContext _context;

    public UpdateVideoCommandHandler(IVideoRepository videoRepository, ApplicationDbContext context)
    {
        _videoRepository = videoRepository;
        _context = context;
    }

    public async Task<Result> Handle(Command.UpdateVideoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var video = await _videoRepository.FindByIdAsync(request.Id, cancellationToken);
            if (video == null)
            {
                return Result.Failure(Error.NotFound("Video.NotFound", "Video not found"));
            }

            if (!Enum.TryParse<VideoType>(request.Type, out var videoType))
            {
                return Result.Failure(Error.Validation("UpdateVideo.InvalidType", "Invalid video type"));
            }

            video.UpdateContent(request.Title, request.Description, videoType);
            video.UpdateVideo(request.VideoUrl, request.Duration, request.Platform);

            if (!string.IsNullOrEmpty(request.ThumbnailUrl))
            {
                video.UpdateThumbnail(request.ThumbnailUrl);
            }

            if (!string.IsNullOrEmpty(request.ExternalVideoId))
            {
                video.SetExternalVideoId(request.ExternalVideoId);
            }

            if (request.RelatedSeasonId.HasValue || request.RelatedRaceId.HasValue || 
                request.RelatedTeamId.HasValue || request.RelatedRiderId.HasValue)
            {
                video.SetRelatedEntities(request.RelatedSeasonId, request.RelatedRaceId, 
                    request.RelatedTeamId, request.RelatedRiderId);
            }

            _videoRepository.Update(video);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("UpdateVideo.Error", ex.Message));
        }
    }
}