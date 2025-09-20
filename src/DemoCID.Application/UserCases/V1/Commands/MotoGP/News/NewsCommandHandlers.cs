using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.News;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Persistence;
using MediatR;

namespace DemoCICD.Application.UserCases.V1.Commands.MotoGP.News;

public sealed class PublishNewsCommandHandler : IRequestHandler<Command.PublishNewsCommand, Result>
{
    private readonly INewsRepository _newsRepository;
    private readonly ApplicationDbContext _context;

    public PublishNewsCommandHandler(INewsRepository newsRepository, ApplicationDbContext context)
    {
        _newsRepository = newsRepository;
        _context = context;
    }

    public async Task<Result> Handle(Command.PublishNewsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var news = await _newsRepository.FindByIdAsync(request.Id, cancellationToken);
            if (news == null)
            {
                return Result.Failure(Error.NotFound("PublishNews.NotFound", $"News with ID {request.Id} not found"));
            }

            news.Publish();

            _newsRepository.Update(news);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(Error.Validation("PublishNews.InvalidOperation", ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("PublishNews.Error", ex.Message));
        }
    }
}

public sealed class ArchiveNewsCommandHandler : IRequestHandler<Command.ArchiveNewsCommand, Result>
{
    private readonly INewsRepository _newsRepository;
    private readonly ApplicationDbContext _context;

    public ArchiveNewsCommandHandler(INewsRepository newsRepository, ApplicationDbContext context)
    {
        _newsRepository = newsRepository;
        _context = context;
    }

    public async Task<Result> Handle(Command.ArchiveNewsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var news = await _newsRepository.FindByIdAsync(request.Id, cancellationToken);
            if (news == null)
            {
                return Result.Failure(Error.NotFound("ArchiveNews.NotFound", $"News with ID {request.Id} not found"));
            }

            news.Archive();

            _newsRepository.Update(news);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(Error.Validation("ArchiveNews.InvalidOperation", ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("ArchiveNews.Error", ex.Message));
        }
    }
}

public sealed class DeleteNewsCommandHandler : IRequestHandler<Command.DeleteNewsCommand, Result>
{
    private readonly INewsRepository _newsRepository;
    private readonly ApplicationDbContext _context;

    public DeleteNewsCommandHandler(INewsRepository newsRepository, ApplicationDbContext context)
    {
        _newsRepository = newsRepository;
        _context = context;
    }

    public async Task<Result> Handle(Command.DeleteNewsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var news = await _newsRepository.FindByIdAsync(request.Id, cancellationToken);
            if (news == null)
            {
                return Result.Failure(Error.NotFound("DeleteNews.NotFound", $"News with ID {request.Id} not found"));
            }

            _newsRepository.Remove(news);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("DeleteNews.Error", ex.Message));
        }
    }
}

public sealed class UpdateNewsSlugCommandHandler : IRequestHandler<Command.UpdateNewsSlugCommand, Result>
{
    private readonly INewsRepository _newsRepository;
    private readonly ApplicationDbContext _context;

    public UpdateNewsSlugCommandHandler(INewsRepository newsRepository, ApplicationDbContext context)
    {
        _newsRepository = newsRepository;
        _context = context;
    }

    public async Task<Result> Handle(Command.UpdateNewsSlugCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var news = await _newsRepository.FindByIdAsync(request.Id, cancellationToken);
            if (news == null)
            {
                return Result.Failure(Error.NotFound("UpdateNewsSlug.NotFound", $"News with ID {request.Id} not found"));
            }

            // Check if slug already exists (excluding current news)
            var slugExists = await _newsRepository.SlugExistsAsync(request.Slug, request.Id, cancellationToken);
            if (slugExists)
            {
                return Result.Failure(Error.Validation("UpdateNewsSlug.SlugExists", "News with this slug already exists"));
            }

            news.UpdateSlug(request.Slug);

            _newsRepository.Update(news);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("UpdateNewsSlug.Error", ex.Message));
        }
    }
}