using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;

namespace DemoCICD.Contract.Services.V1.MotoGP.Video;

public static class Command
{
    public record CreateVideoCommand(
        string Title,
        string Description,
        string Type,
        string VideoUrl,
        TimeSpan Duration,
        string? Platform = null,
        string? ThumbnailUrl = null,
        string? ExternalVideoId = null,
        Guid? RelatedSeasonId = null,
        Guid? RelatedRaceId = null,
        Guid? RelatedTeamId = null,
        Guid? RelatedRiderId = null,
        List<string>? Tags = null
    ) : ICommand<Result>;

    public record UpdateVideoCommand(
        Guid Id,
        string Title,
        string Description,
        string Type,
        string VideoUrl,
        TimeSpan Duration,
        string? Platform = null,
        string? ThumbnailUrl = null,
        string? ExternalVideoId = null,
        Guid? RelatedSeasonId = null,
        Guid? RelatedRaceId = null,
        Guid? RelatedTeamId = null,
        Guid? RelatedRiderId = null,
        List<string>? Tags = null
    ) : ICommand<Result>;

    public record DeleteVideoCommand(Guid Id) : ICommand<Result>;

    public record PublishVideoCommand(Guid Id) : ICommand<Result>;

    public record ArchiveVideoCommand(Guid Id) : ICommand<Result>;

    public record SetVideoAsFeaturedCommand(Guid Id) : ICommand<Result>;

    public record RemoveVideoFromFeaturedCommand(Guid Id) : ICommand<Result>;

    public record IncrementVideoViewCountCommand(Guid Id) : ICommand<Result>;
}