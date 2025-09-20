using Carter;
using DemoCICD.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DemoCICD.Presentation.APIs.AI;

public class ChatAgentApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/chat";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("AI Chat Agent")
            .MapGroup(BaseUrl).HasApiVersion(1);

        // Chat operations
        group1.MapPost("message", ProcessChatMessageV1).RequireAuthorization();
        group1.MapGet("history", GetChatHistoryV1).RequireAuthorization();
    }

    public static async Task<IResult> ProcessChatMessageV1(
        ISender sender, 
        [FromBody] DemoCICD.Contract.Services.V1.AI.Command.ProcessChatMessage command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> GetChatHistoryV1(
        ISender sender,
        [FromQuery] string? userId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new DemoCICD.Contract.Services.V1.AI.Query.GetChatHistory(userId, page, pageSize);
        var result = await sender.Send(query);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}