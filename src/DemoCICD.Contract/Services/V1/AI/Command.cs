using DemoCICD.Contract.Abstractions.Message;

namespace DemoCICD.Contract.Services.V1.AI;

public static class Command
{
    public record ProcessChatMessage(string Message, string? UserId = null) : ICommand<Response.ChatResponse>;
}