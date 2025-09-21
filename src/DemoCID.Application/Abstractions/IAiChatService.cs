namespace DemoCICD.Application.Abstractions;

/// <summary>
/// Interface cho AI Chat Service
/// Xử lý tương tác với các AI models (OpenAI, Ollama, etc.)
/// </summary>
public interface IAiChatService
{
    /// <summary>
    /// Xử lý tin nhắn chat với AI để hiểu ý định người dùng
    /// Hỗ trợ ngôn ngữ tự nhiên (tiếng Việt và tiếng Anh)
    /// </summary>
    /// <param name="message">Tin nhắn từ người dùng</param>
    /// <param name="cancellationToken">Token để cancel operation</param>
    /// <returns>Phản hồi từ AI model</returns>
    Task<string> ProcessChatMessageAsync(string message, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Kiểm tra xem AI có thể thực hiện action cụ thể không
    /// </summary>
    /// <param name="action">Tên action cần kiểm tra</param>
    /// <param name="cancellationToken">Token để cancel operation</param>
    /// <returns>True nếu có thể thực hiện action</returns>
    Task<bool> CanPerformActionAsync(string action, CancellationToken cancellationToken = default);
}