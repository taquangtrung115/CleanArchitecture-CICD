using DemoCICD.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityAction = DemoCICD.Domain.Entities.Identity.Action;

namespace DemoCICD.Persistence.Data;

public static class IdentitySeeder
{
    public static async Task SeedIdentityDataAsync(ApplicationDbContext context)
    {
        await SeedActionsAsync(context);
        await SeedFunctionsAsync(context);
        await SeedActionInFunctionsAsync(context);
        await SeedRolesAsync(context);
        await SeedPermissionsAsync(context);
        await SeedPasswordResetTokensAsync(context);
        
        await context.SaveChangesAsync();
    }

    private static async Task SeedActionsAsync(ApplicationDbContext context)
    {
        if (await context.Actions.AnyAsync())
            return; // Data already exists

        var actions = new List<IdentityAction>
        {
            new IdentityAction { Id = "CREATE", Name = "Create", SortOrder = 1, IsActive = true },
            new IdentityAction { Id = "READ", Name = "Read", SortOrder = 2, IsActive = true },
            new IdentityAction { Id = "UPDATE", Name = "Update", SortOrder = 3, IsActive = true },
            new IdentityAction { Id = "DELETE", Name = "Delete", SortOrder = 4, IsActive = true },
            new IdentityAction { Id = "APPROVE", Name = "Approve", SortOrder = 5, IsActive = true },
            new IdentityAction { Id = "REJECT", Name = "Reject", SortOrder = 6, IsActive = true },
            new IdentityAction { Id = "EXPORT", Name = "Export", SortOrder = 7, IsActive = true },
            new IdentityAction { Id = "IMPORT", Name = "Import", SortOrder = 8, IsActive = true },
            new IdentityAction { Id = "MANAGE", Name = "Manage", SortOrder = 9, IsActive = true },
            new IdentityAction { Id = "VIEW_REPORT", Name = "View Report", SortOrder = 10, IsActive = true }
        };

        context.Actions.AddRange(actions);
    }

    private static async Task SeedFunctionsAsync(ApplicationDbContext context)
    {
        if (await context.Functions.AnyAsync())
            return; // Data already exists

        var functions = new List<Function>
        {
            new Function { Id = "SYSTEM", Name = "System Management", Url = "/system", ParrentId = "", SortOrder = 1, CssClass = "fa-cog", IsActive = true },
            new Function { Id = "USER", Name = "User Management", Url = "/users", ParrentId = "SYSTEM", SortOrder = 2, CssClass = "fa-user", IsActive = true },
            new Function { Id = "ROLE", Name = "Role Management", Url = "/roles", ParrentId = "SYSTEM", SortOrder = 3, CssClass = "fa-users", IsActive = true },
            new Function { Id = "MOTOGP", Name = "MotoGP Management", Url = "/motogp", ParrentId = "", SortOrder = 4, CssClass = "fa-motorcycle", IsActive = true },
            new Function { Id = "TEAMS", Name = "Teams", Url = "/motogp/teams", ParrentId = "MOTOGP", SortOrder = 5, CssClass = "fa-flag", IsActive = true },
            new Function { Id = "RIDERS", Name = "Riders", Url = "/motogp/riders", ParrentId = "MOTOGP", SortOrder = 6, CssClass = "fa-user-circle", IsActive = true },
            new Function { Id = "RACES", Name = "Races", Url = "/motogp/races", ParrentId = "MOTOGP", SortOrder = 7, CssClass = "fa-flag-checkered", IsActive = true },
            new Function { Id = "NEWS", Name = "News Management", Url = "/news", ParrentId = "", SortOrder = 8, CssClass = "fa-newspaper", IsActive = true },
            new Function { Id = "CHAT", Name = "Chat Management", Url = "/chat", ParrentId = "", SortOrder = 9, CssClass = "fa-comments", IsActive = true },
            new Function { Id = "PRODUCT", Name = "Product Management", Url = "/products", ParrentId = "", SortOrder = 10, CssClass = "fa-box", IsActive = true }
        };
        
        context.Functions.AddRange(functions);
    }

