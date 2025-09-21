using Carter;
using DemoCICD.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using DemoCICD.Contract.Services.V1.Chat;

namespace DemoCICD.Presentation.APIs.Chat;

public class UserChatApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/user-chat";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("User Chat")
            .MapGroup(BaseUrl).HasApiVersion(1);

        // Message operations
        group1.MapPost("messages", SendMessageV1).RequireAuthorization();
        group1.MapGet("messages/history", GetChatHistoryV1).RequireAuthorization();
        group1.MapPatch("messages/{messageId:guid}/read", MarkMessageAsReadV1).RequireAuthorization();
        group1.MapGet("messages/unread-count", GetUnreadCountV1).RequireAuthorization();

        // Room operations
        group1.MapPost("rooms", CreateChatRoomV1).RequireAuthorization();
        group1.MapGet("rooms", GetChatRoomsV1).RequireAuthorization();
        group1.MapPost("rooms/{roomId:guid}/join", JoinChatRoomV1).RequireAuthorization();
        group1.MapPost("rooms/{roomId:guid}/leave", LeaveChatRoomV1).RequireAuthorization();
        group1.MapGet("rooms/{roomId:guid}/members", GetRoomMembersV1).RequireAuthorization();

        // User operations
        group1.MapGet("users/online", GetOnlineUsersV1).RequireAuthorization();
    }

    public static async Task<IResult> SendMessageV1(
        ISender sender,
        [FromBody] Command.SendMessage command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> GetChatHistoryV1(
        ISender sender,
        [FromQuery] Guid userId,
        [FromQuery] Guid? otherUserId = null,
        [FromQuery] Guid? roomId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        var query = new Query.GetChatHistory(userId, otherUserId, roomId, page, pageSize);
        var result = await sender.Send(query);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> MarkMessageAsReadV1(
        ISender sender,
        [FromRoute] Guid messageId,
        [FromQuery] Guid userId)
    {
        var command = new Command.MarkMessageAsRead(messageId, userId);
        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> GetUnreadCountV1(
        ISender sender,
        [FromQuery] Guid userId)
    {
        var query = new Query.GetUnreadMessageCount(userId);
        var result = await sender.Send(query);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> CreateChatRoomV1(
        ISender sender,
        [FromBody] Command.CreateChatRoom command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> GetChatRoomsV1(
        ISender sender,
        [FromQuery] Guid userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new Query.GetChatRooms(userId, page, pageSize);
        var result = await sender.Send(query);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> JoinChatRoomV1(
        ISender sender,
        [FromRoute] Guid roomId,
        [FromQuery] Guid userId)
    {
        var command = new Command.JoinChatRoom(roomId, userId);
        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> LeaveChatRoomV1(
        ISender sender,
        [FromRoute] Guid roomId,
        [FromQuery] Guid userId)
    {
        var command = new Command.LeaveChatRoom(roomId, userId);
        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> GetRoomMembersV1(
        ISender sender,
        [FromRoute] Guid roomId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        var query = new Query.GetRoomMembers(roomId, page, pageSize);
        var result = await sender.Send(query);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> GetOnlineUsersV1(
        ISender sender,
        [FromQuery] Guid currentUserId)
    {
        var query = new Query.GetOnlineUsers(currentUserId);
        var result = await sender.Send(query);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}