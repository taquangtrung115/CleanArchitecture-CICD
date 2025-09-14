using DemoCICD.Domain.Entities.MotoGP.RaceManagement;
using DemoCICD.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoCICD.Persistence.Configurations.MotoGP;

internal sealed class RaceEntryConfiguration : IEntityTypeConfiguration<RaceEntry>
{
    public void Configure(EntityTypeBuilder<RaceEntry> builder)
    {
        builder.ToTable(TableNames.RaceEntries);

        builder.HasKey(re => re.Id);
        
        builder.Property(re => re.RaceId)
            .IsRequired();
            
        builder.Property(re => re.RiderId)
            .IsRequired();
            
        builder.Property(re => re.TeamId)
            .IsRequired();
            
        builder.Property(re => re.BikeId)
            .IsRequired();
            
        builder.Property(re => re.StartingGrid)
            .IsRequired();
            
        builder.Property(re => re.FastestLap)
            .IsRequired()
            .HasDefaultValue(false);
            
        builder.Property(re => re.FastestLapTime);
            
        builder.Property(re => re.Notes)
            .HasMaxLength(1000);

        // RaceResult value object
        builder.OwnsOne(re => re.Result, result =>
        {
            result.Property(r => r.Position)
                .HasColumnName("ResultPosition");
            result.Property(r => r.Points)
                .HasColumnName("ResultPoints");
            result.Property(r => r.RaceTime)
                .HasColumnName("ResultRaceTime");
            result.Property(r => r.GapToWinner)
                .HasColumnName("ResultGapToWinner");
            result.Property(r => r.DidNotFinish)
                .HasDefaultValue(false)
                .HasColumnName("ResultDidNotFinish");
            result.Property(r => r.DidNotStart)
                .HasDefaultValue(false)
                .HasColumnName("ResultDidNotStart");
            result.Property(r => r.Disqualified)
                .HasDefaultValue(false)
                .HasColumnName("ResultDisqualified");
            result.Property(r => r.Reason)
                .HasMaxLength(500)
                .HasColumnName("ResultReason");
        });

        // Audit fields
        builder.Property(re => re.CreatedAt)
            .IsRequired();
        builder.Property(re => re.UpdatedAt);
        builder.Property(re => re.CreatedBy)
            .HasMaxLength(100);
        builder.Property(re => re.UpdatedBy)
            .HasMaxLength(100);
        builder.Property(re => re.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
        builder.Property(re => re.DeletedAt);
        builder.Property(re => re.DeletedBy)
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(re => re.RaceId);
        builder.HasIndex(re => re.RiderId);
        builder.HasIndex(re => re.TeamId);
        builder.HasIndex(re => re.BikeId);
        builder.HasIndex(re => new { re.RaceId, re.RiderId })
            .IsUnique()
            .HasFilter("IsDeleted = 0");
        builder.HasIndex(re => new { re.RaceId, re.StartingGrid })
            .IsUnique()
            .HasFilter("IsDeleted = 0");
        builder.HasIndex(re => re.FastestLap)
            .HasFilter("FastestLap = 1 AND IsDeleted = 0");
        builder.HasIndex(re => re.IsDeleted);
        
        // Relationships
        builder.HasOne<Race>()
            .WithMany(r => r.RaceEntries)
            .HasForeignKey(re => re.RaceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}