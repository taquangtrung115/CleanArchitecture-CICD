using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.News;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Domain.Entities.MotoGP.MediaNews;
using DemoCICD.Persistence;
using MediatR;

namespace DemoCICD.Application.UserCases.V1.Commands.MotoGP.News;

public sealed class CreateNewsCommandHandler : IRequestHandler<Command.CreateNewsCommand, Result<Response.NewsResponse>>
{
    private readonly INewsRepository _newsRepository;
    private readonly ApplicationDbContext _context;

    public CreateNewsCommandHandler(INewsRepository newsRepository, ApplicationDbContext context)
    {
        _newsRepository = newsRepository;
        _context = context;
    }

    public async Task<Result<Response.NewsResponse>> Handle(Command.CreateNewsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Parse the category
            if (!Enum.TryParse<NewsCategory>(request.Category, out var category))
            {
                return Result.Failure<Response.NewsResponse>(Error.Validation("CreateNews.InvalidCategory", "Invalid news category"));
            }

            // Check if slug already exists
            var slugExists = await _newsRepository.SlugExistsAsync(request.Slug, cancellationToken: cancellationToken);
            if (slugExists)
            {
                return Result.Failure<Response.NewsResponse>(Error.Validation("CreateNews.SlugExists", "News with this slug already exists"));
            }

            // Create the news entity
            var news = Domain.Entities.MotoGP.MediaNews.News.Create(
                request.Title,
                request.Summary,
                request.Content,
                category,
                Guid.NewGuid(), // This should be the current user ID in a real implementation
                request.Slug
            );

            // Set optional properties
            if (!string.IsNullOrEmpty(request.FeaturedImage))
            {
                news.UpdateFeaturedImage(request.FeaturedImage, request.ImageCaption);
            }

            if (request.RelatedSeasonId.HasValue || request.RelatedRaceId.HasValue || 
                request.RelatedTeamId.HasValue || request.RelatedRiderId.HasValue)
            {
                news.SetRelatedEntities(request.RelatedSeasonId, request.RelatedRaceId, 
                    request.RelatedTeamId, request.RelatedRiderId);
            }

            if (request.Tags != null && request.Tags.Any())
            {
                foreach (var tag in request.Tags)
            {
                    news.AddTag(tag);
                }
            }

            _newsRepository.Add(news);
            await _context.SaveChangesAsync(cancellationToken);

            // Create response
            var response = new Response.NewsResponse(
                news.Id,
                news.Title,
                news.Summary,
                news.Content,
                news.Category.ToString(),
                news.Status.ToString(),
                news.AuthorId,
                news.PublishedDate,
                news.FeaturedImage,
                news.ImageCaption,
                news.Slug,
                news.ViewCount,
                news.IsFeatured,
                news.IsBreaking,
                news.RelatedSeasonId,
                news.RelatedRaceId,
                news.RelatedTeamId,
                news.RelatedRiderId,
                news.Tags.ToList(),
                news.CreatedAt,
                news.UpdatedAt
            );

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<Response.NewsResponse>(Error.Failure("CreateNews.Error", ex.Message));
        }
    }
}