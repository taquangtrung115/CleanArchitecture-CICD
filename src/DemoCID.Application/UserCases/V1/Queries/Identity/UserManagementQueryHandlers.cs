using DemoCICD.Application.Abstractions;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DemoCICD.Application.UserCases.V1.Queries.Identity;

public sealed class GetUserByIdQueryHandler : IQueryHandler<Query.GetUserById, Response.UserDetails>
{
    private readonly IUserManagementService _userManagementService;

    public GetUserByIdQueryHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<Result<Response.UserDetails>> Handle(Query.GetUserById request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManagementService.GetUserByIdAsync(request.UserId);

            if (user == null)
            {
                return Result.Failure<Response.UserDetails>(
                    new Error("User.NotFound", "User not found"));
            }

            var response = new Response.UserDetails(
                user.Id,
                user.UserName!,
                user.Email!,
                user.FirstName,
                user.LastName,
                user.FullName,
                user.DayOfBirth,
                user.IsDirector,
                user.IsHeadOfDepartment,
                user.ManagerId,
                user.PositionId,
                user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow,
                DateTime.UtcNow, // This would come from a created timestamp in real implementation
                user.Phone,
                user.Address,
                user.City,
                user.Country,
                user.Bio,
                user.Website,
                user.Avatar);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting user by ID: {UserId}", request.UserId);
            return Result.Failure<Response.UserDetails>(
                new Error("User.GetError", "An error occurred while retrieving user"));
        }
    }
}

public sealed class GetUsersQueryHandler : IQueryHandler<Query.GetUsers, Response.UserList>
{
    private readonly IUserManagementService _userManagementService;

    public GetUsersQueryHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<Result<Response.UserList>> Handle(Query.GetUsers request, CancellationToken cancellationToken)
    {
        try
        {
            var (users, totalCount) = await _userManagementService.GetUsersAsync(
                request.Page,
                request.PageSize,
                request.SearchTerm);

            var userSummaries = users.Select(u => new Response.UserSummary(
                u.Id,
                u.UserName!,
                u.Email!,
                u.FullName,
                u.LockoutEnd.HasValue && u.LockoutEnd > DateTimeOffset.UtcNow));

            var response = new Response.UserList(
                userSummaries,
                totalCount,
                request.Page,
                request.PageSize);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting users with page: {Page}, pageSize: {PageSize}, searchTerm: {SearchTerm}", 
                request.Page, request.PageSize, request.SearchTerm);
            return Result.Failure<Response.UserList>(
                new Error("Users.GetError", "An error occurred while retrieving users"));
        }
    }
}

public sealed class GetUserRolesQueryHandler : IQueryHandler<Query.GetUserRoles, Response.UserRoleList>
{
    private readonly IUserManagementService _userManagementService;

    public GetUserRolesQueryHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<Result<Response.UserRoleList>> Handle(Query.GetUserRoles request, CancellationToken cancellationToken)
    {
        try
        {
            var roles = await _userManagementService.GetUserRolesAsync(request.UserId);

            var roleSummaries = roles.Select(r => new Response.RoleSummary(
                r.Id,
                r.Name!,
                r.RoleCode,
                r.Description));

            var response = new Response.UserRoleList(
                request.UserId,
                roleSummaries);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting user roles for user: {UserId}", request.UserId);
            return Result.Failure<Response.UserRoleList>(
                new Error("UserRoles.GetError", "An error occurred while retrieving user roles"));
        }
    }
}

public sealed class GetCurrentUserProfileQueryHandler : IQueryHandler<Query.GetCurrentUserProfile, Response.UserDetails>
{
    private readonly IUserManagementService _userManagementService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetCurrentUserProfileQueryHandler(
        IUserManagementService userManagementService,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManagementService = userManagementService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<Response.UserDetails>> Handle(Query.GetCurrentUserProfile request, CancellationToken cancellationToken)
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User?.Identity?.IsAuthenticated != true)
            {
                return Result.Failure<Response.UserDetails>(
                    new Error("Authentication.NotAuthenticated", "User is not authenticated"));
            }

            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Result.Failure<Response.UserDetails>(
                    new Error("Authentication.InvalidUserId", "Unable to retrieve user ID from token"));
            }

            var user = await _userManagementService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return Result.Failure<Response.UserDetails>(
                    new Error("User.NotFound", "User not found"));
            }

            var response = new Response.UserDetails(
                user.Id,
                user.UserName!,
                user.Email!,
                user.FirstName,
                user.LastName,
                user.FullName,
                user.DayOfBirth,
                user.IsDirector,
                user.IsHeadOfDepartment,
                user.ManagerId,
                user.PositionId,
                user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow,
                DateTime.UtcNow, // This would come from a created timestamp in real implementation
                user.Phone,
                user.Address,
                user.City,
                user.Country,
                user.Bio,
                user.Website,
                user.Avatar);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting current user profile");
            return Result.Failure<Response.UserDetails>(
                new Error("User.GetError", "An error occurred while retrieving current user profile"));
        }
    }
}

public sealed class GetNotificationSettingsQueryHandler : IQueryHandler<Query.GetNotificationSettings, Response.NotificationSettings>
{
    public async Task<Result<Response.NotificationSettings>> Handle(Query.GetNotificationSettings request, CancellationToken cancellationToken)
    {
        // Return default notification settings for now
        var response = new Response.NotificationSettings(
            EmailNotifications: true,
            PushNotifications: true,
            SmsNotifications: false,
            NewsUpdates: true,
            SecurityAlerts: true,
            MarketingEmails: false);

        return Result.Success(response);
    }
}

public sealed class GetPrivacySettingsQueryHandler : IQueryHandler<Query.GetPrivacySettings, Response.PrivacySettings>
{
    public async Task<Result<Response.PrivacySettings>> Handle(Query.GetPrivacySettings request, CancellationToken cancellationToken)
    {
        // Return default privacy settings for now
        var response = new Response.PrivacySettings(
            ProfileVisibility: "public",
            ShowEmail: false,
            ShowPhone: false,
            AllowSearchByEmail: true,
            AllowSearchByPhone: false);

        return Result.Success(response);
    }
}

public sealed class GetUserSessionsQueryHandler : IQueryHandler<Query.GetUserSessions, Response.UserSessionList>
{
    public async Task<Result<Response.UserSessionList>> Handle(Query.GetUserSessions request, CancellationToken cancellationToken)
    {
        // Return empty session list for now
        var response = new Response.UserSessionList(Enumerable.Empty<Response.UserSession>());
        return Result.Success(response);
    }
}
