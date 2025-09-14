using DemoCICD.Domain.Entities.MotoGP.MediaNews;
using DemoCICD.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoCICD.Persistence.Configurations.MotoGP;

internal sealed class NewsConfiguration : IEntityTypeConfiguration<News>
{
    public void Configure(EntityTypeBuilder<News> builder)
    {
        builder.ToTable(TableNames.News);

        builder.HasKey(n => n.Id);
        
        builder.Property(n => n.Title)
            .HasMaxLength(500)
            .IsRequired();
            
        builder.Property(n => n.Summary)
            .HasMaxLength(1000)
            .IsRequired();
            
        builder.Property(n => n.Content)
            .IsRequired();
            
        builder.Property(n => n.Category)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
            
        builder.Property(n => n.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
            
        builder.Property(n => n.AuthorId)
            .IsRequired();
            
        builder.Property(n => n.PublishedDate)
            .IsRequired();
            
        builder.Property(n => n.FeaturedImage)
            .HasMaxLength(500);
            
        builder.Property(n => n.ImageCaption)
            .HasMaxLength(500);
            
        builder.Property(n => n.Slug)
            .HasMaxLength(300)
            .IsRequired();
            
        builder.Property(n => n.ViewCount)
            .IsRequired()
            .HasDefaultValue(0);
            
        builder.Property(n => n.IsFeatured)
            .IsRequired()
            .HasDefaultValue(false);
            
        builder.Property(n => n.IsBreaking)
            .IsRequired()
            .HasDefaultValue(false);

        // Related entity IDs
        builder.Property(n => n.RelatedSeasonId);
        builder.Property(n => n.RelatedRaceId);
        builder.Property(n => n.RelatedTeamId);
        builder.Property(n => n.RelatedRiderId);

        // Tags collection - stored as JSON
        builder.Property(n => n.Tags)
            .HasConversion(
                tags => string.Join(';', tags),
                tags => tags.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
            )
            .HasMaxLength(2000);

        // Audit fields
        builder.Property(n => n.CreatedAt)
            .IsRequired();
        builder.Property(n => n.UpdatedAt);
        builder.Property(n => n.CreatedBy)
            .HasMaxLength(100);
        builder.Property(n => n.UpdatedBy)
            .HasMaxLength(100);
        builder.Property(n => n.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
        builder.Property(n => n.DeletedAt);
        builder.Property(n => n.DeletedBy)
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(n => n.Slug)
            .IsUnique()
            .HasFilter("IsDeleted = 0");
        builder.HasIndex(n => n.Status);
        builder.HasIndex(n => n.Category);
        builder.HasIndex(n => n.AuthorId);
        builder.HasIndex(n => n.PublishedDate)
            .HasFilter("Status = 'Published' AND IsDeleted = 0");
        builder.HasIndex(n => n.IsFeatured)
            .HasFilter("IsFeatured = 1 AND Status = 'Published' AND IsDeleted = 0");
        builder.HasIndex(n => n.IsBreaking)
            .HasFilter("IsBreaking = 1 AND Status = 'Published' AND IsDeleted = 0");
        builder.HasIndex(n => n.RelatedSeasonId);
        builder.HasIndex(n => n.RelatedRaceId);
        builder.HasIndex(n => n.RelatedTeamId);
        builder.HasIndex(n => n.RelatedRiderId);
        builder.HasIndex(n => n.IsDeleted);
    }
}