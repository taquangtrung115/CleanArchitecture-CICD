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
/// Carter Module cho qu?n lý phòng tr?
/// </summary>
public class RoomManagementApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/rental-rooms";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.NewVersionedApi("rental-rooms")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        // Commands
        group.MapPost(string.Empty, CreateRoom)
            .WithName("CreateRoom")
            .WithSummary("T?o phòng tr? m?i")
            .Produces<Result<Guid>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPut("{roomId:guid}", UpdateRoom)
            .WithName("UpdateRoom")
            .WithSummary("C?p nh?t thông tin phòng");

        group.MapDelete("{roomId:guid}", DeleteRoom)
            .WithName("DeleteRoom")
            .WithSummary("Xóa phòng");

        group.MapPatch("{roomId:guid}/availability", UpdateRoomAvailability)
            .WithName("UpdateRoomAvailability")
            .WithSummary("C?p nh?t tr?ng thái phòng");

        // Queries
        group.MapGet(string.Empty, GetRooms)
            .WithName("GetRooms")
            .WithSummary("L?y danh sách phòng có phân trang");

        group.MapGet("{roomId:guid}", GetRoomById)
            .WithName("GetRoomById")
            .WithSummary("L?y thông tin phòng theo ID");

        group.MapGet("location/{locationId:guid}", GetRoomsByLocation)
            .WithName("GetRoomsByLocation")
            .WithSummary("L?y danh sách phòng theo ??a ch?");
    }

    #region Commands

    private static async Task<IResult> CreateRoom(
        ISender sender,
        [FromBody] RoomCommand.CreateRoomCommand command)
    {
        var result = await sender.Send(command);
        return result.IsSuccess
            ? Results.Ok(result)
            : HandlerFailure(result);
    }

    private static async Task<IResult> UpdateRoom(
        ISender sender,
        Guid roomId,
        [FromBody] RoomCommand.UpdateRoomCommand command)
    {
        var updateCommand = new RoomCommand.UpdateRoomCommand(
            roomId,
            command.RoomNumber,
            command.Capacity,
            command.PricePerNight,
            command.Description,
            command.LocationId);

        var result = await sender.Send(updateCommand);
        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteRoom(ISender sender, Guid roomId)
    {
        var result = await sender.Send(new RoomCommand.DeleteRoomCommand(roomId));
        return Results.Ok(result);
    }

    private static async Task<IResult> UpdateRoomAvailability(
        ISender sender,
        Guid roomId,
        [FromBody] UpdateAvailabilityRequest request)
    {
        var result = await sender.Send(new RoomCommand.UpdateRoomAvailabilityCommand(roomId, request.IsAvailable));
        return Results.Ok(result);
    }

    #endregion

    #region Queries

    private static async Task<IResult> GetRooms(
        ISender sender,
        string? searchTerm = null,
        bool? isAvailable = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int? minCapacity = null,
        string? sortColumn = null,
        string? sortOrder = null,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var query = new RoomQuery.GetRoomsQuery(
            searchTerm,
            isAvailable,
            minPrice,
            maxPrice,
            minCapacity,
            sortColumn,
            SortOrderExtension.ConvertStringToSortOrder(sortOrder),
            pageIndex,
            pageSize);

        var result = await sender.Send(query);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetRoomById(ISender sender, Guid roomId)
    {
        var result = await sender.Send(new RoomQuery.GetRoomByIdQuery(roomId));
        return Results.Ok(result);
    }

    private static async Task<IResult> GetRoomsByLocation(ISender sender, Guid locationId)
    {
        var result = await sender.Send(new RoomQuery.GetRoomsByLocationQuery(locationId));
        return Results.Ok(result);
    }

    #endregion

    #region Helper Records

    public record UpdateAvailabilityRequest(bool IsAvailable);

    #endregion
}
