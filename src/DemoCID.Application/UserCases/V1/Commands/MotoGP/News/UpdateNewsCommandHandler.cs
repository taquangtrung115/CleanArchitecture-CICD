using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.News;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Domain.Entities.MotoGP.MediaNews;
using DemoCICD.Persistence;
using MediatR;

namespace DemoCICD.Application.UserCases.V1.Commands.MotoGP.News;

public sealed class UpdateNewsCommandHandler : IRequestHandler<Command.UpdateNewsCommand, Result>
{
    private readonly INewsRepository _newsRepository;
    private readonly ApplicationDbContext _context;

    public UpdateNewsCommandHandler(INewsRepository newsRepository, ApplicationDbContext context)
    {
        _newsRepository = newsRepository;
        _context = context;
    }

    public async Task<Result> Handle(Command.UpdateNewsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var news = await _newsRepository.FindByIdAsync(request.Id, cancellationToken);
            if (news == null)
            {
                return Result.Failure(Error.NotFound("UpdateNews.NotFound", $"News with ID {request.Id} not found"));
            }

            // Parse the category
            if (!Enum.TryParse<NewsCategory>(request.Category, out var category))
            {
                return Result.Failure(Error.Validation("UpdateNews.InvalidCategory", "Invalid news category"));
            }

            // Update content
            news.UpdateContent(request.Title, request.Summary, request.Content, category);

            // Update optional properties
            if (!string.IsNullOrEmpty(request.FeaturedImage) || request.FeaturedImage == null)
            {
                news.UpdateFeaturedImage(request.FeaturedImage, request.ImageCaption);
            }

            // Update featured status
            if (request.IsFeatured && !news.IsFeatured && news.Status == NewsStatus.Published)
            {
                news.SetAsFeatured();
            }
            else if (!request.IsFeatured && news.IsFeatured)
            {
                news.RemoveFromFeatured();
            }

            // Update breaking status
            if (request.IsBreaking && !news.IsBreaking && news.Status == NewsStatus.Published)
            {
                news.SetAsBreaking();
            }
            else if (!request.IsBreaking && news.IsBreaking)
            {
                news.RemoveBreaking();
            }

            // Update related entities
            if (request.RelatedSeasonId.HasValue || request.RelatedRaceId.HasValue || 
                request.RelatedTeamId.HasValue || request.RelatedRiderId.HasValue)
            {
                news.SetRelatedEntities(request.RelatedSeasonId, request.RelatedRaceId, 
                    request.RelatedTeamId, request.RelatedRiderId);
            }

            // Update tags - first remove all existing tags, then add new ones
            var existingTags = news.Tags.ToList();
            foreach (var tag in existingTags)
            {
                news.RemoveTag(tag);
            }

            if (request.Tags != null && request.Tags.Any())
            {
                foreach (var tag in request.Tags)
                {
                    news.AddTag(tag);
                }
            }

            _newsRepository.Update(news);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("UpdateNews.Error", ex.Message));
        }
    }
}