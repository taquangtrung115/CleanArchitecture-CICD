using DemoCICD.Domain.Abstractions.Entities;

namespace DemoCICD.Domain.Entities.MotoGP.MediaNews;

public enum NewsCategory
{
    General,
    RaceResults,
    Transfers,
    Technical,
    Interviews,
    Championship,
    Breaking
}

public enum NewsStatus
{
    Draft,
    Published,
    Archived,
    Deleted
}

public class News : AuditableEntity<Guid>
{
    public string Title { get; private set; }
    public string Summary { get; private set; }
    public string Content { get; private set; }
    public NewsCategory Category { get; private set; }
    public NewsStatus Status { get; private set; }
    public Guid AuthorId { get; private set; }
    public DateTime PublishedDate { get; private set; }
    public string? FeaturedImage { get; private set; }
    public string? ImageCaption { get; private set; }
    public string Slug { get; private set; }
    public int ViewCount { get; private set; }
    public bool IsFeatured { get; private set; }
    public bool IsBreaking { get; private set; }

    // Relations to other entities
    public Guid? RelatedSeasonId { get; private set; }
    public Guid? RelatedRaceId { get; private set; }
    public Guid? RelatedTeamId { get; private set; }
    public Guid? RelatedRiderId { get; private set; }

    private readonly List<string> _tags = new();
    public IReadOnlyList<string> Tags => _tags.AsReadOnly();

    public News(Guid id, string title, string summary, string content, NewsCategory category, 
               Guid authorId, string slug)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));
        if (string.IsNullOrWhiteSpace(summary))
            throw new ArgumentException("Summary cannot be empty", nameof(summary));
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content cannot be empty", nameof(content));
        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Slug cannot be empty", nameof(slug));

        Id = id;
        Title = title;
        Summary = summary;
        Content = content;
        Category = category;
        AuthorId = authorId;
        Slug = slug;
        Status = NewsStatus.Draft;
        PublishedDate = DateTime.UtcNow;
        ViewCount = 0;
        IsFeatured = false;
        IsBreaking = false;
    }

    public static News Create(string title, string summary, string content, NewsCategory category, 
                             Guid authorId, string slug)
    {
        return new News(Guid.NewGuid(), title, summary, content, category, authorId, slug);
    }

    public void UpdateContent(string title, string summary, string content, NewsCategory category)
    {
        if (Status == NewsStatus.Archived)
            throw new InvalidOperationException("Cannot update archived news");

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));
        if (string.IsNullOrWhiteSpace(summary))
            throw new ArgumentException("Summary cannot be empty", nameof(summary));
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content cannot be empty", nameof(content));

        Title = title;
        Summary = summary;
        Content = content;
        Category = category;
        SetUpdatedAudit();
    }

    public void UpdateSlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Slug cannot be empty", nameof(slug));

        Slug = slug;
        SetUpdatedAudit();
    }

    public void UpdateFeaturedImage(string? featuredImage, string? imageCaption = null)
    {
        FeaturedImage = featuredImage;
        ImageCaption = imageCaption;
        SetUpdatedAudit();
    }

    public void Publish()
    {
        if (Status != NewsStatus.Draft)
            throw new InvalidOperationException($"Cannot publish news in {Status} status");

        Status = NewsStatus.Published;
        PublishedDate = DateTime.UtcNow;
        SetUpdatedAudit();
    }

    public void Archive()
    {
        if (Status == NewsStatus.Draft)
            throw new InvalidOperationException("Cannot archive draft news");

        Status = NewsStatus.Archived;
        SetUpdatedAudit();
    }

    public void SetAsFeatured()
    {
        if (Status != NewsStatus.Published)
            throw new InvalidOperationException("Only published news can be featured");

        IsFeatured = true;
        SetUpdatedAudit();
    }

    public void RemoveFromFeatured()
    {
        IsFeatured = false;
        SetUpdatedAudit();
    }

    public void SetAsBreaking()
    {
        if (Status != NewsStatus.Published)
            throw new InvalidOperationException("Only published news can be marked as breaking");

        IsBreaking = true;
        Category = NewsCategory.Breaking;
        SetUpdatedAudit();
    }

    public void RemoveBreaking()
    {
        IsBreaking = false;
        if (Category == NewsCategory.Breaking)
            Category = NewsCategory.General;
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

    public bool IsPublished => Status == NewsStatus.Published;
    public bool HasFeaturedImage => !string.IsNullOrEmpty(FeaturedImage);
    public int DaysSincePublished => (DateTime.UtcNow - PublishedDate).Days;
}