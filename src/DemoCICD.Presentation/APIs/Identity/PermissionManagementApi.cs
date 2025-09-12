using Carter;
using DemoCICD.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DemoCICD.Presentation.APIs.Identity;

public class PermissionManagementApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/carter/v{version:apiVersion}/permissions";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Permission Management")
            .MapGroup(BaseUrl).HasApiVersion(1);

        // Permission CRUD operations
        group1.MapPost(string.Empty, CreatePermissionV1).RequireAuthorization();
        group1.MapGet(string.Empty, GetPermissionsV1).RequireAuthorization();
        group1.MapGet("{roleId:guid}/{functionId}/{actionId}", GetPermissionByIdV1).RequireAuthorization();
        group1.MapDelete("{roleId:guid}/{functionId}/{actionId}", DeletePermissionV1).RequireAuthorization();
    }

    public static async Task<IResult> CreatePermissionV1(ISender sender, [FromBody] DemoCICD.Contract.Services.V1.Identity.Command.CreatePermission command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Created($"/api/carter/v1/permissions/{result.Value.RoleId}/{result.Value.FunctionId}/{result.Value.ActionId}", result);
    }

    public static async Task<IResult> GetPermissionsV1(ISender sender, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = new DemoCICD.Contract.Services.V1.Identity.Query.GetPermissions(page, pageSize);
        var result = await sender.Send(query);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> GetPermissionByIdV1(ISender sender, [FromRoute] Guid roleId, [FromRoute] string functionId, [FromRoute] string actionId)
    {
        var query = new DemoCICD.Contract.Services.V1.Identity.Query.GetPermissionById(roleId, functionId, actionId);
        var result = await sender.Send(query);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> DeletePermissionV1(ISender sender, [FromRoute] Guid roleId, [FromRoute] string functionId, [FromRoute] string actionId)
    {
        var command = new DemoCICD.Contract.Services.V1.Identity.Command.DeletePermission(roleId, functionId, actionId);
        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.NoContent();
    }
}