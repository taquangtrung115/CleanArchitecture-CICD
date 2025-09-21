using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Chat;
using Serilog;

namespace DemoCICD.Application.UserCases.V1.Queries.Chat;

public sealed class GetOnlineUsersQueryHandler : IQueryHandler<Query.GetOnlineUsers, Response.OnlineUsersResponse>
{
    public GetOnlineUsersQueryHandler()
    {
    }

    public async Task<Result<Response.OnlineUsersResponse>> Handle(Query.GetOnlineUsers request, CancellationToken cancellationToken)
    {
        try
        {
            Log.Information("Getting online users for user {UserId}", request.CurrentUserId);

            // Simulate loading from database or cache
            await Task.Delay(100, cancellationToken);

            // Mock online users data
            var onlineUsers = new List<Response.OnlineUserResponse>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Alice Johnson",
                    Email = "alice@example.com",
                    LastSeen = DateTime.UtcNow.AddMinutes(-2),
                    IsOnline = true
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Bob Smith",
                    Email = "bob@example.com",
                    LastSeen = DateTime.UtcNow.AddMinutes(-1),
                    IsOnline = true
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Carol Davis",
                    Email = "carol@example.com",
                    LastSeen = DateTime.UtcNow.AddMinutes(-30),
                    IsOnline = false
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "David Wilson",
                    Email = "david@example.com",
                    LastSeen = DateTime.UtcNow,
                    IsOnline = true
                }
            }.Where(u => u.Id != request.CurrentUserId).ToList();

            var response = new Response.OnlineUsersResponse
            {
                Users = onlineUsers
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting online users for user {UserId}", request.CurrentUserId);
            return Result.Failure<Response.OnlineUsersResponse>(Error.Failure(
                "Chat.GetOnlineUsers.Failed",
                "Failed to get online users"));
        }
    }
}