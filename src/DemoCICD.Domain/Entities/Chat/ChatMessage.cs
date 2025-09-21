using DemoCICD.Domain.Abstractions.Entities;

namespace DemoCICD.Domain.Entities.Chat;

public class ChatMessage : AuditableEntity<Guid>
{
    public Guid SenderId { get; set; }
    public Guid? ReceiverId { get; set; } // null for room messages
    public Guid? RoomId { get; set; } // null for direct messages
    public string Content { get; set; } = string.Empty;
    public MessageType Type { get; set; } = MessageType.Text;
    public bool IsRead { get; set; } = false;

    // Navigation properties
    public virtual ChatRoom? Room { get; set; }
}

public enum MessageType
{
    Text = 0,
    Image = 1,
    File = 2,
    System = 3
}