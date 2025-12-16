using DemoCICD.Domain.Entities.RentalRoom.Profiles;
using DemoCICD.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoCICD.Persistence.Configurations.RentalRoom;

/// <summary>
/// EF Core Configuration cho Profile entity
/// </summary>
internal class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable(TableNames.Profiles);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(100);

        builder.Property(x => x.IdentityCard)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.PermanentAddress)
            .HasMaxLength(500);

        builder.Property(x => x.Occupation)
            .HasMaxLength(100);

        builder.Property(x => x.DepositAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.IsActive)
            .IsRequired();

        // Indexes
        builder.HasIndex(x => x.IdentityCard).IsUnique();
        builder.HasIndex(x => x.PhoneNumber);
        builder.HasIndex(x => x.RoomId);
        builder.HasIndex(x => x.IsActive);
    }
}
