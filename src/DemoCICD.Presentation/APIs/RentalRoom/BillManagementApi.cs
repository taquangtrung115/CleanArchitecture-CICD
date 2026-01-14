using Carter;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Enumerations;
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
/// Carter Module cho qu?n lý hóa ??n phòng tr?
/// </summary>
public class BillManagementApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/rental-bills";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.NewVersionedApi("rental-bills")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        // Bill Commands
        group.MapPost(string.Empty, CreateBill)
            .WithName("CreateBill")
            .WithSummary("T?o hóa ??n m?i");

        group.MapPut("{billId:guid}", UpdateBill)
            .WithName("UpdateBill")
            .WithSummary("C?p nh?t hóa ??n");

        group.MapDelete("{billId:guid}", DeleteBill)
            .WithName("DeleteBill")
            .WithSummary("Xóa hóa ??n");

        // Bill Detail Commands
        group.MapPost("{billId:guid}/details", AddBillDetail)
            .WithName("AddBillDetail")
            .WithSummary("Thêm chi ti?t d?ch v? vào hóa ??n");

        group.MapPut("details/{detailId:guid}", UpdateBillDetail)
            .WithName("UpdateBillDetail")
            .WithSummary("C?p nh?t chi ti?t hóa ??n");

        group.MapDelete("{billId:guid}/details/{detailId:guid}", DeleteBillDetail)
            .WithName("DeleteBillDetail")
            .WithSummary("Xóa chi ti?t hóa ??n");

        // Payment Commands
        group.MapPost("{billId:guid}/payment", MakePayment)
            .WithName("MakePayment")
            .WithSummary("Thanh toán hóa ??n");

        group.MapPatch("{billId:guid}/mark-overdue", MarkAsOverdue)
            .WithName("MarkBillAsOverdue")
            .WithSummary("?ánh d?u hóa ??n quá h?n");

        // Bill Queries
        group.MapGet(string.Empty, GetBills)
            .WithName("GetBills")
            .WithSummary("L?y danh sách hóa ??n có phân trang");

        group.MapGet("{billId:guid}", GetBillById)
            .WithName("GetBillById")
            .WithSummary("L?y thông tin hóa ??n chi ti?t theo ID");

        group.MapGet("room/{roomId:guid}", GetBillsByRoom)
            .WithName("GetBillsByRoom")
            .WithSummary("L?y danh sách hóa ??n theo phòng");

        group.MapGet("profile/{profileId:guid}", GetBillsByProfile)
            .WithName("GetBillsByProfile")
            .WithSummary("L?y danh sách hóa ??n theo ng??i thuê");

        group.MapGet("period/{month:int}/{year:int}", GetBillsByMonthYear)
            .WithName("GetBillsByMonthYear")
            .WithSummary("L?y danh sách hóa ??n theo tháng/n?m");
    }

    #region Bill Commands

    public static async Task<IResult> CreateBill(
        ISender sender,
        [FromBody] BillCommand.CreateBillCommand command)
    {
        var result = await sender.Send(command);
        return result.IsSuccess
            ? Results.Ok(result)
            : HandlerFailure(result);
    }

    public static async Task<IResult> UpdateBill(
        ISender sender,
        Guid billId,
        [FromBody] UpdateBillRequest request)
    {
        var updateCommand = new BillCommand.UpdateBillCommand(
            billId,
            request.DueDate,
            request.Notes);

        var result = await sender.Send(updateCommand);
        return Results.Ok(result);
    }

    public static async Task<IResult> DeleteBill(ISender sender, Guid billId)
    {
        var result = await sender.Send(new BillCommand.DeleteBillCommand(billId));
        return Results.Ok(result);
    }

    #endregion

    #region Bill Detail Commands

    public static async Task<IResult> AddBillDetail(
        ISender sender,
        Guid billId,
        [FromBody] AddBillDetailRequest request)
    {
        var addCommand = new BillCommand.AddBillDetailCommand(
            billId,
            request.ServiceType,
            request.ServiceName,
            request.Unit,
            request.Quantity,
            request.UnitPrice,
            request.OldIndex,
            request.NewIndex,
            request.Notes);

        var result = await sender.Send(addCommand);
        return Results.Ok(result);
    }

    public static async Task<IResult> UpdateBillDetail(
        ISender sender,
        Guid detailId,
        [FromBody] BillCommand.UpdateBillDetailCommand command)
    {
        var updateCommand = new BillCommand.UpdateBillDetailCommand(
            detailId,
            command.Quantity,
            command.UnitPrice,
            command.OldIndex,
            command.NewIndex,
            command.Notes);

        var result = await sender.Send(updateCommand);
        return Results.Ok(result);
    }

    public static async Task<IResult> DeleteBillDetail(ISender sender, Guid billId, Guid detailId)
    {
        var result = await sender.Send(new BillCommand.DeleteBillDetailCommand(billId, detailId));
        return Results.Ok(result);
    }

    #endregion

    #region Payment Commands

    public static async Task<IResult> MakePayment(
        ISender sender,
        Guid billId,
        [FromBody] MakePaymentRequest request)
    {
        var paymentCommand = new BillCommand.MakePaymentCommand(
            billId,
            request.Amount,
            request.PaymentMethod,
            request.PaymentDate);

        var result = await sender.Send(paymentCommand);
        return Results.Ok(result);
    }

    public static async Task<IResult> MarkAsOverdue(ISender sender, Guid billId)
    {
        var result = await sender.Send(new BillCommand.MarkBillAsOverdueCommand(billId));
        return Results.Ok(result);
    }

    #endregion

    #region Queries

    private static async Task<IResult> GetBills(
        ISender sender,
        string? searchTerm = null,
        BillStatus? status = null,
        Guid? roomId = null,
        Guid? profileId = null,
        int? month = null,
        int? year = null,
        string? sortColumn = null,
        string? sortOrder = null,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var query = new BillQuery.GetBillsQuery(
            searchTerm,
            status,
            roomId,
            profileId,
            month,
            year,
            sortColumn,
            SortOrderExtension.ConvertStringToSortOrder(sortOrder),
            pageIndex,
            pageSize);

        var result = await sender.Send(query);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetBillById(ISender sender, Guid billId)
    {
        var result = await sender.Send(new BillQuery.GetBillByIdQuery(billId));
        return Results.Ok(result);
    }

    private static async Task<IResult> GetBillsByRoom(ISender sender, Guid roomId)
    {
        var result = await sender.Send(new BillQuery.GetBillsByRoomQuery(roomId));
        return Results.Ok(result);
    }

    private static async Task<IResult> GetBillsByProfile(ISender sender, Guid profileId)
    {
        var result = await sender.Send(new BillQuery.GetBillsByProfileQuery(profileId));
        return Results.Ok(result);
    }

    private static async Task<IResult> GetBillsByMonthYear(ISender sender, int month, int year)
    {
        var result = await sender.Send(new BillQuery.GetBillsByMonthYearQuery(month, year));
        return Results.Ok(result);
    }

    #endregion

    #region Helper Records

    public record UpdateBillRequest(DateTime DueDate, string? Notes);
    
    public record AddBillDetailRequest(
        ServiceType ServiceType,
        string ServiceName,
        string Unit,
        decimal Quantity,
        decimal UnitPrice,
        decimal? OldIndex,
        decimal? NewIndex,
        string? Notes);

    public record MakePaymentRequest(
        decimal Amount,
        string PaymentMethod,
        DateTime? PaymentDate);

    #endregion
}
