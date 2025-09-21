using DemoCICD.Contract.Abstractions.Message;

namespace DemoCICD.Contract.Services.V1.Chat;

public static class Query
{
    public record GetChatHistory(
        Guid UserId,
        Guid? OtherUserId = null,
        Guid? RoomId = null,
        int Page = 1,
        int PageSize = 50
    ) : IQuery<Response.ChatHistoryResponse>;

    public record GetChatRooms(
        Guid UserId,
        int Page = 1,
        int PageSize = 20
    ) : IQuery<Response.ChatRoomsResponse>;

    public record GetRoomMembers(
        Guid RoomId,
        int Page = 1,
        int PageSize = 50
    ) : IQuery<Response.RoomMembersResponse>;

    public record GetOnlineUsers(
        Guid CurrentUserId
    ) : IQuery<Response.OnlineUsersResponse>;

    public record GetUnreadMessageCount(
        Guid UserId
    ) : IQuery<Response.UnreadCountResponse>;
}