    private static async Task SeedActionInFunctionsAsync(ApplicationDbContext context)
    {
        if (await context.ActionInFunctions.AnyAsync())
            return; // Data already exists

        var actionInFunctions = new List<ActionInFunction>
        {
            // System functions
            new ActionInFunction { ActionId = "READ", FunctionId = "SYSTEM" },
            new ActionInFunction { ActionId = "MANAGE", FunctionId = "SYSTEM" },

            // User management
            new ActionInFunction { ActionId = "CREATE", FunctionId = "USER" },
            new ActionInFunction { ActionId = "READ", FunctionId = "USER" },
            new ActionInFunction { ActionId = "UPDATE", FunctionId = "USER" },
            new ActionInFunction { ActionId = "DELETE", FunctionId = "USER" },

            // Role management
            new ActionInFunction { ActionId = "CREATE", FunctionId = "ROLE" },
            new ActionInFunction { ActionId = "READ", FunctionId = "ROLE" },
            new ActionInFunction { ActionId = "UPDATE", FunctionId = "ROLE" },
            new ActionInFunction { ActionId = "DELETE", FunctionId = "ROLE" },

            // MotoGP management
            new ActionInFunction { ActionId = "READ", FunctionId = "MOTOGP" },
            new ActionInFunction { ActionId = "CREATE", FunctionId = "TEAMS" },
            new ActionInFunction { ActionId = "READ", FunctionId = "TEAMS" },
            new ActionInFunction { ActionId = "UPDATE", FunctionId = "TEAMS" },
            new ActionInFunction { ActionId = "DELETE", FunctionId = "TEAMS" },
            new ActionInFunction { ActionId = "CREATE", FunctionId = "RIDERS" },
            new ActionInFunction { ActionId = "READ", FunctionId = "RIDERS" },
            new ActionInFunction { ActionId = "UPDATE", FunctionId = "RIDERS" },
            new ActionInFunction { ActionId = "DELETE", FunctionId = "RIDERS" },
            new ActionInFunction { ActionId = "CREATE", FunctionId = "RACES" },
            new ActionInFunction { ActionId = "READ", FunctionId = "RACES" },
            new ActionInFunction { ActionId = "UPDATE", FunctionId = "RACES" },

            // News management
            new ActionInFunction { ActionId = "CREATE", FunctionId = "NEWS" },
            new ActionInFunction { ActionId = "READ", FunctionId = "NEWS" },
            new ActionInFunction { ActionId = "UPDATE", FunctionId = "NEWS" },
            new ActionInFunction { ActionId = "DELETE", FunctionId = "NEWS" },

            // Product management
            new ActionInFunction { ActionId = "CREATE", FunctionId = "PRODUCT" },
            new ActionInFunction { ActionId = "READ", FunctionId = "PRODUCT" },
            new ActionInFunction { ActionId = "UPDATE", FunctionId = "PRODUCT" },
            new ActionInFunction { ActionId = "DELETE", FunctionId = "PRODUCT" }
        };

        context.ActionInFunctions.AddRange(actionInFunctions);
    }

    private static async Task SeedRolesAsync(ApplicationDbContext context)
    {
        if (await context.Roles.AnyAsync())
            return; // Data already exists

        var roles = new List<AppRole>
        {
            new AppRole { Id = Guid.NewGuid(), Name = "Administrator", NormalizedName = "ADMINISTRATOR", Description = "Full system access", RoleCode = "ADMIN" },
            new AppRole { Id = Guid.NewGuid(), Name = "Manager", NormalizedName = "MANAGER", Description = "Management access", RoleCode = "MANAGER" },
            new AppRole { Id = Guid.NewGuid(), Name = "Editor", NormalizedName = "EDITOR", Description = "Content editing access", RoleCode = "EDITOR" },
            new AppRole { Id = Guid.NewGuid(), Name = "Viewer", NormalizedName = "VIEWER", Description = "Read-only access", RoleCode = "VIEWER" },
            new AppRole { Id = Guid.NewGuid(), Name = "News Editor", NormalizedName = "NEWS_EDITOR", Description = "News management access", RoleCode = "NEWS_EDITOR" },
            new AppRole { Id = Guid.NewGuid(), Name = "MotoGP Manager", NormalizedName = "MOTOGP_MANAGER", Description = "MotoGP content management", RoleCode = "MOTOGP_MANAGER" },
            new AppRole { Id = Guid.NewGuid(), Name = "Chat Moderator", NormalizedName = "CHAT_MODERATOR", Description = "Chat moderation access", RoleCode = "CHAT_MOD" },
            new AppRole { Id = Guid.NewGuid(), Name = "User", NormalizedName = "USER", Description = "Standard user access", RoleCode = "USER" }
        };

        context.Roles.AddRange(roles);
    }

