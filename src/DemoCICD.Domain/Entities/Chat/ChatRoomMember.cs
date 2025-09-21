using DemoCICD.Domain.Abstractions.Entities;

namespace DemoCICD.Domain.Entities.Chat;

public class ChatRoomMember : AuditableEntity<Guid>
{
    public Guid RoomId { get; set; }
    public Guid UserId { get; set; }
    public MemberRole Role { get; set; } = MemberRole.Member;
    public DateTime JoinedDate { get; set; }
    public DateTime? LastSeenDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ChatRoom Room { get; set; } = null!;
}

public enum MemberRole
{
    Member = 0,
    Admin = 1,
    Owner = 2
}