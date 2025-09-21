using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Chat;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Reponsitories;
using DemoCICD.Domain.Entities.Chat;
using DemoCICD.Domain.Entities.Identity;
using Serilog;

namespace DemoCICD.Application.UserCases.V1.Commands.Chat;

public sealed class CreateChatRoomCommandHandler : ICommandHandler<Command.CreateChatRoom, Response.ChatRoomResponse>
{
    private readonly IRepositoryBase<ChatRoom, Guid> _roomRepository;
    private readonly IRepositoryBase<ChatRoomMember, Guid> _memberRepository;
    private readonly IAppUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateChatRoomCommandHandler(
        IRepositoryBase<ChatRoom, Guid> roomRepository,
        IRepositoryBase<ChatRoomMember, Guid> memberRepository,
        IAppUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _memberRepository = memberRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Response.ChatRoomResponse>> Handle(Command.CreateChatRoom request, CancellationToken cancellationToken)
    {
        try
        {
            Log.Information("Creating chat room: {Name} of type {Type}", request.Name, request.Type);

            // Parse room type
            if (!Enum.TryParse<RoomType>(request.Type, true, out var roomType))
            {
                roomType = RoomType.Group;
            }

            // Create the room entity
            var room = new ChatRoom
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Type = roomType,
                IsActive = true
            };

            // Set audit fields
            room.SetCreatedAudit("System"); // TODO: Get from current user context

            // Save the room
            _roomRepository.Add(room);

            // Add members if provided
            var memberCount = 0;
            if (request.MemberIds != null && request.MemberIds.Any())
            {
                foreach (var memberId in request.MemberIds)
                {
                    // Verify user exists
                    var user = await _userRepository.FindByIdAsync(memberId, cancellationToken);
                    if (user != null)
                    {
                        var member = new ChatRoomMember
                        {
                            Id = Guid.NewGuid(),
                            RoomId = room.Id,
                            UserId = memberId,
                            Role = MemberRole.Member,
                            JoinedDate = DateTime.UtcNow,
                            IsActive = true
                        };

                        // Set audit fields
                        member.SetCreatedAudit("System");

                        _memberRepository.Add(member);
                        memberCount++;
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new Response.ChatRoomResponse
            {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                Type = room.Type.ToString(),
                IsActive = room.IsActive,
                CreatedDate = room.CreatedAt,
                MemberCount = memberCount,
                LastMessageDate = null,
                LastMessage = null
            };

            Log.Information("Chat room created successfully with ID {RoomId}", room.Id);
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