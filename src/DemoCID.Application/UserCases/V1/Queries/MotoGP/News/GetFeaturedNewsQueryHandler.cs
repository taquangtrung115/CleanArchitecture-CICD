using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.News;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using MediatR;

namespace DemoCICD.Application.UserCases.V1.Queries.MotoGP.News;

public sealed class GetFeaturedNewsQueryHandler : IRequestHandler<Query.GetFeaturedNewsQuery, Result<IEnumerable<Response.NewsResponse>>>
{
    private readonly INewsRepository _newsRepository;

    public GetFeaturedNewsQueryHandler(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<Result<IEnumerable<Response.NewsResponse>>> Handle(Query.GetFeaturedNewsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var news = await _newsRepository.GetFeaturedNewsAsync(cancellationToken);
            var limitedNews = news.Take(request.Limit);

            var responses = limitedNews.Select(n => new Response.NewsResponse(
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

            return Result.Success(responses);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<Response.NewsResponse>>(Error.Failure("GetFeaturedNews.Failed", ex.Message));
        }
    }
}