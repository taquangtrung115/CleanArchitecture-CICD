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
/// Carter Module cho qu?n lý ng??i thuê phòng
/// </summary>
public class ProfileManagementApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/rental-profiles";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.NewVersionedApi("rental-profiles")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        // Commands
        group.MapPost(string.Empty, CreateProfile)
            .WithName("CreateProfile")
            .WithSummary("T?o profile ng??i thuê m?i");

        group.MapPut("{profileId:guid}", UpdateProfile)
            .WithName("UpdateProfile")
            .WithSummary("C?p nh?t thông tin ng??i thuê");

        group.MapDelete("{profileId:guid}", DeleteProfile)
            .WithName("DeleteProfile")
            .WithSummary("Xóa profile ng??i thuê");

        group.MapPost("{profileId:guid}/assign-room", AssignRoom)
            .WithName("AssignRoomToProfile")
            .WithSummary("Gán phòng cho ng??i thuê");

        group.MapPost("{profileId:guid}/end-rental", EndRental)
            .WithName("EndRental")
            .WithSummary("K?t thúc h?p ??ng thuê");

        // Queries
        group.MapGet(string.Empty, GetProfiles)
            .WithName("GetProfiles")
            .WithSummary("L?y danh sách ng??i thuê có phân trang");

        group.MapGet("{profileId:guid}", GetProfileById)
            .WithName("GetProfileById")
            .WithSummary("L?y thông tin ng??i thuê theo ID");

        group.MapGet("room/{roomId:guid}", GetProfilesByRoom)
            .WithName("GetProfilesByRoom")
            .WithSummary("L?y danh sách ng??i thuê theo phòng");
    }

    #region Commands

    private static async Task<IResult> CreateProfile(
        ISender sender,
        [FromBody] ProfileCommand.CreateProfileCommand command)
    {
        var result = await sender.Send(command);
        return result.IsSuccess
            ? Results.Ok(result)
            : HandlerFailure(result);
    }

    private static async Task<IResult> UpdateProfile(
        ISender sender,
        Guid profileId,
        [FromBody] ProfileCommand.UpdateProfileCommand command)
    {
        var updateCommand = new ProfileCommand.UpdateProfileCommand(
            profileId,
            command.FullName,
            command.PhoneNumber,
            command.IdentityCard,
            command.Email,
            command.DateOfBirth,
            command.PermanentAddress,
            command.Occupation);

        var result = await sender.Send(updateCommand);
        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteProfile(ISender sender, Guid profileId)
    {
        var result = await sender.Send(new ProfileCommand.DeleteProfileCommand(profileId));
        return Results.Ok(result);
    }

    private static async Task<IResult> AssignRoom(
        ISender sender,
        Guid profileId,
        [FromBody] AssignRoomRequest request)
    {
        var assignCommand = new ProfileCommand.AssignRoomToProfileCommand(
            profileId,
            request.RoomId,
            request.RentStartDate,
            request.DepositAmount);

        var result = await sender.Send(assignCommand);
        return Results.Ok(result);
    }

    private static async Task<IResult> EndRental(
        ISender sender,
        Guid profileId,
        [FromBody] EndRentalRequest request)
    {
        var result = await sender.Send(new ProfileCommand.EndRentalCommand(profileId, request.RentEndDate));
        return Results.Ok(result);
    }

    #endregion

    #region Queries

    private static async Task<IResult> GetProfiles(
        ISender sender,
        string? searchTerm = null,
        bool? isActive = null,
        Guid? roomId = null,
        string? sortColumn = null,
        string? sortOrder = null,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var query = new ProfileQuery.GetProfilesQuery(
            searchTerm,
            isActive,
            roomId,
            sortColumn,
            SortOrderExtension.ConvertStringToSortOrder(sortOrder),
            pageIndex,
            pageSize);

        var result = await sender.Send(query);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetProfileById(ISender sender, Guid profileId)
    {
        var result = await sender.Send(new ProfileQuery.GetProfileByIdQuery(profileId));
        return Results.Ok(result);
    }

    private static async Task<IResult> GetProfilesByRoom(ISender sender, Guid roomId)
    {
        var result = await sender.Send(new ProfileQuery.GetProfilesByRoomQuery(roomId));
        return Results.Ok(result);
    }

    #endregion

    #region Helper Records

    private record AssignRoomRequest(Guid RoomId, DateTime RentStartDate, decimal DepositAmount);
    private record EndRentalRequest(DateTime RentEndDate);

    #endregion
}
