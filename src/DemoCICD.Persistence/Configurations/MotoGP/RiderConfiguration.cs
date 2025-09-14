using DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;
using DemoCICD.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoCICD.Persistence.Configurations.MotoGP;

internal sealed class RiderConfiguration : IEntityTypeConfiguration<Rider>
{
    public void Configure(EntityTypeBuilder<Rider> builder)
    {
        builder.ToTable(TableNames.Riders);

        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.FirstName)
            .HasMaxLength(100)
            .IsRequired();
            
        builder.Property(r => r.LastName)
            .HasMaxLength(100)
            .IsRequired();
            
        builder.Property(r => r.RacingNumber)
            .IsRequired();
            
        builder.Property(r => r.DateOfBirth)
            .IsRequired();
            
        builder.Property(r => r.CurrentTeamId);
            
        builder.Property(r => r.Nickname)
            .HasMaxLength(100);
            
        builder.Property(r => r.Photo)
            .HasMaxLength(500);
            
        builder.Property(r => r.Height)
            .HasPrecision(18, 2)
            .IsRequired();
            
        builder.Property(r => r.Weight)
            .HasPrecision(18, 2)
            .IsRequired();
            
        builder.Property(r => r.IsActive)
            .IsRequired();
            
        builder.Property(r => r.DebutDate);
        builder.Property(r => r.RetirementDate);

        // Nationality value object
        builder.OwnsOne(r => r.Nationality, nationality =>
        {
            nationality.Property(n => n.Code)
                .HasMaxLength(3)
                .IsRequired()
                .HasColumnName("NationalityCode");
            nationality.Property(n => n.Name)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("NationalityName");
            nationality.Property(n => n.Flag)
                .HasMaxLength(500)
                .HasColumnName("NationalityFlag");
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
        builder.HasIndex(r => r.RacingNumber)
            .IsUnique()
            .HasFilter("IsActive = 1 AND IsDeleted = 0");
        builder.HasIndex(r => r.CurrentTeamId);
        builder.HasIndex(r => r.IsActive);
        builder.HasIndex(r => r.IsDeleted);
        builder.HasIndex(r => new { r.LastName, r.FirstName });
        
        // Relationships
        builder.HasOne<Team>()
            .WithMany(t => t.Riders)
            .HasForeignKey(r => r.CurrentTeamId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
