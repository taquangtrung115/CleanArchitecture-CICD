using Carter;
using DemoCICD.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DemoCICD.Presentation.APIs.Identity;

public class UserManagementApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/carter/v{version:apiVersion}/users";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("User Management")
            .MapGroup(BaseUrl).HasApiVersion(1);

        // User CRUD operations
        group1.MapPost(string.Empty, CreateUserV1).RequireAuthorization();
        group1.MapGet(string.Empty, GetUsersV1).RequireAuthorization();
        group1.MapGet("{userId:guid}", GetUserByIdV1).RequireAuthorization();
        group1.MapPut("{userId:guid}", UpdateUserV1).RequireAuthorization();
        group1.MapDelete("{userId:guid}", DeleteUserV1).RequireAuthorization();

        // User management operations
        group1.MapPost("{userId:guid}/change-password", ChangePasswordV1).RequireAuthorization();
        group1.MapPost("{userId:guid}/reset-password", ResetPasswordV1).RequireAuthorization();
        group1.MapPost("{userId:guid}/lock", LockUserV1).RequireAuthorization();
        group1.MapPost("{userId:guid}/unlock", UnlockUserV1).RequireAuthorization();

        // User role operations
        group1.MapGet("{userId:guid}/roles", GetUserRolesV1).RequireAuthorization();
        group1.MapPost("{userId:guid}/roles/{roleId:guid}", AssignUserToRoleV1).RequireAuthorization();
        group1.MapDelete("{userId:guid}/roles/{roleId:guid}", RemoveUserFromRoleV1).RequireAuthorization();
    }

    public static async Task<IResult> CreateUserV1(ISender sender, [FromBody] DemoCICD.Contract.Services.V1.Identity.Command.CreateUser command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Created($"/api/carter/v1/users/{result.Value.UserId}", result);
    }

    public static async Task<IResult> GetUsersV1(ISender sender, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
    {
        var query = new DemoCICD.Contract.Services.V1.Identity.Query.GetUsers(page, pageSize, searchTerm);
        var result = await sender.Send(query);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> GetUserByIdV1(ISender sender, [FromRoute] Guid userId)
    {
        var query = new DemoCICD.Contract.Services.V1.Identity.Query.GetUserById(userId);
        var result = await sender.Send(query);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> UpdateUserV1(ISender sender, [FromRoute] Guid userId, [FromBody] DemoCICD.Contract.Services.V1.Identity.Command.UpdateUser command)
    {
        if (userId != command.UserId)
            return Results.BadRequest("User ID mismatch");

        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> DeleteUserV1(ISender sender, [FromRoute] Guid userId)
    {
        var command = new DemoCICD.Contract.Services.V1.Identity.Command.DeleteUser(userId);
        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.NoContent();
    }

    public static async Task<IResult> ChangePasswordV1(ISender sender, [FromRoute] Guid userId, [FromBody] DemoCICD.Contract.Services.V1.Identity.Command.ChangePassword command)
    {
        if (userId != command.UserId)
            return Results.BadRequest("User ID mismatch");

        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> ResetPasswordV1(ISender sender, [FromRoute] Guid userId, [FromBody] DemoCICD.Contract.Services.V1.Identity.Command.ResetPassword command)
    {
        if (userId != command.UserId)
            return Results.BadRequest("User ID mismatch");

        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> LockUserV1(ISender sender, [FromRoute] Guid userId)
    {
        var command = new DemoCICD.Contract.Services.V1.Identity.Command.LockUser(userId);
        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> UnlockUserV1(ISender sender, [FromRoute] Guid userId)
    {
        var command = new DemoCICD.Contract.Services.V1.Identity.Command.UnlockUser(userId);
        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> GetUserRolesV1(ISender sender, [FromRoute] Guid userId)
    {
        var query = new DemoCICD.Contract.Services.V1.Identity.Query.GetUserRoles(userId);
        var result = await sender.Send(query);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> AssignUserToRoleV1(ISender sender, [FromRoute] Guid userId, [FromRoute] Guid roleId)
    {
        var command = new DemoCICD.Contract.Services.V1.Identity.Command.AssignUserToRole(userId, roleId);
        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> RemoveUserFromRoleV1(ISender sender, [FromRoute] Guid userId, [FromRoute] Guid roleId)
    {
        var command = new DemoCICD.Contract.Services.V1.Identity.Command.RemoveUserFromRole(userId, roleId);
        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}