using DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;
using DemoCICD.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoCICD.Persistence.Configurations.MotoGP;

internal sealed class RiderTeamHistoryConfiguration : IEntityTypeConfiguration<RiderTeamHistory>
{
    public void Configure(EntityTypeBuilder<RiderTeamHistory> builder)
    {
        builder.ToTable(TableNames.RiderTeamHistories);

        builder.HasKey(rth => rth.Id);
        
        builder.Property(rth => rth.RiderId)
            .IsRequired();
            
        builder.Property(rth => rth.TeamId)
            .IsRequired();
            
        builder.Property(rth => rth.SeasonId)
            .IsRequired();
            
        builder.Property(rth => rth.StartDate)
            .IsRequired();
            
        builder.Property(rth => rth.EndDate);
            
        builder.Property(rth => rth.Notes)
            .HasMaxLength(1000);

        // Audit fields
        builder.Property(rth => rth.CreatedAt)
            .IsRequired();
        builder.Property(rth => rth.UpdatedAt);
        builder.Property(rth => rth.CreatedBy)
            .HasMaxLength(100);
        builder.Property(rth => rth.UpdatedBy)
            .HasMaxLength(100);
        builder.Property(rth => rth.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
        builder.Property(rth => rth.DeletedAt);
        builder.Property(rth => rth.DeletedBy)
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(rth => rth.RiderId);
        builder.HasIndex(rth => rth.TeamId);
        builder.HasIndex(rth => rth.SeasonId);
        builder.HasIndex(rth => new { rth.RiderId, rth.StartDate });
        builder.HasIndex(rth => rth.EndDate)
            .HasFilter("EndDate IS NULL AND IsDeleted = 0"); // Current relationships
        builder.HasIndex(rth => rth.IsDeleted);
        
        // Relationships
        builder.HasOne<Rider>()
            .WithMany(r => r.TeamHistory)
            .HasForeignKey(rth => rth.RiderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}