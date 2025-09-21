using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Chat;
using Serilog;

namespace DemoCICD.Application.UserCases.V1.Queries.Chat;

public sealed class GetChatHistoryQueryHandler : IQueryHandler<Query.GetChatHistory, Response.ChatHistoryResponse>
{
    public GetChatHistoryQueryHandler()
    {
    }

    public async Task<Result<Response.ChatHistoryResponse>> Handle(Query.GetChatHistory request, CancellationToken cancellationToken)
    {
        try
        {
            Log.Information("Getting chat history for user {UserId} with {OtherUserId} in room {RoomId}", 
                request.UserId, request.OtherUserId, request.RoomId);

            // Simulate loading from database
            await Task.Delay(200, cancellationToken);

            // Mock chat history data
            var messages = new List<Response.ChatMessageResponse>();

            // Add some sample messages for demo
            if (request.OtherUserId.HasValue || request.RoomId.HasValue)
            {
                var baseTime = DateTime.UtcNow.AddHours(-2);
                
                messages.AddRange(new[]
                {
                    new Response.ChatMessageResponse
                    {
                        Id = Guid.NewGuid(),
                        SenderId = request.UserId,
                        ReceiverId = request.OtherUserId,
                        RoomId = request.RoomId,
                        Content = "Hello! How are you doing?",
                        Type = "Text",
                        IsRead = true,
                        CreatedDate = baseTime.AddMinutes(10),
                        SenderName = "You"
                    },
                    new Response.ChatMessageResponse
                    {
                        Id = Guid.NewGuid(),
                        SenderId = request.OtherUserId ?? Guid.NewGuid(),
                        ReceiverId = request.UserId,
                        RoomId = request.RoomId,
                        Content = "Hi there! I'm doing great, thanks for asking. What about you?",
                        Type = "Text",
                        IsRead = true,
                        CreatedDate = baseTime.AddMinutes(15),
                        SenderName = "Demo User"
                    },
                    new Response.ChatMessageResponse
                    {
                        Id = Guid.NewGuid(),
                        SenderId = request.UserId,
                        ReceiverId = request.OtherUserId,
                        RoomId = request.RoomId,
                        Content = "I'm doing well too! Working on the new chat feature.",
                        Type = "Text",
                        IsRead = true,
                        CreatedDate = baseTime.AddMinutes(20),
                        SenderName = "You"
                    }
                });
            }

            var response = new Response.ChatHistoryResponse
            {
                Messages = messages,
                TotalCount = messages.Count,
                Page = request.Page,
                PageSize = request.PageSize,
                HasNext = false
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