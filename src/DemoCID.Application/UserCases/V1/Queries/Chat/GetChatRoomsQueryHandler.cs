using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Chat;
using DemoCICD.Domain.Abstractions.Reponsitories;
using DemoCICD.Domain.Entities.Chat;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DemoCICD.Application.UserCases.V1.Queries.Chat;

public sealed class GetChatRoomsQueryHandler : IQueryHandler<Query.GetChatRooms, Response.ChatRoomsResponse>
{
    private readonly IRepositoryBase<ChatRoom, Guid> _roomRepository;
    private readonly IRepositoryBase<ChatRoomMember, Guid> _memberRepository;

    public GetChatRoomsQueryHandler(
        IRepositoryBase<ChatRoom, Guid> roomRepository,
        IRepositoryBase<ChatRoomMember, Guid> memberRepository)
    {
        _roomRepository = roomRepository;
        _memberRepository = memberRepository;
    }

    public async Task<Result<Response.ChatRoomsResponse>> Handle(Query.GetChatRooms request, CancellationToken cancellationToken)
    {
        try
        {
            Log.Information("Getting chat rooms for user {UserId}", request.UserId);

            // Get rooms where the user is a member
            var userRoomsQuery = _memberRepository.FindAll(m => m.UserId == request.UserId && m.IsActive)
                .Include(m => m.Room)
                .ThenInclude(r => r.Messages)
                .Where(m => m.Room.IsActive)
                .Select(m => m.Room)
                .Distinct();

            // Apply pagination
            var totalCount = await userRoomsQuery.CountAsync(cancellationToken);
            
            var rooms = await userRoomsQuery
                .OrderByDescending(r => r.UpdatedAt ?? r.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var roomResponses = new List<Response.ChatRoomResponse>();

            foreach (var room in rooms)
            {
                // Get member count for each room
                var memberCount = await _memberRepository
                    .FindAll(m => m.RoomId == room.Id && m.IsActive)
                    .CountAsync(cancellationToken);

                // Get last message info
                var lastMessage = room.Messages
                    .Where(m => !m.IsDeleted)
                    .OrderByDescending(m => m.CreatedAt)
                    .FirstOrDefault();

                roomResponses.Add(new Response.ChatRoomResponse
                {
                    Id = room.Id,
                    Name = room.Name,
                    Description = room.Description,
                    Type = room.Type.ToString(),
                    IsActive = room.IsActive,
                    CreatedDate = room.CreatedAt,
                    MemberCount = memberCount,
                    LastMessageDate = lastMessage?.CreatedAt,
                    LastMessage = lastMessage?.Content
                });
            }

            var response = new Response.ChatRoomsResponse
            {
                Rooms = roomResponses,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                HasNext = (request.Page * request.PageSize) < totalCount
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting chat rooms for user {UserId}", request.UserId);
            return Result.Failure<Response.ChatRoomsResponse>(Error.Failure(
                "Chat.GetRooms.Failed",
                "Failed to get chat rooms"));
        }
    }
}