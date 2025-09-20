using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Video;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Persistence;
using MediatR;

namespace DemoCICD.Application.UserCases.V1.Commands.MotoGP.Video;

public sealed class IncrementVideoViewCountCommandHandler : IRequestHandler<Command.IncrementVideoViewCountCommand, Result>
{
    private readonly IVideoRepository _videoRepository;
    private readonly ApplicationDbContext _context;

    public IncrementVideoViewCountCommandHandler(IVideoRepository videoRepository, ApplicationDbContext context)
    {
        _videoRepository = videoRepository;
        _context = context;
    }

    public async Task<Result> Handle(Command.IncrementVideoViewCountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var video = await _videoRepository.FindByIdAsync(request.Id, cancellationToken);
            if (video == null)
            {
                return Result.Failure(Error.NotFound("Video.NotFound", "Video not found"));
            }

            video.IncrementViewCount();
            _videoRepository.Update(video);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("IncrementVideoViewCount.Error", ex.Message));
        }
    }
}