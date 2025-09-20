using DemoCICD.Application.Abstractions;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Identity;
using Microsoft.Extensions.Logging;
using Serilog;

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
                DateTime.UtcNow); // This would come from a created timestamp in real implementation

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

public sealed class GetCurrentUserProfileQueryHandler : IQueryHandler<Query.GetCurrentUserProfile, Response.UserProfile>
{
    private readonly IUserManagementService _userManagementService;

    public GetCurrentUserProfileQueryHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<Result<Response.UserProfile>> Handle(Query.GetCurrentUserProfile request, CancellationToken cancellationToken)
    {
        try
        {
            // Note: The actual user ID will be injected by the API endpoint from the JWT claims
            // For now, this handler expects the user ID to be available through some mechanism
            // This will be resolved when we add the endpoint
            throw new NotImplementedException("This handler requires user context to be provided by the endpoint");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting current user profile");
            return Result.Failure<Response.UserProfile>(
                new Error("Profile.GetError", "An error occurred while retrieving user profile"));
        }
    }
}
