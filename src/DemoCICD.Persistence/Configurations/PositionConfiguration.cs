using DemoCICD.Domain.Entities.Identity;
using DemoCICD.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoCICD.Persistence.Configurations;

internal sealed class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable(TableNames.Positions);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.Code)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Level)
            .HasDefaultValue(1);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(100);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("IX_Position_Code");

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_Position_Name");

        // One Position has many Users
        builder.HasMany(p => p.Users)
            .WithOne()
            .HasForeignKey(u => u.PositionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}