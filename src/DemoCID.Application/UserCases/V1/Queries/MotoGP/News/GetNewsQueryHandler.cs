using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.News;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using DemoCICD.Domain.Entities.MotoGP.MediaNews;
using MediatR;

namespace DemoCICD.Application.UserCases.V1.Queries.MotoGP.News;

public sealed class GetNewsQueryHandler : IRequestHandler<Query.GetNewsQuery, Result<PagedResult<Response.NewsResponse>>>
{
    private readonly INewsRepository _newsRepository;

    public GetNewsQueryHandler(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<Result<PagedResult<Response.NewsResponse>>> Handle(Query.GetNewsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<Domain.Entities.MotoGP.MediaNews.News> news;

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                news = await _newsRepository.SearchNewsAsync(request.SearchTerm, request.PageIndex, request.PageSize, cancellationToken);
            }
            else if (!string.IsNullOrEmpty(request.Category) && Enum.TryParse<NewsCategory>(request.Category, out var category))
            {
                news = await _newsRepository.GetNewsByCategoryAsync(category, request.PageIndex, request.PageSize, cancellationToken);
            }
            else if (request.IsFeatured == true)
            {
                var featuredNews = await _newsRepository.GetFeaturedNewsAsync(cancellationToken);
                news = featuredNews.Take(request.PageSize);
            }
            else if (request.IsBreaking == true)
            {
                news = await _newsRepository.GetBreakingNewsAsync(cancellationToken);
            }
            else
            {
                news = await _newsRepository.GetPublishedNewsAsync(request.PageIndex, request.PageSize, cancellationToken);
            }

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

            // For pagination, we need to get total count
            var allPublishedNews = await _newsRepository.GetPublishedNewsAsync(1, int.MaxValue, cancellationToken);
            var totalCount = allPublishedNews.Count();

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
            return Result.Failure<PagedResult<Response.NewsResponse>>(Error.Failure("GetNews.Failed", ex.Message));
        }
    }
}