    private static async Task SeedPermissionsAsync(ApplicationDbContext context)
    {
        if (await context.Permissions.AnyAsync())
            return; // Data already exists

        // Get the first admin role for permissions
        var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleCode == "ADMIN");
        if (adminRole == null) return;

        var permissions = new List<Permission>
        {
            new Permission { RoleId = adminRole.Id, FunctionId = "SYSTEM", ActionId = "MANAGE" },
            new Permission { RoleId = adminRole.Id, FunctionId = "USER", ActionId = "CREATE" },
            new Permission { RoleId = adminRole.Id, FunctionId = "USER", ActionId = "READ" },
            new Permission { RoleId = adminRole.Id, FunctionId = "USER", ActionId = "UPDATE" },
            new Permission { RoleId = adminRole.Id, FunctionId = "USER", ActionId = "DELETE" },
            new Permission { RoleId = adminRole.Id, FunctionId = "ROLE", ActionId = "CREATE" },
            new Permission { RoleId = adminRole.Id, FunctionId = "ROLE", ActionId = "READ" },
            new Permission { RoleId = adminRole.Id, FunctionId = "ROLE", ActionId = "UPDATE" },
            new Permission { RoleId = adminRole.Id, FunctionId = "ROLE", ActionId = "DELETE" },
            new Permission { RoleId = adminRole.Id, FunctionId = "MOTOGP", ActionId = "READ" }
        };

        context.Permissions.AddRange(permissions);
    }

    private static async Task SeedPasswordResetTokensAsync(ApplicationDbContext context)
    {
        if (await context.PasswordResetTokens.AnyAsync())
            return; // Data already exists

        // Create some expired/used tokens for demo purposes
        var tokens = new List<PasswordResetToken>
        {
            new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                Email = "admin@demo.com",
                ResetCode = "ABC123",
                ExpiresAt = DateTime.UtcNow.AddDays(-1),
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                IsUsed = true,
                UsedAt = DateTime.UtcNow.AddDays(-1)
            },
            new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                Email = "user@demo.com",
                ResetCode = "DEF456",
                ExpiresAt = DateTime.UtcNow.AddDays(-3),
                CreatedAt = DateTime.UtcNow.AddDays(-4),
                IsUsed = false
            },
            new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                Email = "manager@demo.com",
                ResetCode = "GHI789",
                ExpiresAt = DateTime.UtcNow.AddDays(-5),
                CreatedAt = DateTime.UtcNow.AddDays(-6),
                IsUsed = true,
                UsedAt = DateTime.UtcNow.AddDays(-5)
            },
            new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                Email = "editor@demo.com",
                ResetCode = "JKL012",
                ExpiresAt = DateTime.UtcNow.AddDays(-7),
                CreatedAt = DateTime.UtcNow.AddDays(-8),
                IsUsed = false
            },
            new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                Email = "viewer@demo.com",
                ResetCode = "MNO345",
                ExpiresAt = DateTime.UtcNow.AddDays(-2),
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                IsUsed = true,
                UsedAt = DateTime.UtcNow.AddDays(-2)
            },
            new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                Email = "test@demo.com",
                ResetCode = "PQR678",
                ExpiresAt = DateTime.UtcNow.AddDays(-10),
                CreatedAt = DateTime.UtcNow.AddDays(-11),
                IsUsed = false
            },
            new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                Email = "demo@demo.com",
                ResetCode = "STU901",
                ExpiresAt = DateTime.UtcNow.AddDays(-4),
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                IsUsed = true,
                UsedAt = DateTime.UtcNow.AddDays(-4)
            },
            new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                Email = "sample@demo.com",
                ResetCode = "VWX234",
                ExpiresAt = DateTime.UtcNow.AddDays(-6),
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                IsUsed = false
            }
        };

        context.PasswordResetTokens.AddRange(tokens);
    }
}
