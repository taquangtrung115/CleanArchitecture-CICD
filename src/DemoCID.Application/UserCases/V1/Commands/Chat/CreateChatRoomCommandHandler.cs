using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Chat;
using Serilog;

namespace DemoCICD.Application.UserCases.V1.Commands.Chat;

public sealed class CreateChatRoomCommandHandler : ICommandHandler<Command.CreateChatRoom, Response.ChatRoomResponse>
{
    public CreateChatRoomCommandHandler()
    {
    }

    public async Task<Result<Response.ChatRoomResponse>> Handle(Command.CreateChatRoom request, CancellationToken cancellationToken)
    {
        try
        {
            var roomId = Guid.NewGuid();
            var timestamp = DateTime.UtcNow;

            Log.Information("Creating chat room: {Name} of type {Type}", request.Name, request.Type);

            // Simulate creating the room
            await Task.Delay(100, cancellationToken);

            var response = new Response.ChatRoomResponse
            {
                Id = roomId,
                Name = request.Name,
                Description = request.Description,
                Type = request.Type,
                IsActive = true,
                CreatedDate = timestamp,
                MemberCount = request.MemberIds?.Count ?? 0,
                LastMessageDate = null,
                LastMessage = null
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating chat room: {Name}", request.Name);
            return Result.Failure<Response.ChatRoomResponse>(Error.Failure(
                "Chat.CreateRoom.Failed",
                "Failed to create chat room"));
        }
    }
}