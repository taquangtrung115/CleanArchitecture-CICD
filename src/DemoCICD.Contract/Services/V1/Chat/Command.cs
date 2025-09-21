using DemoCICD.Contract.Abstractions.Message;

namespace DemoCICD.Contract.Services.V1.Chat;

public static class Command
{
    public record SendMessage(
        Guid SenderId,
        Guid? ReceiverId,
        Guid? RoomId,
        string Content,
        string Type = "Text"
    ) : ICommand<Response.ChatMessageResponse>;

    public record CreateChatRoom(
        string Name,
        string? Description,
        string Type = "Group",
        List<Guid>? MemberIds = null
    ) : ICommand<Response.ChatRoomResponse>;

    public record JoinChatRoom(
        Guid RoomId,
        Guid UserId
    ) : ICommand<Response.MembershipResponse>;

    public record LeaveChatRoom(
        Guid RoomId,
        Guid UserId
    ) : ICommand<Response.MembershipResponse>;

    public record MarkMessageAsRead(
        Guid MessageId,
        Guid UserId
    ) : ICommand<Response.MessageReadResponse>;
}