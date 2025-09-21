using DemoCICD.Domain.Entities.Chat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoCICD.Persistence.Configurations.Chat;

internal sealed class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable("ChatMessages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SenderId)
            .IsRequired();

        builder.Property(x => x.ReceiverId)
            .IsRequired(false);

        builder.Property(x => x.RoomId)
            .IsRequired(false);

        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(x => x.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.IsRead)
            .IsRequired()
            .HasDefaultValue(false);

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
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.SetNull);

        // Add indexes
        builder.HasIndex(x => x.SenderId);
        builder.HasIndex(x => x.ReceiverId);
        builder.HasIndex(x => x.RoomId);
        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.IsRead);
        builder.HasIndex(x => new { x.SenderId, x.ReceiverId });
        builder.HasIndex(x => new { x.RoomId, x.CreatedAt });
    }
}