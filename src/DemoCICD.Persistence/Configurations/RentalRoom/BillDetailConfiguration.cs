using DemoCICD.Contract.Enumerations;
using DemoCICD.Domain.Entities.RentalRoom.Bills;
using DemoCICD.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoCICD.Persistence.Configurations.RentalRoom;

/// <summary>
/// EF Core Configuration cho BillDetail entity
/// </summary>
internal class BillDetailConfiguration : IEntityTypeConfiguration<BillDetail>
{
    public void Configure(EntityTypeBuilder<BillDetail> builder)
    {
        builder.ToTable(TableNames.BillDetails);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BillId)
            .IsRequired();

        builder.Property(x => x.ServiceType)
            .HasConversion(
                v => v.Value,
                v => ServiceType.FromValue(v))
            .IsRequired();

        builder.Property(x => x.ServiceName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Unit)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.TotalPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.OldIndex)
            .HasPrecision(18, 2);

        builder.Property(x => x.NewIndex)
            .HasPrecision(18, 2);

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(x => x.BillId);
        builder.HasIndex(x => x.ServiceType);
    }
}
