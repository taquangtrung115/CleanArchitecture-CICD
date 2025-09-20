using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.AI;
using Serilog;

namespace DemoCICD.Application.UserCases.V1.Queries.AI;

public sealed class GetChatHistoryQueryHandler : IQueryHandler<Query.GetChatHistory, Response.ChatHistoryResponse>
{
    public async Task<Result<Response.ChatHistoryResponse>> Handle(Query.GetChatHistory request, CancellationToken cancellationToken)
    {
        try
        {
            // For demo purposes, return mock data
            // In a real implementation, you'd fetch from a database
            var mockMessages = new List<Response.ChatMessage>
            {
                new("1", "Tạo role quản trị", "Đã tạo role 'Quản trị viên' thành công", "Create Role", DateTime.UtcNow.AddHours(-2)),
                new("2", "Danh sách roles", "Hiển thị danh sách tất cả roles trong hệ thống", "List Roles", DateTime.UtcNow.AddHours(-1)),
                new("3", "Gán quyền USER_READ vào role Admin", "Đã gán quyền USER_READ vào role Admin thành công", "Assign Permission", DateTime.UtcNow.AddMinutes(-30))
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

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving chat history for user: {UserId}", request.UserId);
            return Result.Failure<Response.ChatHistoryResponse>(
                new Error("ChatHistory.Error", "An error occurred while retrieving chat history"));
        }
    }
}