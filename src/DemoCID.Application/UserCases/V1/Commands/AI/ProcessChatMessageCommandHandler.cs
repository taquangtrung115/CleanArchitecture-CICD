using DemoCICD.Application.Abstractions;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.AI;
using Serilog;
using System.Text.Json;

namespace DemoCICD.Application.UserCases.V1.Commands.AI;

/// <summary>
/// Handler xử lý tin nhắn chat với AI Agent để quản lý role và permission
/// Tuân thủ Clean Architecture pattern và sử dụng MediatR cho CQRS
/// </summary>
public sealed class ProcessChatMessageCommandHandler : ICommandHandler<Command.ProcessChatMessage, Response.ChatResponse>
{
    private readonly IAiChatService _aiChatService;
    private readonly IIdentityManagementService _identityManagementService;

    /// <summary>
    /// Khởi tạo handler với các dependency cần thiết
    /// </summary>
    /// <param name="aiChatService">Service xử lý AI chat (OpenAI integration)</param>
    /// <param name="identityManagementService">Service quản lý identity operations</param>
    public ProcessChatMessageCommandHandler(
        IAiChatService aiChatService,
        IIdentityManagementService identityManagementService)
    {
        _aiChatService = aiChatService;
        _identityManagementService = identityManagementService;
    }

    /// <summary>
    /// Xử lý lệnh chat từ người dùng và thực hiện action tương ứng
    /// Hỗ trợ cả tiếng Việt và tiếng Anh
    /// </summary>
    /// <param name="request">Lệnh chat từ người dùng</param>
    /// <param name="cancellationToken">Token để cancel operation</param>
    /// <returns>Kết quả xử lý chat bao gồm response và action đã thực hiện</returns>
    public async Task<Result<Response.ChatResponse>> Handle(Command.ProcessChatMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var chatId = Guid.NewGuid().ToString();
            
            // Xử lý tin nhắn với AI để hiểu ý định của người dùng
            var aiResponse = await _aiChatService.ProcessChatMessageAsync(request.Message, cancellationToken);
            
            // Phân tích AI response để trích xuất action và parameters
            var actionResult = await ProcessActionFromAiResponse(aiResponse, cancellationToken);
            
            Log.Information("Tin nhắn chat đã được xử lý thành công. ChatId: {ChatId}, Message: {Message}", chatId, request.Message);

            var response = new Response.ChatResponse(
                chatId,
                request.Message,
                aiResponse,
                actionResult.Success,
                actionResult.ActionPerformed,
                actionResult.Result,
                DateTime.UtcNow);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Lỗi khi xử lý tin nhắn chat: {Message}", request.Message);
            return Result.Failure<Response.ChatResponse>(
                new Error("ChatProcessing.Error", "Đã xảy ra lỗi khi xử lý tin nhắn chat"));
        }
    }

    /// <summary>
    /// Xử lý và phân tích AI response để thực hiện action tương ứng
    /// Hỗ trợ các lệnh: tạo role, tạo quyền, gán quyền, gán user, liệt kê roles
    /// </summary>
    /// <param name="aiResponse">Phản hồi từ AI service</param>
    /// <param name="cancellationToken">Token để cancel operation</param>
    /// <returns>Kết quả thực hiện action</returns>
    private async Task<(bool Success, string? ActionPerformed, object? Result)> ProcessActionFromAiResponse(
        string aiResponse, 
        CancellationToken cancellationToken)
    {
        try
        {
            // Phân tích đơn giản - trong thực tế nên sử dụng parsing phức tạp hơn
            var lowerResponse = aiResponse.ToLowerInvariant();
            
            // Xử lý các lệnh tạo role (hỗ trợ tiếng Việt và tiếng Anh)
            if (lowerResponse.Contains("create role") || lowerResponse.Contains("tạo role"))
            {
                return await HandleCreateRoleAction(cancellationToken);
            }
            // Xử lý các lệnh tạo quyền
            else if (lowerResponse.Contains("create permission") || lowerResponse.Contains("tạo quyền"))
            {
                return await HandleCreatePermissionAction(cancellationToken);
            }
            // Xử lý các lệnh gán quyền vào role
            else if (lowerResponse.Contains("assign permission") || lowerResponse.Contains("gán quyền"))
            {
                return await HandleAssignPermissionAction(cancellationToken);
            }
            // Xử lý các lệnh gán user vào role
            else if (lowerResponse.Contains("assign user") || lowerResponse.Contains("gán user"))
            {
                return await HandleAssignUserToRoleAction(cancellationToken);
            }
            // Xử lý các lệnh liệt kê roles
            else if (lowerResponse.Contains("list roles") || lowerResponse.Contains("danh sách role"))
            {
                var roles = await _identityManagementService.GetRolesAsync(cancellationToken);
                return (true, "Liệt kê Roles", roles);
            }
            
            // Không xác định được action cụ thể
            return (false, null, null);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Lỗi khi xử lý action từ AI response: {Response}", aiResponse);
            return (false, null, null);
        }
    }

    /// <summary>
    /// Xử lý lệnh tạo role mới
    /// </summary>
    private async Task<(bool Success, string ActionPerformed, object? Result)> HandleCreateRoleAction(
        CancellationToken cancellationToken)
    {
        try
        {
            // Trích xuất đơn giản - trong thực tế cần parsing phức tạp hơn
            // Để demo, tạo role mặc định
            var result = await _identityManagementService.CreateRoleAsync(
                "AI Generated Role", 
                "Role được tạo bởi AI assistant", 
                "AI_ROLE", 
                cancellationToken);
            
            return (true, "Tạo Role", result);
        }
        catch
        {
            return (false, "Tạo Role", null);
        }
    }

    /// <summary>
    /// Xử lý lệnh tạo permission mới
    /// </summary>
    private async Task<(bool Success, string ActionPerformed, object? Result)> HandleCreatePermissionAction(
        CancellationToken cancellationToken)
    {
        try
        {
            // Để demo - tạo permission mặc định
            var defaultRoleId = Guid.NewGuid(); // Trong thực tế cần parse từ AI response
            var result = await _identityManagementService.CreatePermissionAsync(
                defaultRoleId, 
                "AI_FUNCTION", 
                "READ", 
                cancellationToken);
            
            return (true, "Tạo Permission", result);
        }
        catch
        {
            return (false, "Tạo Permission", null);
        }
    }

    /// <summary>
    /// Xử lý lệnh gán permission vào role
    /// </summary>
    private async Task<(bool Success, string ActionPerformed, object? Result)> HandleAssignPermissionAction(
        CancellationToken cancellationToken)
    {
        try
        {
            // Để demo
            var defaultRoleId = Guid.NewGuid();
            var result = await _identityManagementService.AssignPermissionToRoleAsync(
                defaultRoleId, 
                "AI_FUNCTION", 
                "WRITE", 
                cancellationToken);
            
            return (true, "Gán Permission vào Role", result);
        }
        catch
        {
            return (false, "Gán Permission vào Role", null);
        }
    }

    /// <summary>
    /// Xử lý lệnh gán user vào role
    /// </summary>
    private async Task<(bool Success, string ActionPerformed, object? Result)> HandleAssignUserToRoleAction(
        CancellationToken cancellationToken)
    {
        try
        {
            // Để demo
            var defaultUserId = Guid.NewGuid();
            var defaultRoleId = Guid.NewGuid();
            var result = await _identityManagementService.AssignUserToRoleAsync(
                defaultUserId, 
                defaultRoleId, 
                cancellationToken);
            
            return (true, "Gán User vào Role", result);
        }
        catch
        {
            return (false, "Gán User vào Role", null);
        }
    }
}
