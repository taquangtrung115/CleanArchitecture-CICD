using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Video;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Domain.Entities.MotoGP.MediaNews;
using DemoCICD.Persistence;
using MediatR;

namespace DemoCICD.Application.UserCases.V1.Commands.MotoGP.Video;

public sealed class CreateVideoCommandHandler : IRequestHandler<Command.CreateVideoCommand, Result>
{
    private readonly IVideoRepository _videoRepository;
    private readonly ApplicationDbContext _context;

    public CreateVideoCommandHandler(IVideoRepository videoRepository, ApplicationDbContext context)
    {
        _videoRepository = videoRepository;
        _context = context;
    }

    public async Task<Result> Handle(Command.CreateVideoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!Enum.TryParse<VideoType>(request.Type, out var videoType))
            {
                return Result.Failure(Error.Validation("CreateVideo.InvalidType", "Invalid video type"));
            }

            var video = Domain.Entities.MotoGP.MediaNews.Video.Create(
                request.Title,
                request.Description,
                videoType,
                request.VideoUrl,
                request.Duration,
                request.Platform
            );

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

            if (request.Tags != null && request.Tags.Any())
            {
                foreach (var tag in request.Tags)
                {
                    video.AddTag(tag);
                }
            }

            _videoRepository.Add(video);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("CreateVideo.Error", ex.Message));
        }
    }
}