using DemoCICD.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Text;
using System.Text.Json;

namespace DemoCICD.Infrastructure.AI;

public class OllamaChatService : IAiChatService
{
    private readonly HttpClient _httpClient;
    private readonly string _ollamaUrl;
    private readonly string _model;

    public OllamaChatService(IConfiguration configuration)
    {
        _ollamaUrl = configuration["Ollama:Url"] ?? "http://localhost:11434/api/generate";
        _model = configuration["Ollama:Model"] ?? "llama2";
        _httpClient = new HttpClient();
    }

    public async Task<string> ProcessChatMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        try
        {
            var systemPrompt = @"Bạn là một AI assistant chuyên về quản lý quyền và role trong hệ thống.
Bạn có thể thực hiện các thao tác sau:
1. Tạo role mới: 'tạo role [tên role]' hoặc 'create role [role name]'
2. Tạo quyền mới: 'tạo quyền [tên quyền]' hoặc 'create permission [permission name]'  
3. Gán quyền vào role: 'gán quyền [tên quyền] vào role [tên role]' hoặc 'assign permission [permission] to role [role]'
4. Gán user vào role: 'gán user [tên user] vào role [tên role]' hoặc 'assign user [user] to role [role]'
5. Liệt kê roles: 'danh sách roles' hoặc 'list roles'

Hãy trả lời bằng tiếng Việt và xác nhận thao tác bạn sẽ thực hiện.";

            var fullPrompt = $"{systemPrompt}\n\nUser: {message}";

            var payload = new
            {
                model = _model,
                prompt = fullPrompt,
                stream = false
            };

            var json = JsonSerializer.Serialize(payload);
            var response = await _httpClient.PostAsync(_ollamaUrl, new StringContent(json, Encoding.UTF8, "application/json"), cancellationToken);
            var respContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Log.Error("Ollama error: {Status} - {Content}", response.StatusCode, respContent);
                return GenerateMockResponse(message);
            }

            using var doc = JsonDocument.Parse(respContent);
            var reply = doc.RootElement.GetProperty("response").GetString();
            return reply ?? GenerateMockResponse(message);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error processing chat message with Ollama: {Message}", message);
            return GenerateMockResponse(message);
        }
    }

    public async Task<bool> CanPerformActionAsync(string action, CancellationToken cancellationToken = default)
    {
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
