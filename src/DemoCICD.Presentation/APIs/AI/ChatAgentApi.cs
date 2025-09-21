using Carter;
using DemoCICD.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DemoCICD.Presentation.APIs.AI;

/// <summary>
/// API endpoints cho AI Chat Agent quản lý role và permission
/// Tuân thủ Clean Architecture và sử dụng Carter framework
/// </summary>
public class ChatAgentApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/chat";

    /// <summary>
    /// Đăng ký các route cho AI Chat Agent
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("AI Chat Agent")
            .MapGroup(BaseUrl).HasApiVersion(1);

        // Các operations cho chat
        group1.MapPost("message", ProcessChatMessageV1).RequireAuthorization();
        group1.MapGet("history", GetChatHistoryV1).RequireAuthorization();
    }

    /// <summary>
    /// Xử lý tin nhắn chat từ người dùng (Version 1)
    /// Hỗ trợ các lệnh quản lý role và permission bằng ngôn ngữ tự nhiên
    /// </summary>
    /// <param name="sender">MediatR sender để gửi command</param>
    /// <param name="command">Command chứa tin nhắn chat</param>
    /// <returns>Kết quả xử lý chat bao gồm AI response và action đã thực hiện</returns>
    public static async Task<IResult> ProcessChatMessageV1(
        ISender sender, 
        [FromBody] DemoCICD.Contract.Services.V1.AI.Command.ProcessChatMessage command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    /// <summary>
    /// Lấy lịch sử chat của người dùng (Version 1)
    /// Hỗ trợ phân trang để hiển thị tin nhắn cũ
    /// </summary>
    /// <param name="sender">MediatR sender để gửi query</param>
    /// <param name="userId">ID của người dùng (optional)</param>
    /// <param name="page">Số trang (mặc định = 1)</param>
    /// <param name="pageSize">Số tin nhắn mỗi trang (mặc định = 20)</param>
    /// <returns>Danh sách tin nhắn chat đã phân trang</returns>
    public static async Task<IResult> GetChatHistoryV1(
        ISender sender,
        [FromQuery] string? userId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new DemoCICD.Contract.Services.V1.AI.Query.GetChatHistory(userId, page, pageSize);
        var result = await sender.Send(query);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}