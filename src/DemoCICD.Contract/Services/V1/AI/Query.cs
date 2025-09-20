using DemoCICD.Contract.Abstractions.Message;

namespace DemoCICD.Contract.Services.V1.AI;

public static class Query
{
    public record GetChatHistory(string? UserId = null, int Page = 1, int PageSize = 20) : IQuery<Response.ChatHistoryResponse>;
}