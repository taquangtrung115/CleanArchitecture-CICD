namespace DemoCICD.Contract.Services.V1.MotoGP.News;

public static class Response
{
    public record NewsResponse(
        Guid Id,
        string Title,
        string Summary,
        string Content,
        string Category,
        string Status,
        Guid AuthorId,
        DateTime PublishedDate,
        string? FeaturedImage,
        string? ImageCaption,
        string Slug,
        int ViewCount,
        bool IsFeatured,
        bool IsBreaking,
        Guid? RelatedSeasonId,
        Guid? RelatedRaceId,
        Guid? RelatedTeamId,
        Guid? RelatedRiderId,
        List<string> Tags,
        DateTime CreatedDate,
        DateTime? LastModifiedDate
    );

    public record NewsListResponse(
        IEnumerable<NewsResponse> News,
        int TotalCount,
        int PageIndex,
        int PageSize
    );

    public record NewsCategoryResponse(
        string Category,
        int Count
    );
}