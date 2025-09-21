using DemoCICD.Domain.Abstractions.Entities;

namespace DemoCICD.Domain.Entities.Chat;

public class ChatRoom : AuditableEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public RoomType Type { get; set; } = RoomType.Group;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<ChatRoomMember> Members { get; set; } = new List<ChatRoomMember>();
    public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}

public enum RoomType
{
    Direct = 0,
    Group = 1,
    Public = 2
}