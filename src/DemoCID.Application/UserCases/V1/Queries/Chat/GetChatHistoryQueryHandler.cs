using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Chat;
using DemoCICD.Domain.Abstractions.Reponsitories;
using DemoCICD.Domain.Entities.Chat;
using DemoCICD.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DemoCICD.Application.UserCases.V1.Queries.Chat;

public sealed class GetChatHistoryQueryHandler : IQueryHandler<Query.GetChatHistory, Response.ChatHistoryResponse>
{
    private readonly IRepositoryBase<ChatMessage, Guid> _messageRepository;
    private readonly IAppUserRepository _userRepository;

    public GetChatHistoryQueryHandler(
        IRepositoryBase<ChatMessage, Guid> messageRepository,
        IAppUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<Response.ChatHistoryResponse>> Handle(Query.GetChatHistory request, CancellationToken cancellationToken)
    {
        try
        {
            Log.Information("Getting chat history for user {UserId} with {OtherUserId} in room {RoomId}", 
                request.UserId, request.OtherUserId, request.RoomId);

            IQueryable<ChatMessage> messagesQuery;

            if (request.RoomId.HasValue)
            {
                // Get room messages
                messagesQuery = _messageRepository.FindAll(m => m.RoomId == request.RoomId);
            }
            else if (request.OtherUserId.HasValue)
            {
                // Get direct messages between two users
                messagesQuery = _messageRepository.FindAll(m => 
                    (m.SenderId == request.UserId && m.ReceiverId == request.OtherUserId) ||
                    (m.SenderId == request.OtherUserId && m.ReceiverId == request.UserId));
            }
            else
            {
                // Get all messages for the user (both sent and received)
                messagesQuery = _messageRepository.FindAll(m => 
                    m.SenderId == request.UserId || m.ReceiverId == request.UserId);
            }

            // Get total count before pagination
            var totalCount = await messagesQuery.CountAsync(cancellationToken);

            // Apply pagination and ordering
            var messages = await messagesQuery
                .OrderByDescending(m => m.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            // Get sender information for each message
            var messageResponses = new List<Response.ChatMessageResponse>();
            var userCache = new Dictionary<Guid, AppUser>();

            foreach (var message in messages.OrderBy(m => m.CreatedAt)) // Re-order for display
            {
                // Get sender info from cache or database
                if (!userCache.TryGetValue(message.SenderId, out var sender))
                {
                    sender = await _userRepository.FindByIdAsync(message.SenderId, cancellationToken);
                    if (sender != null)
                    {
                        userCache[message.SenderId] = sender;
                    }
                }

                var senderName = sender != null 
                    ? (sender.UserName ?? $"{sender.FirstName} {sender.LastName}".Trim())
                    : "Unknown User";

                messageResponses.Add(new Response.ChatMessageResponse
                {
                    Id = message.Id,
                    SenderId = message.SenderId,
                    ReceiverId = message.ReceiverId,
                    RoomId = message.RoomId,
                    Content = message.Content,
                    Type = message.Type.ToString(),
                    IsRead = message.IsRead,
                    CreatedDate = message.CreatedAt,
                    SenderName = senderName
                });
            }

            var response = new Response.ChatHistoryResponse
            {
                Messages = messageResponses,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                HasNext = (request.Page * request.PageSize) < totalCount
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting chat history for user {UserId}", request.UserId);
            return Result.Failure<Response.ChatHistoryResponse>(Error.Failure(
                "Chat.GetHistory.Failed",
                "Failed to get chat history"));
        }
    }
}