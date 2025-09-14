using DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;
using DemoCICD.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoCICD.Persistence.Configurations.MotoGP;

internal sealed class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable(TableNames.Teams);

        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Name)
            .HasMaxLength(200)
            .IsRequired();
            
        builder.Property(t => t.ShortName)
            .HasMaxLength(50)
            .IsRequired();
            
        builder.Property(t => t.Logo)
            .HasMaxLength(500);
            
        builder.Property(t => t.Website)
            .HasMaxLength(500);
            
        builder.Property(t => t.FoundedYear)
            .IsRequired();
            
        builder.Property(t => t.Description)
            .HasMaxLength(1000);
            
        builder.Property(t => t.PrimaryColor)
            .HasMaxLength(50);
            
        builder.Property(t => t.SecondaryColor)
            .HasMaxLength(50);
            
        builder.Property(t => t.IsActive)
            .IsRequired();

        // Country value object
        builder.OwnsOne(t => t.Country, country =>
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

        // Audit fields
        builder.Property(t => t.CreatedAt)
            .IsRequired();
        builder.Property(t => t.UpdatedAt);
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(100);
        builder.Property(t => t.UpdatedBy)
            .HasMaxLength(100);
        builder.Property(t => t.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
        builder.Property(t => t.DeletedAt);
        builder.Property(t => t.DeletedBy)
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(t => t.Name)
            .IsUnique()
            .HasFilter("IsDeleted = 0");
        builder.HasIndex(t => t.ShortName)
            .IsUnique()
            .HasFilter("IsDeleted = 0");
        builder.HasIndex(t => t.IsActive);
        builder.HasIndex(t => t.IsDeleted);
        
        // Relationships - Riders and Bikes will be configured in their respective configurations
    }
}