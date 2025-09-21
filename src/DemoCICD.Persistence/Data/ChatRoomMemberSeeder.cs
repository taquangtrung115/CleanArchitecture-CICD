using DemoCICD.Domain.Entities.Chat;
using DemoCICD.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Data;

public static class ChatRoomMemberSeeder
{
    public static async Task SeedChatRoomMembersAsync(ApplicationDbContext context)
    {
        if (await context.ChatRoomMembers.AnyAsync())
            return; // Data already exists

        var chatRooms = await context.ChatRooms.ToListAsync();
        var users = await context.Users.ToListAsync();

        if (!chatRooms.Any() || !users.Any()) return;

        var chatRoomMembers = new List<ChatRoomMember>();

        foreach (var room in chatRooms)
        {
            // Add room owner
            if (users.Any())
            {
                var owner = users.First();
                var ownerMember = new ChatRoomMember
                {
                    Id = Guid.NewGuid(),
                    RoomId = room.Id,
                    UserId = owner.Id,
                    Role = MemberRole.Owner,
                    JoinedDate = room.CreatedAt,
                    LastSeenDate = DateTime.UtcNow.AddMinutes(-30),
                    IsActive = true
                };
                ownerMember.SetCreatedAudit(owner.UserName ?? "System");
                chatRoomMembers.Add(ownerMember);

                // Add some regular members
                var otherUsers = users.Skip(1).Take(Math.Min(8, users.Count - 1));
                foreach (var user in otherUsers)
                {
                    var member = new ChatRoomMember
                    {
                        Id = Guid.NewGuid(),
                        RoomId = room.Id,
                        UserId = user.Id,
                        Role = MemberRole.Member,
                        JoinedDate = room.CreatedAt.AddDays(Random.Shared.Next(1, 30)),
                        LastSeenDate = DateTime.UtcNow.AddMinutes(-Random.Shared.Next(5, 300)),
                        IsActive = true
                    };
                    member.SetCreatedAudit(user.UserName ?? "System");
                    chatRoomMembers.Add(member);
                }

                // Make one user an admin
                if (users.Count > 2)
                {
                    var admin = users.Skip(1).First();
                    var adminMember = chatRoomMembers.FirstOrDefault(m => m.UserId == admin.Id && m.RoomId == room.Id);
                    if (adminMember != null)
                    {
                        adminMember.Role = MemberRole.Admin;
                    }
                }
            }
        }

        context.ChatRoomMembers.AddRange(chatRoomMembers.Take(50)); // Limit to prevent too many records
        await context.SaveChangesAsync();
    }
}