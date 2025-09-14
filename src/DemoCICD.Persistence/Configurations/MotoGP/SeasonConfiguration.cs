using DemoCICD.Domain.Entities.MotoGP.RaceManagement;
using DemoCICD.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoCICD.Persistence.Configurations.MotoGP;

internal sealed class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.ToTable(TableNames.Seasons);

        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.Year)
            .IsRequired();
            
        builder.Property(s => s.Name)
            .HasMaxLength(200)
            .IsRequired();
            
        builder.Property(s => s.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
            
        builder.Property(s => s.StartDate)
            .IsRequired();
            
        builder.Property(s => s.EndDate)
            .IsRequired();
            
        builder.Property(s => s.Description)
            .HasMaxLength(1000);
            
        builder.Property(s => s.IsCurrentSeason)
            .IsRequired();

        // Audit fields
        builder.Property(s => s.CreatedAt)
            .IsRequired();
        builder.Property(s => s.UpdatedAt);
        builder.Property(s => s.CreatedBy)
            .HasMaxLength(100);
        builder.Property(s => s.UpdatedBy)
            .HasMaxLength(100);
        builder.Property(s => s.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
        builder.Property(s => s.DeletedAt);
        builder.Property(s => s.DeletedBy)
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(s => s.Year)
            .IsUnique()
            .HasFilter("IsDeleted = 0");
        builder.HasIndex(s => s.IsCurrentSeason)
            .HasFilter("IsCurrentSeason = 1 AND IsDeleted = 0");
        builder.HasIndex(s => s.IsDeleted);
        
        // Relationships - Races will be configured in RaceConfiguration
    }
}