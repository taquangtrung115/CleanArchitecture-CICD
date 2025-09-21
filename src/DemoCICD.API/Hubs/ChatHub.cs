using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DemoCICD.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private static readonly Dictionary<string, string> UserConnections = new();
    private static readonly Dictionary<string, List<string>> UserGroups = new();

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
        {
            UserConnections[Context.ConnectionId] = userId;
            
            // Join user to their personal group for direct messages
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
            
            // Notify others that user is online
            await Clients.Others.SendAsync("UserOnline", userId);
        }
        
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = UserConnections.GetValueOrDefault(Context.ConnectionId);
        if (userId != null)
        {
            UserConnections.Remove(Context.ConnectionId);
            
            // Remove from groups
            if (UserGroups.ContainsKey(userId))
            {
                foreach (var groupName in UserGroups[userId])
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
                }
                UserGroups.Remove(userId);
            }
            
            // Notify others that user is offline
            await Clients.Others.SendAsync("UserOffline", userId);
        }
        
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessageToUser(string receiverId, string message)
    {
        var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (senderId == null) return;

        var messageData = new
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Message = message,
            Timestamp = DateTime.UtcNow
        };

        // Send to receiver
        await Clients.Group($"user_{receiverId}")
            .SendAsync("ReceiveMessage", messageData);

        // Send back to sender for confirmation
        await Clients.Caller.SendAsync("MessageSent", messageData);
    }

    public async Task JoinChatRoom(string roomId)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return;

        await Groups.AddToGroupAsync(Context.ConnectionId, $"room_{roomId}");
        
        if (!UserGroups.ContainsKey(userId))
            UserGroups[userId] = new List<string>();
        
        UserGroups[userId].Add($"room_{roomId}");
        
        await Clients.Group($"room_{roomId}")
            .SendAsync("UserJoinedRoom", userId, roomId);
    }

    public async Task LeaveChatRoom(string roomId)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return;

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"room_{roomId}");
        
        if (UserGroups.ContainsKey(userId))
        {
            UserGroups[userId].Remove($"room_{roomId}");
        }
        
        await Clients.Group($"room_{roomId}")
            .SendAsync("UserLeftRoom", userId, roomId);
    }

    public async Task SendMessageToRoom(string roomId, string message)
    {
        var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (senderId == null) return;

        var messageData = new
        {
            SenderId = senderId,
            RoomId = roomId,
            Message = message,
            Timestamp = DateTime.UtcNow
        };

        await Clients.Group($"room_{roomId}")
            .SendAsync("ReceiveRoomMessage", messageData);
    }

    public async Task GetOnlineUsers()
    {
        var onlineUsers = UserConnections.Values.Distinct().ToList();
        await Clients.Caller.SendAsync("OnlineUsers", onlineUsers);
    }
}