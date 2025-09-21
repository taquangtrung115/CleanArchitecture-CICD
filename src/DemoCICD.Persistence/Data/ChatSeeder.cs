using DemoCICD.Domain.Entities.Chat;
using DemoCICD.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Data;

public static class ChatSeeder
{
    public static async Task SeedChatDataAsync(ApplicationDbContext context)
    {
        // Check if chat data already exists
        if (await context.ChatRooms.AnyAsync() || await context.ChatMessages.AnyAsync())
            return; // Data already exists

        // Get some existing users for seeding
        var users = await context.Users.Take(5).ToListAsync();
        if (users.Count < 2)
        {
            // Create some sample users if none exist
            users = await CreateSampleUsersAsync(context);
        }

        // Create sample chat rooms
        var rooms = await CreateSampleRoomsAsync(context, users);
        
        // Create sample chat messages
        await CreateSampleMessagesAsync(context, users, rooms);

        await context.SaveChangesAsync();
    }

    private static async Task<List<AppUser>> CreateSampleUsersAsync(ApplicationDbContext context)
    {
        var users = new List<AppUser>
        {
            new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = "alice.johnson",
                Email = "alice.johnson@demo.com",
                FirstName = "Alice",
                LastName = "Johnson",
                EmailConfirmed = true
            },
            new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = "bob.smith",
                Email = "bob.smith@demo.com",
                FirstName = "Bob",
                LastName = "Smith",
                EmailConfirmed = true
            },
            new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = "charlie.brown",
                Email = "charlie.brown@demo.com",
                FirstName = "Charlie",
                LastName = "Brown",
                EmailConfirmed = true
            },
            new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = "diana.wilson",
                Email = "diana.wilson@demo.com",
                FirstName = "Diana",
                LastName = "Wilson",
                EmailConfirmed = true
            },
            new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = "eve.davis",
                Email = "eve.davis@demo.com",
                FirstName = "Eve",
                LastName = "Davis",
                EmailConfirmed = true
            }
        };

        context.Users.AddRange(users);
        await context.SaveChangesAsync();
        return users;
    }

    private static async Task<List<ChatRoom>> CreateSampleRoomsAsync(ApplicationDbContext context, List<AppUser> users)
    {
        var rooms = new List<ChatRoom>();

        // Create a development team room
        var devTeamRoom = new ChatRoom
        {
            Id = Guid.NewGuid(),
            Name = "Development Team",
            Description = "Team discussions and updates",
            Type = RoomType.Group,
            IsActive = true
        };
        devTeamRoom.SetCreatedAudit("System");
        rooms.Add(devTeamRoom);

        // Create a project alpha room
        var projectRoom = new ChatRoom
        {
            Id = Guid.NewGuid(),
            Name = "Project Alpha",
            Description = "Project Alpha coordination",
            Type = RoomType.Group,
            IsActive = true
        };
        projectRoom.SetCreatedAudit("System");
        rooms.Add(projectRoom);

        // Create a general chat room
        var generalRoom = new ChatRoom
        {
            Id = Guid.NewGuid(),
            Name = "General Chat",
            Description = "General discussions",
            Type = RoomType.Public,
            IsActive = true
        };
        generalRoom.SetCreatedAudit("System");
        rooms.Add(generalRoom);

        context.ChatRooms.AddRange(rooms);
        await context.SaveChangesAsync();

        // Add members to rooms
        var members = new List<ChatRoomMember>();

        // Add all users to development team room
        foreach (var user in users)
        {
            var member = new ChatRoomMember
            {
                Id = Guid.NewGuid(),
                RoomId = devTeamRoom.Id,
                UserId = user.Id,
                Role = user == users.First() ? MemberRole.Admin : MemberRole.Member,
                JoinedDate = DateTime.UtcNow.AddDays(-30),
                IsActive = true
            };
            member.SetCreatedAudit("System");
            members.Add(member);
        }

        // Add some users to project room
        for (int i = 0; i < 3 && i < users.Count; i++)
        {
            var member = new ChatRoomMember
            {
                Id = Guid.NewGuid(),
                RoomId = projectRoom.Id,
                UserId = users[i].Id,
                Role = i == 0 ? MemberRole.Owner : MemberRole.Member,
                JoinedDate = DateTime.UtcNow.AddDays(-15),
                IsActive = true
            };
            member.SetCreatedAudit("System");
            members.Add(member);
        }

        // Add all users to general room
        foreach (var user in users)
        {
            var member = new ChatRoomMember
            {
                Id = Guid.NewGuid(),
                RoomId = generalRoom.Id,
                UserId = user.Id,
                Role = MemberRole.Member,
                JoinedDate = DateTime.UtcNow.AddDays(-20),
                IsActive = true
            };
            member.SetCreatedAudit("System");
            members.Add(member);
        }

        context.ChatRoomMembers.AddRange(members);
        await context.SaveChangesAsync();

        return rooms;
    }

    private static async Task CreateSampleMessagesAsync(ApplicationDbContext context, List<AppUser> users, List<ChatRoom> rooms)
    {
        var messages = new List<ChatMessage>();
        var random = new Random();

        // Sample messages for development team room
        var devRoom = rooms.FirstOrDefault(r => r.Name == "Development Team");
        if (devRoom != null)
        {
            var devMessages = new[]
            {
                "Good morning team! Ready for today's sprint planning?",
                "Yes! I've finished the authentication module. Ready for code review.",
                "Great work! I'll review it this afternoon. Also, did everyone see the new requirements?",
                "I saw them. The chat feature looks interesting to implement.",
                "Let's discuss the database design for the chat system.",
                "I think we should use a message table with references to rooms and users.",
                "Agreed. We also need to consider real-time notifications.",
                "SignalR would be perfect for that!",
                "Let's schedule a technical discussion for tomorrow.",
                "Sounds good! I'll set up the meeting."
            };

            for (int i = 0; i < devMessages.Length; i++)
            {
                var user = users[i % users.Count];
                var message = new ChatMessage
                {
                    Id = Guid.NewGuid(),
                    SenderId = user.Id,
                    RoomId = devRoom.Id,
                    Content = devMessages[i],
                    Type = MessageType.Text,
                    IsRead = i < devMessages.Length - 2 // Mark most as read, leave recent ones unread
                };
                message.SetCreatedAudit(user.UserName ?? "System");
                
                // Stagger message times
                var createdTime = DateTime.UtcNow.AddHours(-48).AddMinutes(i * 20);
                typeof(ChatMessage).GetProperty("CreatedAt")?.SetValue(message, createdTime);
                
                messages.Add(message);
            }
        }

        // Sample messages for project room
        var projectRoom = rooms.FirstOrDefault(r => r.Name == "Project Alpha");
        if (projectRoom != null)
        {
            var projectMessages = new[]
            {
                "Project Alpha kickoff meeting scheduled for next Monday.",
                "I've created the initial project structure in the repository.",
                "Thanks! I'll start working on the API design.",
                "Don't forget we need to finalize the database schema first.",
                "Good point. Let me share the ER diagram.",
                "The schema looks good. When can we start implementation?",
                "I think we can start by end of this week.",
                "Perfect! I'll prepare the development environment.",
                "Remember to follow the coding standards we discussed.",
                "Absolutely! Code quality is our top priority."
            };

            for (int i = 0; i < projectMessages.Length && i < 3; i++) // Only first 3 users are in this room
            {
                var user = users[i % 3];
                var message = new ChatMessage
                {
                    Id = Guid.NewGuid(),
                    SenderId = user.Id,
                    RoomId = projectRoom.Id,
                    Content = projectMessages[i],
                    Type = MessageType.Text,
                    IsRead = i < projectMessages.Length - 3
                };
                message.SetCreatedAudit(user.UserName ?? "System");
                
                var createdTime = DateTime.UtcNow.AddHours(-24).AddMinutes(i * 30);
                typeof(ChatMessage).GetProperty("CreatedAt")?.SetValue(message, createdTime);
                
                messages.Add(message);
            }
        }

        // Sample direct messages between users
        if (users.Count >= 2)
        {
            var directMessages = new[]
            {
                "Hey! How's the new feature coming along?",
                "Going well! Should be ready for testing by tomorrow.",
                "That's great to hear. Let me know if you need any help.",
                "Will do! Thanks for offering.",
                "See you at the team meeting later?",
                "Yes, I'll be there. Looking forward to the demo!"
            };

            for (int i = 0; i < directMessages.Length; i++)
            {
                var sender = users[i % 2];
                var receiver = users[(i + 1) % 2];
                
                var message = new ChatMessage
                {
                    Id = Guid.NewGuid(),
                    SenderId = sender.Id,
                    ReceiverId = receiver.Id,
                    Content = directMessages[i],
                    Type = MessageType.Text,
                    IsRead = i < directMessages.Length - 1
                };
                message.SetCreatedAudit(sender.UserName ?? "System");
                
                var createdTime = DateTime.UtcNow.AddHours(-12).AddMinutes(i * 15);
                typeof(ChatMessage).GetProperty("CreatedAt")?.SetValue(message, createdTime);
                
                messages.Add(message);
            }
        }

        // Sample messages for general room
        var generalRoom = rooms.FirstOrDefault(r => r.Name == "General Chat");
        if (generalRoom != null)
        {
            var generalMessages = new[]
            {
                "Welcome to the general chat room!",
                "Thanks! Excited to be part of the team.",
                "Does anyone know when the next company meeting is?",
                "I think it's scheduled for next Friday.",
                "Great! Don't forget about the team lunch tomorrow.",
                "Looking forward to it!"
            };

            for (int i = 0; i < generalMessages.Length; i++)
            {
                var user = users[i % users.Count];
                var message = new ChatMessage
                {
                    Id = Guid.NewGuid(),
                    SenderId = user.Id,
                    RoomId = generalRoom.Id,
                    Content = generalMessages[i],
                    Type = MessageType.Text,
                    IsRead = true
                };
                message.SetCreatedAudit(user.UserName ?? "System");
                
                var createdTime = DateTime.UtcNow.AddHours(-6).AddMinutes(i * 10);
                typeof(ChatMessage).GetProperty("CreatedAt")?.SetValue(message, createdTime);
                
                messages.Add(message);
            }
        }

        context.ChatMessages.AddRange(messages);
        await context.SaveChangesAsync();
    }
}