using DemoCICD.Contract.Abstractions.Message;

namespace DemoCICD.Contract.Services.V1.AI;

/// <summary>
/// Các Query để truy vấn dữ liệu từ AI Chat Agent
/// Tuân thủ CQRS pattern cho Clean Architecture
/// </summary>
public static class Query
{
    /// <summary>
    /// Query để lấy lịch sử chat của người dùng
    /// Hỗ trợ phân trang để tối ưu performance
    /// </summary>
    /// <param name="UserId">ID của người dùng (optional)</param>
    /// <param name="Page">Số trang (mặc định = 1)</param>
    /// <param name="PageSize">Số tin nhắn mỗi trang (mặc định = 20)</param>
    public record GetChatHistory(string? UserId = null, int Page = 1, int PageSize = 20) : IQuery<Response.ChatHistoryResponse>;
}