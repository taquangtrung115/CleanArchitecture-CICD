namespace DemoCICD.Contract.Services.V1.AI;

public static class Response
{
    public record ChatResponse(
        string Id,
        string Message,
        string Response,
        bool Success,
        string? ActionPerformed = null,
        object? ActionResult = null,
        DateTime Timestamp = default);

    public record ChatHistoryResponse(
        List<ChatMessage> Messages,
        int TotalCount,
        int Page,
        int PageSize);

    public record ChatMessage(
        string Id,
        string Message,
        string Response,
        string? ActionPerformed,
        DateTime Timestamp);
}