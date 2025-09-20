using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;

namespace DemoCICD.Contract.Services.V1.MotoGP.News;

public static class Query
{
    public record GetNewsQuery(
        int PageIndex = 1,
        int PageSize = 10,
        string? Category = null,
        string? SearchTerm = null,
        bool? IsFeatured = null,
        bool? IsBreaking = null
    ) : IQuery<PagedResult<Response.NewsResponse>>;

    public record GetNewsByIdQuery(Guid Id) : IQuery<Response.NewsResponse>;

    public record GetNewsBySlugQuery(string Slug) : IQuery<Response.NewsResponse>;

    public record GetNewsByCategoryQuery(
        string Category,
        int PageIndex = 1,
        int PageSize = 10
    ) : IQuery<PagedResult<Response.NewsResponse>>;

    public record GetFeaturedNewsQuery(int Limit = 5) : IQuery<IEnumerable<Response.NewsResponse>>;

    public record GetBreakingNewsQuery() : IQuery<IEnumerable<Response.NewsResponse>>;

    public record GetRelatedNewsQuery(
        Guid? SeasonId = null,
        Guid? RaceId = null,
        Guid? TeamId = null,
        Guid? RiderId = null,
        int Limit = 5
    ) : IQuery<IEnumerable<Response.NewsResponse>>;
}