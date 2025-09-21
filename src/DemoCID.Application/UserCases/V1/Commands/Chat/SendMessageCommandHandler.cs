using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Chat;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Reponsitories;
using DemoCICD.Domain.Entities.Chat;
using DemoCICD.Domain.Entities.Identity;
using Serilog;

namespace DemoCICD.Application.UserCases.V1.Commands.Chat;

public sealed class SendMessageCommandHandler : ICommandHandler<Command.SendMessage, Response.ChatMessageResponse>
{
    private readonly IRepositoryBase<ChatMessage, Guid> _messageRepository;
    private readonly IAppUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SendMessageCommandHandler(
        IRepositoryBase<ChatMessage, Guid> messageRepository,
        IAppUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Response.ChatMessageResponse>> Handle(Command.SendMessage request, CancellationToken cancellationToken)
    {
        try
        {
            Log.Information("Processing chat message from {SenderId} to {ReceiverId} in room {RoomId}: {Content}", 
                request.SenderId, request.ReceiverId, request.RoomId, request.Content);

            // Get sender information
            var sender = await _userRepository.FindByIdAsync(request.SenderId, cancellationToken);
            if (sender == null)
            {
                return Result.Failure<Response.ChatMessageResponse>(Error.NotFound(
                    "Chat.Sender.NotFound",
                    "Sender not found"));
            }

            // Validate receiver if it's a direct message
            if (request.ReceiverId.HasValue && request.RoomId == null)
            {
                var receiver = await _userRepository.FindByIdAsync(request.ReceiverId.Value, cancellationToken);
                if (receiver == null)
                {
                    return Result.Failure<Response.ChatMessageResponse>(Error.NotFound(
                        "Chat.Receiver.NotFound",
                        "Receiver not found"));
                }
            }

            // Parse message type
            if (!Enum.TryParse<MessageType>(request.Type, true, out var messageType))
            {
                messageType = MessageType.Text;
            }

            // Create the message entity
            var message = new ChatMessage
            {
                Id = Guid.NewGuid(),
                SenderId = request.SenderId,
                ReceiverId = request.ReceiverId,
                RoomId = request.RoomId,
                Content = request.Content,
                Type = messageType,
                IsRead = false
            };

            // Set audit fields
            message.SetCreatedAudit(sender.UserName ?? sender.Id.ToString());

            // Save to database
            _messageRepository.Add(message);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new Response.ChatMessageResponse
            {
                Id = message.Id,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                RoomId = message.RoomId,
                Content = message.Content,
                Type = message.Type.ToString(),
                IsRead = message.IsRead,
                CreatedDate = message.CreatedAt,
                SenderName = sender.UserName ?? $"{sender.FirstName} {sender.LastName}".Trim()
            };

            Log.Information("Chat message saved successfully with ID {MessageId}", message.Id);
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