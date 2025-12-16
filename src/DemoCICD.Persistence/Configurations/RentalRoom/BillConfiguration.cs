using DemoCICD.Contract.Enumerations;
using DemoCICD.Domain.Entities.RentalRoom.Bills;
using DemoCICD.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoCICD.Persistence.Configurations.RentalRoom;

/// <summary>
/// EF Core Configuration cho Bill entity
/// </summary>
internal class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.ToTable(TableNames.Bills);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BillNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.RoomId)
            .IsRequired();

        builder.Property(x => x.ProfileId)
            .IsRequired();

        builder.Property(x => x.Month)
            .IsRequired();

        builder.Property(x => x.Year)
            .IsRequired();

        builder.Property(x => x.RoomPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.ServiceTotal)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.PaidAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.RemainingAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion(
                v => v.Value,
                v => BillStatus.FromValue(v))
            .IsRequired();

        builder.Property(x => x.PaymentMethod)
            .HasMaxLength(50);

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        // Relationships
        builder.HasMany(x => x.BillDetails)
            .WithOne()
            .HasForeignKey(d => d.BillId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(x => x.BillNumber).IsUnique();
        builder.HasIndex(x => x.RoomId);
        builder.HasIndex(x => x.ProfileId);
        builder.HasIndex(x => new { x.Month, x.Year });
        builder.HasIndex(x => x.Status);
    }
}
