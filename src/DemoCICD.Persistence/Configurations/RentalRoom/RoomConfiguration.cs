using DemoCICD.Domain.Entities.RentalRoom.Rooms;
using DemoCICD.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoCICD.Persistence.Configurations.RentalRoom;

/// <summary>
/// EF Core Configuration cho Room entity
/// </summary>
internal class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable(TableNames.Rooms);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.RoomNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Capacity)
            .IsRequired();

        builder.Property(x => x.PricePerNight)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.IsAvailable)
            .IsRequired();

        builder.Property(x => x.LocationId)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(x => x.RoomNumber).IsUnique();
        builder.HasIndex(x => x.IsAvailable);
        builder.HasIndex(x => x.LocationId);
    }
}
