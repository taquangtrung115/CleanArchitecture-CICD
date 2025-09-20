namespace DemoCICD.Contract.Services.V1.MotoGP.Video;

public static class Response
{
    public record VideoResponse(
        Guid Id,
        string Title,
        string Description,
        string Type,
        string Status,
        string VideoUrl,
        string? ThumbnailUrl,
        string FormattedDuration,
        DateTime PublishedDate,
        int ViewCount,
        bool IsFeatured,
        string? Platform,
        string? ExternalVideoId,
        Guid? RelatedSeasonId,
        Guid? RelatedRaceId,
        Guid? RelatedTeamId,
        Guid? RelatedRiderId,
        List<string> Tags,
        DateTime CreatedDate,
        DateTime? LastModifiedDate
    );

    public record VideoListResponse(
        IEnumerable<VideoResponse> Videos,
        int TotalCount,
        int PageIndex,
        int PageSize
    );

    public record VideoTypeResponse(
        string Type,
        int Count
    );

    public record VideoPlatformResponse(
        string Platform,
        int Count
    );
}