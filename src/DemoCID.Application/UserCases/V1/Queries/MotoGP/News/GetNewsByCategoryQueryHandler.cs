using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.News;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Domain.Entities.MotoGP.MediaNews;
using MediatR;

namespace DemoCICD.Application.UserCases.V1.Queries.MotoGP.News;

public sealed class GetNewsByCategoryQueryHandler : IRequestHandler<Query.GetNewsByCategoryQuery, Result<PagedResult<Response.NewsResponse>>>
{
    private readonly INewsRepository _newsRepository;

    public GetNewsByCategoryQueryHandler(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<Result<PagedResult<Response.NewsResponse>>> Handle(Query.GetNewsByCategoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!Enum.TryParse<NewsCategory>(request.Category, out var category))
            {
                return Result.Failure<PagedResult<Response.NewsResponse>>(Error.Validation("Category.Invalid", $"Invalid category: {request.Category}"));
            }

            var news = await _newsRepository.GetNewsByCategoryAsync(category, request.PageIndex, request.PageSize, cancellationToken);

            var newsResponses = news.Select(n => new Response.NewsResponse(
                n.Id,
                n.Title,
                n.Summary,
                n.Content,
                n.Category.ToString(),
                n.Status.ToString(),
                n.AuthorId,
                n.PublishedDate,
                n.FeaturedImage,
                n.ImageCaption,
                n.Slug,
                n.ViewCount,
                n.IsFeatured,
                n.IsBreaking,
                n.RelatedSeasonId,
                n.RelatedRaceId,
                n.RelatedTeamId,
                n.RelatedRiderId,
                n.Tags.ToList(),
                n.CreatedAt,
                n.UpdatedAt
            ));

            // Get total count for this category
            var allCategoryNews = await _newsRepository.GetNewsByCategoryAsync(category, 1, int.MaxValue, cancellationToken);
            var totalCount = allCategoryNews.Count();

            var pagedResult = PagedResult<Response.NewsResponse>.Create(
                newsResponses.ToList(),
                request.PageIndex,
                request.PageSize,
                totalCount
            );

            return Result.Success(pagedResult);
        }
        catch (Exception ex)
        {
            return Result.Failure<PagedResult<Response.NewsResponse>>(Error.Failure("GetNewsByCategory.Failed", ex.Message));
        }
    }
}