namespace DemoCICD.Contract.Services.V1.AI;

/// <summary>
/// Các Response models cho AI Chat Agent
/// Chứa dữ liệu trả về từ các operations
/// </summary>
public static class Response
{
    /// <summary>
    /// Response cho một tin nhắn chat đã được xử lý
    /// </summary>
    /// <param name="Id">ID duy nhất của chat session</param>
    /// <param name="Message">Tin nhắn gốc từ người dùng</param>
    /// <param name="Response">Phản hồi từ AI</param>
    /// <param name="Success">Trạng thái thành công của action</param>
    /// <param name="ActionPerformed">Tên action đã được thực hiện (nếu có)</param>
    /// <param name="ActionResult">Kết quả của action (nếu có)</param>
    /// <param name="Timestamp">Thời gian xử lý</param>
    public record ChatResponse(
        string Id,
        string Message,
        string Response,
        bool Success,
        string? ActionPerformed = null,
        object? ActionResult = null,
        DateTime Timestamp = default);

    /// <summary>
    /// Response chứa lịch sử chat đã phân trang
    /// </summary>
    /// <param name="Messages">Danh sách tin nhắn</param>
    /// <param name="TotalCount">Tổng số tin nhắn</param>
    /// <param name="Page">Trang hiện tại</param>
    /// <param name="PageSize">Số tin nhắn mỗi trang</param>
    public record ChatHistoryResponse(
        List<ChatMessage> Messages,
        int TotalCount,
        int Page,
        int PageSize);

    /// <summary>
    /// Tin nhắn chat trong lịch sử
    /// </summary>
    /// <param name="Id">ID của tin nhắn</param>
    /// <param name="Message">Tin nhắn gốc</param>
    /// <param name="Response">Phản hồi từ AI</param>
    /// <param name="ActionPerformed">Action đã thực hiện</param>
    /// <param name="Timestamp">Thời gian</param>
    public record ChatMessage(
        string Id,
        string Message,
        string Response,
        string? ActionPerformed,
        DateTime Timestamp);
}