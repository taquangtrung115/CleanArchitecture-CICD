using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.AI;
using Serilog;

namespace DemoCICD.Application.UserCases.V1.Queries.AI;

/// <summary>
/// Handler để lấy lịch sử chat của người dùng với AI Agent
/// Tuân thủ Clean Architecture và CQRS pattern
/// </summary>
public sealed class GetChatHistoryQueryHandler : IQueryHandler<Query.GetChatHistory, Response.ChatHistoryResponse>
{
    /// <summary>
    /// Xử lý truy vấn lấy lịch sử chat
    /// </summary>
    /// <param name="request">Truy vấn lịch sử chat</param>
    /// <param name="cancellationToken">Token để cancel operation</param>
    /// <returns>Danh sách tin nhắn chat đã phân trang</returns>
    public async Task<Result<Response.ChatHistoryResponse>> Handle(Query.GetChatHistory request, CancellationToken cancellationToken)
    {
        try
        {
            // Để demo, trả về dữ liệu mẫu
            // Trong implementation thực tế, sẽ truy vấn từ database
            var mockMessages = new List<Response.ChatMessage>
            {
                new("1", "Tạo role quản trị", "Đã tạo role 'Quản trị viên' thành công", "Tạo Role", DateTime.UtcNow.AddHours(-2)),
                new("2", "Danh sách roles", "Hiển thị danh sách tất cả roles trong hệ thống", "Liệt kê Roles", DateTime.UtcNow.AddHours(-1)),
                new("3", "Gán quyền USER_READ vào role Admin", "Đã gán quyền USER_READ vào role Admin thành công", "Gán Permission", DateTime.UtcNow.AddMinutes(-30)),
                new("4", "Create role Manager", "Role 'Manager' has been created successfully", "Tạo Role", DateTime.UtcNow.AddMinutes(-15)),
                new("5", "List all permissions", "Displayed all available permissions in the system", "Liệt kê Permissions", DateTime.UtcNow.AddMinutes(-5))
            };

            var pagedMessages = mockMessages
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var response = new Response.ChatHistoryResponse(
                pagedMessages,
                mockMessages.Count,
                request.Page,
                request.PageSize);

            Log.Information("Đã truy xuất lịch sử chat thành công cho user: {UserId}, Page: {Page}, PageSize: {PageSize}", 
                request.UserId, request.Page, request.PageSize);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Lỗi khi truy xuất lịch sử chat cho user: {UserId}", request.UserId);
            return Result.Failure<Response.ChatHistoryResponse>(
                new Error("ChatHistory.Error", "Đã xảy ra lỗi khi truy xuất lịch sử chat"));
        }
    }
}