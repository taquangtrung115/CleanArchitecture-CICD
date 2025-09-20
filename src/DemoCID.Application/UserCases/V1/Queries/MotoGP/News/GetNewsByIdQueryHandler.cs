using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.News;
using DemoCICD.Domain.Abstractions.Reponsitories.MotoGP;
using MediatR;

namespace DemoCICD.Application.UserCases.V1.Queries.MotoGP.News;

public sealed class GetNewsByIdQueryHandler : IRequestHandler<Query.GetNewsByIdQuery, Result<Response.NewsResponse>>
{
    private readonly INewsRepository _newsRepository;

    public GetNewsByIdQueryHandler(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<Result<Response.NewsResponse>> Handle(Query.GetNewsByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var news = await _newsRepository.FindByIdAsync(request.Id, cancellationToken);

            if (news == null)
            {
                return Result.Failure<Response.NewsResponse>(Error.NotFound("News.NotFound", $"News with ID {request.Id} was not found"));
            }

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
            return Result.Failure<Response.NewsResponse>(Error.Failure("GetNewsById.Failed", ex.Message));
        }
    }
}