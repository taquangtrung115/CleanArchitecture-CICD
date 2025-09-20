using Carter;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.News;
using DemoCICD.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DemoCICD.Presentation.APIs.MotoGP;

public class NewsApi : ApiEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/motogp/news")
            .WithTags("MotoGP - News")
            ;

        group.MapGet("", GetNews)
            .WithName("GetNews")
            .WithSummary("Get paginated list of MotoGP news")
            .WithDescription("Retrieves a paginated list of MotoGP news with optional filtering")
            .Produces<PagedResult<Response.NewsResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("{id:guid}", GetNewsById)
            .WithName("GetNewsById")
            .WithSummary("Get news article by ID")
            .WithDescription("Retrieves a specific news article by its unique identifier")
            .Produces<Response.NewsResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("slug/{slug}", GetNewsBySlug)
            .WithName("GetNewsBySlug")
            .WithSummary("Get news article by slug")
            .WithDescription("Retrieves a specific news article by its URL slug")
            .Produces<Response.NewsResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("featured", GetFeaturedNews)
            .WithName("GetFeaturedNews")
            .WithSummary("Get featured news articles")
            .WithDescription("Retrieves featured news articles with a specified limit")
            .Produces<IEnumerable<Response.NewsResponse>>(StatusCodes.Status200OK);

        group.MapGet("breaking", GetBreakingNews)
            .WithName("GetBreakingNews")
            .WithSummary("Get breaking news articles")
            .WithDescription("Retrieves current breaking news articles")
            .Produces<IEnumerable<Response.NewsResponse>>(StatusCodes.Status200OK);

        group.MapGet("category/{category}", GetNewsByCategory)
            .WithName("GetNewsByCategory")
            .WithSummary("Get news articles by category")
            .WithDescription("Retrieves paginated news articles filtered by category")
            .Produces<PagedResult<Response.NewsResponse>>(StatusCodes.Status200OK);
    }

    public static async Task<IResult> GetNews(
        ISender sender,
        int pageIndex = 1,
        int pageSize = 10,
        string? category = null,
        string? searchTerm = null,
        bool? isFeatured = null,
        bool? isBreaking = null)
    {
        var query = new Query.GetNewsQuery(pageIndex, pageSize, category, searchTerm, isFeatured, isBreaking);
        var result = await sender.Send(query);
        return Results.Ok(result);
    }

    public static async Task<IResult> GetNewsById(ISender sender, Guid id)
    {
        var query = new Query.GetNewsByIdQuery(id);
        var result = await sender.Send(query);
        
        if (result.IsFailure)
            return HandlerFailure(result);
            
        return Results.Ok(result);
    }

    public static async Task<IResult> GetNewsBySlug(ISender sender, string slug)
    {
        var query = new Query.GetNewsBySlugQuery(slug);
        var result = await sender.Send(query);
        
        if (result.IsFailure)
            return HandlerFailure(result);
            
        return Results.Ok(result);
    }

    public static async Task<IResult> GetFeaturedNews(ISender sender, int limit = 5)
    {
        var query = new Query.GetFeaturedNewsQuery(limit);
        var result = await sender.Send(query);
        return Results.Ok(result);
    }

    public static async Task<IResult> GetBreakingNews(ISender sender)
    {
        var query = new Query.GetBreakingNewsQuery();
        var result = await sender.Send(query);
        return Results.Ok(result);
    }

    public static async Task<IResult> GetNewsByCategory(
        ISender sender,
        string category,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var query = new Query.GetNewsByCategoryQuery(category, pageIndex, pageSize);
        var result = await sender.Send(query);
        return Results.Ok(result);
    }
}
