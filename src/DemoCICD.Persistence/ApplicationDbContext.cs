using DemoCICD.Domain.Entities;
using DemoCICD.Domain.Entities.Identity;
using DemoCICD.Domain.Entities.MotoGP.RaceManagement;
using DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;
using DemoCICD.Domain.Entities.MotoGP.MediaNews;
using DemoCICD.Domain.Entities.Chat;
using DemoCICD.Domain.Abstractions.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Action = DemoCICD.Domain.Entities.Identity.Action;
using Function = DemoCICD.Domain.Entities.Identity.Function;

namespace DemoCICD.Persistence;

public sealed class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
        
        // Configure soft delete filter for all AuditableEntity types
        ConfigureSoftDeleteFilter(builder);
        
        base.OnModelCreating(builder);
    }

    private void ConfigureSoftDeleteFilter(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (IsAuditableEntity(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(AuditableEntity<object>.IsDeleted));
                var condition = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(condition, parameter);
                
                builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }

    private static bool IsAuditableEntity(Type type)
    {
        while (type != null)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(AuditableEntity<>))
                return true;
            type = type.BaseType;
        }
        return false;
    }

    public DbSet<AppUser> AppUses { get; set; }
    public DbSet<Action> Actions { get; set; }
    public DbSet<Function> Functions { get; set; }
    public DbSet<ActionInFunction> ActionInFunctions { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
    public DbSet<Product> Products { get; set; }

    // MotoGP Entities
    public DbSet<Season> Seasons { get; set; }
    public DbSet<Race> Races { get; set; }
    public DbSet<RaceEntry> RaceEntries { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Rider> Riders { get; set; }
    public DbSet<RiderTeamHistory> RiderTeamHistories { get; set; }
    public DbSet<Bike> Bikes { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<Video> Videos { get; set; }

    // Chat Entities
    public DbSet<ChatRoom> ChatRooms { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<ChatRoomMember> ChatRoomMembers { get; set; }
}
