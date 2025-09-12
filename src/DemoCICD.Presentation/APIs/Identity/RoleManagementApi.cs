using Carter;
using DemoCICD.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DemoCICD.Presentation.APIs.Identity;

public class RoleManagementApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/carter/v{version:apiVersion}/roles";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Role Management")
            .MapGroup(BaseUrl).HasApiVersion(1);

        // Role CRUD operations
        group1.MapPost(string.Empty, CreateRoleV1).RequireAuthorization();
        group1.MapGet(string.Empty, GetRolesV1).RequireAuthorization();
        group1.MapGet("{roleId:guid}", GetRoleByIdV1).RequireAuthorization();
        group1.MapPut("{roleId:guid}", UpdateRoleV1).RequireAuthorization();
        group1.MapDelete("{roleId:guid}", DeleteRoleV1).RequireAuthorization();

        // Role user operations
        group1.MapGet("{roleId:guid}/users", GetUsersInRoleV1).RequireAuthorization();

        // Role permission operations
        group1.MapGet("{roleId:guid}/permissions", GetRolePermissionsV1).RequireAuthorization();
        group1.MapPost("{roleId:guid}/permissions", GrantPermissionToRoleV1).RequireAuthorization();
        group1.MapDelete("{roleId:guid}/permissions", RevokePermissionFromRoleV1).RequireAuthorization();
    }

    public static async Task<IResult> CreateRoleV1(ISender sender, [FromBody] DemoCICD.Contract.Services.V1.Identity.Command.CreateRole command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Created($"/api/carter/v1/roles/{result.Value.RoleId}", result);
    }

    public static async Task<IResult> GetRolesV1(ISender sender, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
    {
        var query = new DemoCICD.Contract.Services.V1.Identity.Query.GetRoles(page, pageSize, searchTerm);
        var result = await sender.Send(query);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> GetRoleByIdV1(ISender sender, [FromRoute] Guid roleId)
    {
        var query = new DemoCICD.Contract.Services.V1.Identity.Query.GetRoleById(roleId);
        var result = await sender.Send(query);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> UpdateRoleV1(ISender sender, [FromRoute] Guid roleId, [FromBody] DemoCICD.Contract.Services.V1.Identity.Command.UpdateRole command)
    {
        if (roleId != command.RoleId)
            return Results.BadRequest("Role ID mismatch");

        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> DeleteRoleV1(ISender sender, [FromRoute] Guid roleId)
    {
        var command = new DemoCICD.Contract.Services.V1.Identity.Command.DeleteRole(roleId);
        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.NoContent();
    }

    public static async Task<IResult> GetUsersInRoleV1(ISender sender, [FromRoute] Guid roleId)
    {
        var query = new DemoCICD.Contract.Services.V1.Identity.Query.GetUsersInRole(roleId);
        var result = await sender.Send(query);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> GetRolePermissionsV1(ISender sender, [FromRoute] Guid roleId)
    {
        var query = new DemoCICD.Contract.Services.V1.Identity.Query.GetRolePermissions(roleId);
        var result = await sender.Send(query);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> GrantPermissionToRoleV1(ISender sender, [FromRoute] Guid roleId, [FromBody] DemoCICD.Contract.Services.V1.Identity.Command.GrantPermissionToRole command)
    {
        if (roleId != command.RoleId)
            return Results.BadRequest("Role ID mismatch");

        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> RevokePermissionFromRoleV1(ISender sender, [FromRoute] Guid roleId, [FromBody] DemoCICD.Contract.Services.V1.Identity.Command.RevokePermissionFromRole command)
    {
        if (roleId != command.RoleId)
            return Results.BadRequest("Role ID mismatch");

        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}