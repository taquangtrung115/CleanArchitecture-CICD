using DemoCICD.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;

namespace DemoCICD.Infrastructure.AI;

public class OpenAiChatService : IAiChatService
{
    private readonly OpenAIClient _openAiClient;
    private readonly ILogger<OpenAiChatService> _logger;
    private readonly string _apiKey;

    public OpenAiChatService(IConfiguration configuration, ILogger<OpenAiChatService> logger)
    {
        _logger = logger;
        _apiKey = configuration["OpenAI:ApiKey"] ?? "demo-key";
        _openAiClient = new OpenAIClient(_apiKey);
    }

    public async Task<string> ProcessChatMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        try
        {
            // Define the system prompt for role and permission management
            var systemPrompt = @"Bạn là một AI assistant chuyên về quản lý quyền và role trong hệ thống.
Bạn có thể thực hiện các thao tác sau:
1. Tạo role mới: 'tạo role [tên role]' hoặc 'create role [role name]'
2. Tạo quyền mới: 'tạo quyền [tên quyền]' hoặc 'create permission [permission name]'  
3. Gán quyền vào role: 'gán quyền [tên quyền] vào role [tên role]' hoặc 'assign permission [permission] to role [role]'
4. Gán user vào role: 'gán user [tên user] vào role [tên role]' hoặc 'assign user [user] to role [role]'
5. Liệt kê roles: 'danh sách roles' hoặc 'list roles'

Hãy trả lời bằng tiếng Việt và xác nhận thao tác bạn sẽ thực hiện.";

            var chatMessages = new List<ChatMessage>
            {
                ChatMessage.CreateSystemMessage(systemPrompt),
                ChatMessage.CreateUserMessage(message)
            };

            var chatOptions = new ChatCompletionOptions
            {
                Temperature = 0.7f
            };

            // For demo purposes, return a mock response since we might not have a real OpenAI API key
            if (_apiKey == "demo-key")
            {
                return GenerateMockResponse(message);
            }

            var response = await _openAiClient.GetChatClient("gpt-3.5-turbo")
                .CompleteChatAsync(chatMessages, chatOptions, cancellationToken);

            return response.Value.Content[0].Text ?? "Xin lỗi, tôi không thể xử lý yêu cầu này.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing chat message with OpenAI: {Message}", message);
            return GenerateMockResponse(message);
        }
    }

    public async Task<bool> CanPerformActionAsync(string action, CancellationToken cancellationToken = default)
    {
        // For demo purposes, assume all actions are allowed
        await Task.CompletedTask;
        return true;
    }

    private string GenerateMockResponse(string message)
    {
        var lowerMessage = message.ToLowerInvariant();

        if (lowerMessage.Contains("tạo role") || lowerMessage.Contains("create role"))
        {
            return "Tôi sẽ tạo một role mới cho bạn. Role sẽ được tạo với tên 'AI Generated Role' và mô tả 'Role được tạo bởi AI assistant'.";
        }
        else if (lowerMessage.Contains("tạo quyền") || lowerMessage.Contains("create permission"))
        {
            return "Tôi sẽ tạo một quyền mới với function 'AI_FUNCTION' và action 'READ'.";
        }
        else if (lowerMessage.Contains("gán quyền") || lowerMessage.Contains("assign permission"))
        {
            return "Tôi sẽ gán quyền vào role theo yêu cầu của bạn.";
        }
        else if (lowerMessage.Contains("gán user") || lowerMessage.Contains("assign user"))
        {
            return "Tôi sẽ gán user vào role theo yêu cầu của bạn.";
        }
        else if (lowerMessage.Contains("danh sách") || lowerMessage.Contains("list"))
        {
            return "Tôi sẽ hiển thị danh sách các roles trong hệ thống.";
        }
        else
        {
            return $"Tôi hiểu bạn muốn: {message}. Tôi sẽ cố gắng thực hiện yêu cầu này nếu có thể.";
        }
    }
}