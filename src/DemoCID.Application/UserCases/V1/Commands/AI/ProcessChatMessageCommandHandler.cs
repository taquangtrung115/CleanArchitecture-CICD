using DemoCICD.Application.Abstractions;
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.AI;
using Serilog;
using System.Text.Json;

namespace DemoCICD.Application.UserCases.V1.Commands.AI;

public sealed class ProcessChatMessageCommandHandler : ICommandHandler<Command.ProcessChatMessage, Response.ChatResponse>
{
    private readonly IAiChatService _aiChatService;
    private readonly IIdentityManagementService _identityManagementService;

    public ProcessChatMessageCommandHandler(
        IAiChatService aiChatService,
        IIdentityManagementService identityManagementService)
    {
        _aiChatService = aiChatService;
        _identityManagementService = identityManagementService;
    }

    public async Task<Result<Response.ChatResponse>> Handle(Command.ProcessChatMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var chatId = Guid.NewGuid().ToString();
            
            // Process the message with AI to understand the intent
            var aiResponse = await _aiChatService.ProcessChatMessageAsync(request.Message, cancellationToken);
            
            // Parse the AI response to extract action and parameters
            var actionResult = await ProcessActionFromAiResponse(aiResponse, cancellationToken);
            
            Log.Information("Chat message processed successfully. ChatId: {ChatId}, Message: {Message}", chatId, request.Message);

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
            Log.Error(ex, "Error processing chat message: {Message}", request.Message);
            return Result.Failure<Response.ChatResponse>(
                new Error("ChatProcessing.Error", "An error occurred while processing the chat message"));
        }
    }

    private async Task<(bool Success, string? ActionPerformed, object? Result)> ProcessActionFromAiResponse(
        string aiResponse, 
        CancellationToken cancellationToken)
    {
        try
        {
            // This is a simplified action parsing - in a real scenario, you'd use more sophisticated parsing
            var lowerResponse = aiResponse.ToLowerInvariant();
            
            if (lowerResponse.Contains("create role") || lowerResponse.Contains("tạo role"))
            {
                return await HandleCreateRoleAction(aiResponse, cancellationToken);
            }
            else if (lowerResponse.Contains("create permission") || lowerResponse.Contains("tạo quyền"))
            {
                return await HandleCreatePermissionAction(aiResponse, cancellationToken);
            }
            else if (lowerResponse.Contains("assign permission") || lowerResponse.Contains("gán quyền"))
            {
                return await HandleAssignPermissionAction(aiResponse, cancellationToken);
            }
            else if (lowerResponse.Contains("assign user") || lowerResponse.Contains("gán user"))
            {
                return await HandleAssignUserToRoleAction(aiResponse, cancellationToken);
            }
            else if (lowerResponse.Contains("list roles") || lowerResponse.Contains("danh sách role"))
            {
                var roles = await _identityManagementService.GetRolesAsync(cancellationToken);
                return (true, "List Roles", roles);
            }
            
            // No specific action identified
            return (false, null, null);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error processing action from AI response: {Response}", aiResponse);
            return (false, null, null);
        }
    }

    private async Task<(bool Success, string ActionPerformed, object? Result)> HandleCreateRoleAction(
        string aiResponse, 
        CancellationToken cancellationToken)
    {
        try
        {
            // Simple extraction - in reality, you'd use more sophisticated parsing
            // For demo purposes, we'll create a default role
            var result = await _identityManagementService.CreateRoleAsync(
                "AI Generated Role", 
                "Role created via AI assistant", 
                "AI_ROLE", 
                cancellationToken);
            
            return (true, "Create Role", result);
        }
        catch
        {
            return (false, "Create Role", null);
        }
    }

    private async Task<(bool Success, string ActionPerformed, object? Result)> HandleCreatePermissionAction(
        string aiResponse, 
        CancellationToken cancellationToken)
    {
        try
        {
            // For demo purposes - create a default permission
            var defaultRoleId = Guid.NewGuid(); // In reality, you'd parse this from the AI response
            var result = await _identityManagementService.CreatePermissionAsync(
                defaultRoleId, 
                "AI_FUNCTION", 
                "READ", 
                cancellationToken);
            
            return (true, "Create Permission", result);
        }
        catch
        {
            return (false, "Create Permission", null);
        }
    }

    private async Task<(bool Success, string ActionPerformed, object? Result)> HandleAssignPermissionAction(
        string aiResponse, 
        CancellationToken cancellationToken)
    {
        try
        {
            // For demo purposes
            var defaultRoleId = Guid.NewGuid();
            var result = await _identityManagementService.AssignPermissionToRoleAsync(
                defaultRoleId, 
                "AI_FUNCTION", 
                "WRITE", 
                cancellationToken);
            
            return (true, "Assign Permission to Role", result);
        }
        catch
        {
            return (false, "Assign Permission to Role", null);
        }
    }

    private async Task<(bool Success, string ActionPerformed, object? Result)> HandleAssignUserToRoleAction(
        string aiResponse, 
        CancellationToken cancellationToken)
    {
        try
        {
            // For demo purposes
            var defaultUserId = Guid.NewGuid();
            var defaultRoleId = Guid.NewGuid();
            var result = await _identityManagementService.AssignUserToRoleAsync(
                defaultUserId, 
                defaultRoleId, 
                cancellationToken);
            
            return (true, "Assign User to Role", result);
        }
        catch
        {
            return (false, "Assign User to Role", null);
        }
    }
}