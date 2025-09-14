using DemoCICD.Domain.Abstractions.Entities;

namespace DemoCICD.Domain.Entities.MotoGP.MediaNews;

public enum VideoType
{
    Highlight,
    Interview,
    Analysis,
    OnBoard,
    PressConference,
    Documentary,
    LiveStream
}

public enum VideoStatus
{
    Draft,
    Processing,
    Published,
    Archived,
    Deleted
}

public class Video : AuditableEntity<Guid>
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public VideoType Type { get; private set; }
    public VideoStatus Status { get; private set; }
    public string VideoUrl { get; private set; }
    public string? ThumbnailUrl { get; private set; }
    public TimeSpan Duration { get; private set; }
    public DateTime PublishedDate { get; private set; }
    public int ViewCount { get; private set; }
    public bool IsFeatured { get; private set; }
    public string? ExternalVideoId { get; private set; } // YouTube, Vimeo, etc.
    public string? Platform { get; private set; } // YouTube, Vimeo, Internal, etc.

    // Relations to other entities
    public Guid? RelatedSeasonId { get; private set; }
    public Guid? RelatedRaceId { get; private set; }
    public Guid? RelatedTeamId { get; private set; }
    public Guid? RelatedRiderId { get; private set; }

    private readonly List<string> _tags = new();
    public IReadOnlyList<string> Tags => _tags.AsReadOnly();

    public Video(Guid id, string title, string description, VideoType type, string videoUrl, 
                TimeSpan duration, string? platform = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        if (string.IsNullOrWhiteSpace(videoUrl))
            throw new ArgumentException("Video URL cannot be empty", nameof(videoUrl));
        if (duration <= TimeSpan.Zero)
            throw new ArgumentException("Duration must be positive", nameof(duration));

        Id = id;
        Title = title;
        Description = description;
        Type = type;
        VideoUrl = videoUrl;
        Duration = duration;
        Platform = platform;
        Status = VideoStatus.Draft;
        PublishedDate = DateTime.UtcNow;
        ViewCount = 0;
        IsFeatured = false;
    }

    public static Video Create(string title, string description, VideoType type, string videoUrl, 
                              TimeSpan duration, string? platform = null)
    {
        return new Video(Guid.NewGuid(), title, description, type, videoUrl, duration, platform);
    }

    public void UpdateContent(string title, string description, VideoType type)
    {
        if (Status == VideoStatus.Archived)
            throw new InvalidOperationException("Cannot update archived video");

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));

        Title = title;
        Description = description;
        Type = type;
        SetUpdatedAudit();
    }

    public void UpdateVideo(string videoUrl, TimeSpan duration, string? platform = null)
    {
        if (string.IsNullOrWhiteSpace(videoUrl))
            throw new ArgumentException("Video URL cannot be empty", nameof(videoUrl));
        if (duration <= TimeSpan.Zero)
            throw new ArgumentException("Duration must be positive", nameof(duration));

        VideoUrl = videoUrl;
        Duration = duration;
        Platform = platform;
        SetUpdatedAudit();
    }

    public void UpdateThumbnail(string? thumbnailUrl)
    {
        ThumbnailUrl = thumbnailUrl;
        SetUpdatedAudit();
    }

    public void SetExternalVideoId(string? externalVideoId)
    {
        ExternalVideoId = externalVideoId;
        SetUpdatedAudit();
    }

    public void StartProcessing()
    {
        if (Status != VideoStatus.Draft)
            throw new InvalidOperationException($"Cannot start processing video in {Status} status");

        Status = VideoStatus.Processing;
        SetUpdatedAudit();
    }

    public void Publish()
    {
        if (Status != VideoStatus.Processing)
            throw new InvalidOperationException($"Cannot publish video in {Status} status");

        Status = VideoStatus.Published;
        PublishedDate = DateTime.UtcNow;
        SetUpdatedAudit();
    }

    public void Archive()
    {
        if (Status == VideoStatus.Draft)
            throw new InvalidOperationException("Cannot archive draft video");

        Status = VideoStatus.Archived;
        SetUpdatedAudit();
    }

    public void SetAsFeatured()
    {
        if (Status != VideoStatus.Published)
            throw new InvalidOperationException("Only published videos can be featured");

        IsFeatured = true;
        SetUpdatedAudit();
    }

    public void RemoveFromFeatured()
    {
        IsFeatured = false;
        SetUpdatedAudit();
    }

    public void IncrementViewCount()
    {
        ViewCount++;
        SetUpdatedAudit();
    }

    public void AddTag(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
            throw new ArgumentException("Tag cannot be empty", nameof(tag));

        var normalizedTag = tag.Trim().ToLowerInvariant();
        if (!_tags.Contains(normalizedTag))
        {
            _tags.Add(normalizedTag);
            SetUpdatedAudit();
        }
    }

    public void RemoveTag(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
            return;

        var normalizedTag = tag.Trim().ToLowerInvariant();
        if (_tags.Remove(normalizedTag))
        {
            SetUpdatedAudit();
        }
    }

    public void SetRelatedEntities(Guid? seasonId = null, Guid? raceId = null, Guid? teamId = null, Guid? riderId = null)
    {
        RelatedSeasonId = seasonId;
        RelatedRaceId = raceId;
        RelatedTeamId = teamId;
        RelatedRiderId = riderId;
        SetUpdatedAudit();
    }

    public bool IsPublished => Status == VideoStatus.Published;
    public bool HasThumbnail => !string.IsNullOrEmpty(ThumbnailUrl);
    public bool IsExternal => !string.IsNullOrEmpty(ExternalVideoId);
    public string FormattedDuration => Duration.ToString(@"mm\:ss");
    public int DaysSincePublished => (DateTime.UtcNow - PublishedDate).Days;
}