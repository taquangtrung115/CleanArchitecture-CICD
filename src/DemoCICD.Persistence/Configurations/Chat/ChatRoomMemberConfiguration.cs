using DemoCICD.Domain.Entities.Chat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoCICD.Persistence.Configurations.Chat;

internal sealed class ChatRoomMemberConfiguration : IEntityTypeConfiguration<ChatRoomMember>
{
    public void Configure(EntityTypeBuilder<ChatRoomMember> builder)
    {
        builder.ToTable("ChatRoomMembers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.RoomId)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.Role)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.JoinedDate)
            .IsRequired();

        builder.Property(x => x.LastSeenDate)
            .IsRequired(false);

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
        builder.HasOne(x => x.Room)
            .WithMany(x => x.Members)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        // Add unique constraint for user-room combination
        builder.HasIndex(x => new { x.RoomId, x.UserId })
            .IsUnique()
            .HasDatabaseName("IX_ChatRoomMembers_RoomId_UserId_Unique");

        // Add indexes
        builder.HasIndex(x => x.RoomId);
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.JoinedDate);
        builder.HasIndex(x => x.IsActive);
    }
}