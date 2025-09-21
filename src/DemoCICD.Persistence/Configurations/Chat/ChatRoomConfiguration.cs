using DemoCICD.Domain.Entities.Chat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoCICD.Persistence.Configurations.Chat;

internal sealed class ChatRoomConfiguration : IEntityTypeConfiguration<ChatRoom>
{
    public void Configure(EntityTypeBuilder<ChatRoom> builder)
    {
        builder.ToTable("ChatRooms");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Configure audit fields
        builder.Property(x => x.CreatedBy)
            .HasMaxLength(50);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(50);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.DeletedAt);

        builder.Property(x => x.DeletedBy)
            .HasMaxLength(50);

        // Configure relationships
        builder.HasMany(x => x.Members)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Messages)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        // Add indexes
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.Type);
        builder.HasIndex(x => x.IsActive);
        builder.HasIndex(x => x.CreatedAt);
    }
}