using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Video;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Persistence;
using MediatR;

namespace DemoCICD.Application.UserCases.V1.Commands.MotoGP.Video;

public sealed class DeleteVideoCommandHandler : IRequestHandler<Command.DeleteVideoCommand, Result>
{
    private readonly IVideoRepository _videoRepository;
    private readonly ApplicationDbContext _context;

    public DeleteVideoCommandHandler(IVideoRepository videoRepository, ApplicationDbContext context)
    {
        _videoRepository = videoRepository;
        _context = context;
    }

    public async Task<Result> Handle(Command.DeleteVideoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var video = await _videoRepository.FindByIdAsync(request.Id, cancellationToken);
            if (video == null)
            {
                return Result.Failure(Error.NotFound("Video.NotFound", "Video not found"));
            }

            _videoRepository.Remove(video);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("DeleteVideo.Error", ex.Message));
        }
    }
}

public sealed class PublishVideoCommandHandler : IRequestHandler<Command.PublishVideoCommand, Result>
{
    private readonly IVideoRepository _videoRepository;
    private readonly ApplicationDbContext _context;

    public PublishVideoCommandHandler(IVideoRepository videoRepository, ApplicationDbContext context)
    {
        _videoRepository = videoRepository;
        _context = context;
    }

    public async Task<Result> Handle(Command.PublishVideoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var video = await _videoRepository.FindByIdAsync(request.Id, cancellationToken);
            if (video == null)
            {
                return Result.Failure(Error.NotFound("Video.NotFound", "Video not found"));
            }

            video.Publish();
            _videoRepository.Update(video);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("PublishVideo.Error", ex.Message));
        }
    }
}

public sealed class SetVideoAsFeaturedCommandHandler : IRequestHandler<Command.SetVideoAsFeaturedCommand, Result>
{
    private readonly IVideoRepository _videoRepository;
    private readonly ApplicationDbContext _context;

    public SetVideoAsFeaturedCommandHandler(IVideoRepository videoRepository, ApplicationDbContext context)
    {
        _videoRepository = videoRepository;
        _context = context;
    }

    public async Task<Result> Handle(Command.SetVideoAsFeaturedCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var video = await _videoRepository.FindByIdAsync(request.Id, cancellationToken);
            if (video == null)
            {
                return Result.Failure(Error.NotFound("Video.NotFound", "Video not found"));
            }

            video.SetAsFeatured();
            _videoRepository.Update(video);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("SetVideoAsFeatured.Error", ex.Message));
        }
    }
}