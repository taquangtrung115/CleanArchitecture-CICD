using DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;
using DemoCICD.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoCICD.Persistence.Configurations.MotoGP;

internal sealed class BikeConfiguration : IEntityTypeConfiguration<Bike>
{
    public void Configure(EntityTypeBuilder<Bike> builder)
    {
        builder.ToTable(TableNames.Bikes);

        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.Manufacturer)
            .HasMaxLength(100)
            .IsRequired();
            
        builder.Property(b => b.Model)
            .HasMaxLength(100)
            .IsRequired();
            
        builder.Property(b => b.Year)
            .IsRequired();
            
        builder.Property(b => b.TeamId);
            
        builder.Property(b => b.ChassisNumber)
            .HasMaxLength(100);
            
        builder.Property(b => b.EngineNumber)
            .HasMaxLength(100);
            
        builder.Property(b => b.Livery)
            .HasMaxLength(500);
            
        builder.Property(b => b.IsActive)
            .IsRequired();
            
        builder.Property(b => b.Notes)
            .HasMaxLength(1000);

        // BikeSpec value object
        builder.OwnsOne(b => b.Specifications, spec =>
        {
            spec.Property(s => s.Engine)
                .HasMaxLength(200)
                .IsRequired()
                .HasColumnName("SpecEngine");
            spec.Property(s => s.Displacement)
                .IsRequired()
                .HasColumnName("SpecDisplacement");
            spec.Property(s => s.MaxPower)
                .IsRequired()
                .HasColumnName("SpecMaxPower");
            spec.Property(s => s.MaxSpeed)
                .HasPrecision(18, 2)
                .IsRequired()
                .HasColumnName("SpecMaxSpeed");
            spec.Property(s => s.Weight)
                .HasPrecision(18, 2)
                .IsRequired()
                .HasColumnName("SpecWeight");
            spec.Property(s => s.Transmission)
                .HasMaxLength(100)
                .HasColumnName("SpecTransmission");
            spec.Property(s => s.Frame)
                .HasMaxLength(100)
                .HasColumnName("SpecFrame");
            spec.Property(s => s.FrontSuspension)
                .HasMaxLength(100)
                .HasColumnName("SpecFrontSuspension");
            spec.Property(s => s.RearSuspension)
                .HasMaxLength(100)
                .HasColumnName("SpecRearSuspension");
            spec.Property(s => s.FrontBrakes)
                .HasMaxLength(100)
                .HasColumnName("SpecFrontBrakes");
            spec.Property(s => s.RearBrakes)
                .HasMaxLength(100)
                .HasColumnName("SpecRearBrakes");
        });

        // Audit fields
        builder.Property(b => b.CreatedAt)
            .IsRequired();
        builder.Property(b => b.UpdatedAt);
        builder.Property(b => b.CreatedBy)
            .HasMaxLength(100);
        builder.Property(b => b.UpdatedBy)
            .HasMaxLength(100);
        builder.Property(b => b.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
        builder.Property(b => b.DeletedAt);
        builder.Property(b => b.DeletedBy)
            .HasMaxLength(100);

        // Computed column for FullName (remove this line to avoid EF Core mapping error)
        // builder.Property(b => b.FullName)
        //     .HasComputedColumnSql("[Manufacturer] + ' ' + [Model] + ' (' + CAST([Year] AS VARCHAR) + ')'", stored: false);

        // Indexes
        builder.HasIndex(b => b.TeamId);
        builder.HasIndex(b => b.Manufacturer);
        builder.HasIndex(b => b.Year);
        builder.HasIndex(b => b.ChassisNumber)
            .IsUnique()
            .HasFilter("ChassisNumber IS NOT NULL AND IsDeleted = 0");
        builder.HasIndex(b => b.EngineNumber)
            .IsUnique()
            .HasFilter("EngineNumber IS NOT NULL AND IsDeleted = 0");
        builder.HasIndex(b => b.IsActive);
        builder.HasIndex(b => b.IsDeleted);
        
        // Relationships
        builder.HasOne<Team>()
            .WithMany(t => t.Bikes)
            .HasForeignKey(b => b.TeamId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
