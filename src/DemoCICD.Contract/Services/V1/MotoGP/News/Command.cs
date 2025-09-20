using DemoCICD.Contract.Abstractions.Message;

namespace DemoCICD.Contract.Services.V1.MotoGP.News;

public static class Command
{
    public record CreateNewsCommand(
        string Title,
        string Summary,
        string Content,
        string Category,
        string Slug,
        string? FeaturedImage = null,
        string? ImageCaption = null,
        bool IsFeatured = false,
        bool IsBreaking = false,
        Guid? RelatedSeasonId = null,
        Guid? RelatedRaceId = null,
        Guid? RelatedTeamId = null,
        Guid? RelatedRiderId = null,
        List<string>? Tags = null
    ) : ICommand<Response.NewsResponse>;

    public record UpdateNewsCommand(
        Guid Id,
        string Title,
        string Summary,
        string Content,
        string Category,
        string? FeaturedImage = null,
        string? ImageCaption = null,
        bool IsFeatured = false,
        bool IsBreaking = false,
        Guid? RelatedSeasonId = null,
        Guid? RelatedRaceId = null,
        Guid? RelatedTeamId = null,
        Guid? RelatedRiderId = null,
        List<string>? Tags = null
    ) : ICommand;

    public record PublishNewsCommand(Guid Id) : ICommand;

    public record ArchiveNewsCommand(Guid Id) : ICommand;

    public record DeleteNewsCommand(Guid Id) : ICommand;

    public record UpdateNewsSlugCommand(Guid Id, string Slug) : ICommand;
}