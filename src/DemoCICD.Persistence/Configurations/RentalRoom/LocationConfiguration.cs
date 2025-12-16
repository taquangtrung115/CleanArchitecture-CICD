using DemoCICD.Domain.Entities.RentalRoom.Locations;
using DemoCICD.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoCICD.Persistence.Configurations.RentalRoom;

/// <summary>
/// EF Core Configuration cho Location entity
/// </summary>
internal class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable(TableNames.Locations);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Street)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Ward)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.District)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.City)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.FullAddress)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.PostalCode)
            .HasMaxLength(20);

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(x => x.City);
        builder.HasIndex(x => x.District);
    }
}
