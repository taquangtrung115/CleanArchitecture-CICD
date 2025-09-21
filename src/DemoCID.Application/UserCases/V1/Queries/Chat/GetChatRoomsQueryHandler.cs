using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Chat;
using Serilog;

namespace DemoCICD.Application.UserCases.V1.Queries.Chat;

public sealed class GetChatRoomsQueryHandler : IQueryHandler<Query.GetChatRooms, Response.ChatRoomsResponse>
{
    public GetChatRoomsQueryHandler()
    {
    }

    public async Task<Result<Response.ChatRoomsResponse>> Handle(Query.GetChatRooms request, CancellationToken cancellationToken)
    {
        try
        {
            Log.Information("Getting chat rooms for user {UserId}", request.UserId);

            // Simulate loading from database
            await Task.Delay(150, cancellationToken);

            // Mock chat rooms data
            var rooms = new List<Response.ChatRoomResponse>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Alice Johnson",
                    Description = "Direct conversation",
                    Type = "Direct",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddDays(-5),
                    MemberCount = 2,
                    LastMessageDate = DateTime.UtcNow.AddMinutes(-15),
                    LastMessage = "Thanks for the update!"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Development Team",
                    Description = "Team discussions and updates",
                    Type = "Group",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddDays(-10),
                    MemberCount = 5,
                    LastMessageDate = DateTime.UtcNow.AddHours(-2),
                    LastMessage = "Let's review the new features tomorrow"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Bob Smith",
                    Description = "Direct conversation",
                    Type = "Direct",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddDays(-3),
                    MemberCount = 2,
                    LastMessageDate = DateTime.UtcNow.AddHours(-4),
                    LastMessage = "See you tomorrow!"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Project Alpha",
                    Description = "Project Alpha coordination",
                    Type = "Group",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddDays(-7),
                    MemberCount = 8,
                    LastMessageDate = DateTime.UtcNow.AddMinutes(-30),
                    LastMessage = "The new deployment looks good"
                }
            };

            var response = new Response.ChatRoomsResponse
            {
                Rooms = rooms,
                TotalCount = rooms.Count,
                Page = request.Page,
                PageSize = request.PageSize,
                HasNext = false
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