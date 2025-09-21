using DemoCICD.Contract.Abstractions.Message;

namespace DemoCICD.Contract.Services.V1.AI;

/// <summary>
/// Các Command để tương tác với AI Chat Agent
/// Tuân thủ CQRS pattern cho Clean Architecture
/// </summary>
public static class Command
{
    /// <summary>
    /// Command để xử lý tin nhắn chat với AI
    /// Hỗ trợ các lệnh quản lý role và permission bằng ngôn ngữ tự nhiên (tiếng Việt và tiếng Anh)
    /// </summary>
    /// <param name="Message">Tin nhắn từ người dùng</param>
    /// <param name="UserId">ID của người dùng (optional, để tracking)</param>
    public record ProcessChatMessage(string Message, string? UserId = null) : ICommand<Response.ChatResponse>;
}