using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Chat;
using Serilog;

namespace DemoCICD.Application.UserCases.V1.Commands.Chat;

public sealed class SendMessageCommandHandler : ICommandHandler<Command.SendMessage, Response.ChatMessageResponse>
{
    public SendMessageCommandHandler()
    {
    }

    public async Task<Result<Response.ChatMessageResponse>> Handle(Command.SendMessage request, CancellationToken cancellationToken)
    {
        try
        {
            // For demo purposes - in a real implementation, this would save to database
            // and use the domain entities we created
            
            var messageId = Guid.NewGuid();
            var timestamp = DateTime.UtcNow;

            Log.Information("Processing chat message from {SenderId} to {ReceiverId} in room {RoomId}: {Content}", 
                request.SenderId, request.ReceiverId, request.RoomId, request.Content);

            // Simulate saving the message
            await Task.Delay(100, cancellationToken);

            var response = new Response.ChatMessageResponse
            {
                Id = messageId,
                SenderId = request.SenderId,
                ReceiverId = request.ReceiverId,
                RoomId = request.RoomId,
                Content = request.Content,
                Type = request.Type,
                IsRead = false,
                CreatedDate = timestamp,
                SenderName = "Demo User" // In real implementation, get from user service
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error sending chat message from {SenderId} to {ReceiverId}", request.SenderId, request.ReceiverId);
            return Result.Failure<Response.ChatMessageResponse>(Error.Failure(
                "Chat.SendMessage.Failed",
                "Failed to send message"));
        }
    }
}