using DemoCICD.Domain.Entities.MotoGP.RaceManagement;
using DemoCICD.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoCICD.Persistence.Configurations.MotoGP;

internal sealed class RaceConfiguration : IEntityTypeConfiguration<Race>
{
    public void Configure(EntityTypeBuilder<Race> builder)
    {
        builder.ToTable(TableNames.Races);

        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.SeasonId)
            .IsRequired();
            
        builder.Property(r => r.Name)
            .HasMaxLength(200)
            .IsRequired();
            
        builder.Property(r => r.CircuitName)
            .HasMaxLength(200)
            .IsRequired();
            
        builder.Property(r => r.RaceDate)
            .IsRequired();
            
        builder.Property(r => r.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
            
        builder.Property(r => r.RoundNumber)
            .IsRequired();
            
        builder.Property(r => r.Description)
            .HasMaxLength(1000);
            
        builder.Property(r => r.WeatherConditions)
            .HasMaxLength(500);
            
        builder.Property(r => r.CircuitLength)
            .HasPrecision(18, 3)
            .IsRequired();
            
        builder.Property(r => r.NumberOfLaps)
            .IsRequired();

        // Country value object
        builder.OwnsOne(r => r.Country, country =>
        {
            country.Property(c => c.Code)
                .HasMaxLength(3)
                .IsRequired()
                .HasColumnName("CountryCode");
            country.Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("CountryName");
            country.Property(c => c.Flag)
                .HasMaxLength(500)
                .HasColumnName("CountryFlag");
        });

        // RaceSchedule value object
        builder.OwnsOne(r => r.Schedule, schedule =>
        {
            schedule.Property(s => s.Practice1)
                .IsRequired()
                .HasColumnName("Practice1DateTime");
            schedule.Property(s => s.Practice2)
                .IsRequired()
                .HasColumnName("Practice2DateTime");
            schedule.Property(s => s.Practice3)
                .HasColumnName("Practice3DateTime");
            schedule.Property(s => s.Qualifying)
                .IsRequired()
                .HasColumnName("QualifyingDateTime");
            schedule.Property(s => s.SprintQualifying)
                .HasColumnName("SprintQualifyingDateTime");
            schedule.Property(s => s.SprintRace)
                .HasColumnName("SprintRaceDateTime");
            schedule.Property(s => s.Race)
                .IsRequired()
                .HasColumnName("RaceDateTime");
        });

        // Audit fields
        builder.Property(r => r.CreatedAt)
            .IsRequired();
        builder.Property(r => r.UpdatedAt);
        builder.Property(r => r.CreatedBy)
            .HasMaxLength(100);
        builder.Property(r => r.UpdatedBy)
            .HasMaxLength(100);
        builder.Property(r => r.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
        builder.Property(r => r.DeletedAt);
        builder.Property(r => r.DeletedBy)
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(r => r.SeasonId);
        builder.HasIndex(r => new { r.SeasonId, r.RoundNumber })
            .IsUnique()
            .HasFilter("IsDeleted = 0");
        builder.HasIndex(r => r.RaceDate);
        builder.HasIndex(r => r.Status);
        builder.HasIndex(r => r.IsDeleted);
        
        // Relationships
        builder.HasOne<Season>()
            .WithMany(s => s.Races)
            .HasForeignKey(r => r.SeasonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}