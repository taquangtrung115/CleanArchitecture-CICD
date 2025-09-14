using DemoCICD.Domain.Entities.MotoGP.MediaNews;
using DemoCICD.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoCICD.Persistence.Configurations.MotoGP;

internal sealed class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.ToTable(TableNames.Videos);

        builder.HasKey(v => v.Id);
        
        builder.Property(v => v.Title)
            .HasMaxLength(500)
            .IsRequired();
            
        builder.Property(v => v.Description)
            .HasMaxLength(2000)
            .IsRequired();
            
        builder.Property(v => v.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
            
        builder.Property(v => v.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
            
        builder.Property(v => v.VideoUrl)
            .HasMaxLength(1000)
            .IsRequired();
            
        builder.Property(v => v.ThumbnailUrl)
            .HasMaxLength(500);
            
        builder.Property(v => v.Duration)
            .IsRequired();
            
        builder.Property(v => v.PublishedDate)
            .IsRequired();
            
        builder.Property(v => v.ViewCount)
            .IsRequired()
            .HasDefaultValue(0);
            
        builder.Property(v => v.IsFeatured)
            .IsRequired()
            .HasDefaultValue(false);
            
        builder.Property(v => v.ExternalVideoId)
            .HasMaxLength(200);
            
        builder.Property(v => v.Platform)
            .HasMaxLength(100);

        // Related entity IDs
        builder.Property(v => v.RelatedSeasonId);
        builder.Property(v => v.RelatedRaceId);
        builder.Property(v => v.RelatedTeamId);
        builder.Property(v => v.RelatedRiderId);

        // Tags collection - stored as JSON
        builder.Property(v => v.Tags)
            .HasConversion(
                tags => string.Join(';', tags),
                tags => tags.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
            )
            .HasMaxLength(2000);

        // Audit fields
        builder.Property(v => v.CreatedAt)
            .IsRequired();
        builder.Property(v => v.UpdatedAt);
        builder.Property(v => v.CreatedBy)
            .HasMaxLength(100);
        builder.Property(v => v.UpdatedBy)
            .HasMaxLength(100);
        builder.Property(v => v.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
        builder.Property(v => v.DeletedAt);
        builder.Property(v => v.DeletedBy)
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(v => v.Status);
        builder.HasIndex(v => v.Type);
        builder.HasIndex(v => v.Platform);
        builder.HasIndex(v => v.PublishedDate)
            .HasFilter("Status = 'Published' AND IsDeleted = 0");
        builder.HasIndex(v => v.IsFeatured)
            .HasFilter("IsFeatured = 1 AND Status = 'Published' AND IsDeleted = 0");
        builder.HasIndex(v => v.ExternalVideoId)
            .HasFilter("ExternalVideoId IS NOT NULL AND IsDeleted = 0");
        builder.HasIndex(v => v.RelatedSeasonId);
        builder.HasIndex(v => v.RelatedRaceId);
        builder.HasIndex(v => v.RelatedTeamId);
        builder.HasIndex(v => v.RelatedRiderId);
        builder.HasIndex(v => v.IsDeleted);
    }
}