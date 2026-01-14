using Carter;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Extensions;
using DemoCICD.Contract.Services.V1.RentalRoom;
using DemoCICD.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DemoCICD.Presentation.APIs.RentalRoom;

/// <summary>
/// Carter Module cho qu?n lý ??a ch? phòng tr?
/// </summary>
public class LocationManagementApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/rental-locations";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.NewVersionedApi("rental-locations")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        // Commands
        group.MapPost(string.Empty, CreateLocation)
            .WithName("CreateLocation")
            .WithSummary("T?o ??a ch? m?i");

        group.MapPut("{locationId:guid}", UpdateLocation)
            .WithName("UpdateLocation")
            .WithSummary("C?p nh?t ??a ch?");

        group.MapDelete("{locationId:guid}", DeleteLocation)
            .WithName("DeleteLocation")
            .WithSummary("Xóa ??a ch?");

        group.MapPatch("{locationId:guid}/coordinates", UpdateCoordinates)
            .WithName("UpdateLocationCoordinates")
            .WithSummary("C?p nh?t t?a ?? GPS");

        // Queries
        group.MapGet(string.Empty, GetLocations)
            .WithName("GetLocations")
            .WithSummary("L?y danh sách ??a ch? có phân trang");

        group.MapGet("{locationId:guid}", GetLocationById)
            .WithName("GetLocationById")
            .WithSummary("L?y thông tin ??a ch? theo ID");
    }

    #region Commands

    private static async Task<IResult> CreateLocation(
        ISender sender,
        [FromBody] LocationCommand.CreateLocationCommand command)
    {
        var result = await sender.Send(command);
        return result.IsSuccess
            ? Results.Ok(result)
            : HandlerFailure(result);
    }

    private static async Task<IResult> UpdateLocation(
        ISender sender,
        Guid locationId,
        [FromBody] LocationCommand.UpdateLocationCommand command)
    {
        var updateCommand = new LocationCommand.UpdateLocationCommand(
            locationId,
            command.Street,
            command.Ward,
            command.District,
            command.City,
            command.PostalCode,
            command.Latitude,
            command.Longitude,
            command.Notes);

        var result = await sender.Send(updateCommand);
        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteLocation(ISender sender, Guid locationId)
    {
        var result = await sender.Send(new LocationCommand.DeleteLocationCommand(locationId));
        return Results.Ok(result);
    }

    private static async Task<IResult> UpdateCoordinates(
        ISender sender,
        Guid locationId,
        [FromBody] UpdateCoordinatesRequest request)
    {
        var updateCommand = new LocationCommand.UpdateLocationCoordinatesCommand(
            locationId,
            request.Latitude,
            request.Longitude);

        var result = await sender.Send(updateCommand);
        return Results.Ok(result);
    }

    #endregion

    #region Queries

    private static async Task<IResult> GetLocations(
        ISender sender,
        string? searchTerm = null,
        string? city = null,
        string? district = null,
        string? sortColumn = null,
        string? sortOrder = null,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var query = new LocationQuery.GetLocationsQuery(
            searchTerm,
            city,
            district,
            sortColumn,
            SortOrderExtension.ConvertStringToSortOrder(sortOrder),
            pageIndex,
            pageSize);

        var result = await sender.Send(query);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetLocationById(ISender sender, Guid locationId)
    {
        var result = await sender.Send(new LocationQuery.GetLocationByIdQuery(locationId));
        return Results.Ok(result);
    }

    #endregion

    #region Helper Records

    public record UpdateCoordinatesRequest(double Latitude, double Longitude);

    #endregion
}
