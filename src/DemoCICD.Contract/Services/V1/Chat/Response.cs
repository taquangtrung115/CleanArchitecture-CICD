namespace DemoCICD.Contract.Services.V1.Chat;

public static class Response
{
    public record ChatMessageResponse
    {
        public Guid Id { get; init; }
        public Guid SenderId { get; init; }
        public Guid? ReceiverId { get; init; }
        public Guid? RoomId { get; init; }
        public string Content { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public bool IsRead { get; init; }
        public DateTime CreatedDate { get; init; }
        public string SenderName { get; init; } = string.Empty;
    }

    public record ChatRoomResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string Type { get; init; } = string.Empty;
        public bool IsActive { get; init; }
        public DateTime CreatedDate { get; init; }
        public int MemberCount { get; init; }
        public DateTime? LastMessageDate { get; init; }
        public string? LastMessage { get; init; }
    }

    public record ChatHistoryResponse
    {
        public List<ChatMessageResponse> Messages { get; init; } = new();
        public int TotalCount { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
        public bool HasNext { get; init; }
    }

    public record ChatRoomsResponse
    {
        public List<ChatRoomResponse> Rooms { get; init; } = new();
        public int TotalCount { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
        public bool HasNext { get; init; }
    }

    public record RoomMemberResponse
    {
        public Guid UserId { get; init; }
        public string UserName { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public DateTime JoinedDate { get; init; }
        public DateTime? LastSeenDate { get; init; }
        public bool IsOnline { get; init; }
    }

    public record RoomMembersResponse
    {
        public List<RoomMemberResponse> Members { get; init; } = new();
        public int TotalCount { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
        public bool HasNext { get; init; }
    }

    public record OnlineUserResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public DateTime LastSeen { get; init; }
        public bool IsOnline { get; init; }
    }

    public record OnlineUsersResponse
    {
        public List<OnlineUserResponse> Users { get; init; } = new();
    }

    public record MembershipResponse
    {
        public bool Success { get; init; }
        public string Message { get; init; } = string.Empty;
    }

    public record MessageReadResponse
    {
        public bool Success { get; init; }
        public DateTime ReadAt { get; init; }
    }

    public record UnreadCountResponse
    {
        public int TotalUnread { get; init; }
        public Dictionary<string, int> UnreadByConversation { get; init; } = new();
    }
}