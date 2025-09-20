using Carter;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Video;
using DemoCICD.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DemoCICD.Presentation.APIs.MotoGP;

public class VideoApi : ApiEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/motogp/videos")
            .WithTags("MotoGP - Videos")
            .RequireAuthorization();

        group.MapGet("", GetVideos)
            .WithName("GetVideos")
            .WithSummary("Get paginated list of MotoGP videos")
            .WithDescription("Retrieves a paginated list of MotoGP videos with optional filtering")
            .Produces<PagedResult<Response.VideoResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("{id:guid}", GetVideoById)
            .WithName("GetVideoById")
            .WithSummary("Get video by ID")
            .WithDescription("Retrieves a specific video by its unique identifier")
            .Produces<Response.VideoResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("external/{externalVideoId}", GetVideoByExternalId)
            .WithName("GetVideoByExternalId")
            .WithSummary("Get video by external ID")
            .WithDescription("Retrieves a specific video by its external platform ID")
            .Produces<Response.VideoResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("featured", GetFeaturedVideos)
            .WithName("GetFeaturedVideos")
            .WithSummary("Get featured videos")
            .WithDescription("Retrieves featured videos")
            .Produces<IEnumerable<Response.VideoResponse>>(StatusCodes.Status200OK);

        group.MapGet("type/{type}", GetVideosByType)
            .WithName("GetVideosByType")
            .WithSummary("Get videos by type")
            .WithDescription("Retrieves paginated videos filtered by type")
            .Produces<PagedResult<Response.VideoResponse>>(StatusCodes.Status200OK);

        group.MapGet("platform/{platform}", GetVideosByPlatform)
            .WithName("GetVideosByPlatform")
            .WithSummary("Get videos by platform")
            .WithDescription("Retrieves paginated videos filtered by platform")
            .Produces<PagedResult<Response.VideoResponse>>(StatusCodes.Status200OK);

        group.MapGet("search", SearchVideos)
            .WithName("SearchVideos")
            .WithSummary("Search videos")
            .WithDescription("Search videos by search term")
            .Produces<PagedResult<Response.VideoResponse>>(StatusCodes.Status200OK);

        group.MapGet("tag/{tag}", GetVideosByTag)
            .WithName("GetVideosByTag")
            .WithSummary("Get videos by tag")
            .WithDescription("Retrieves paginated videos filtered by tag")
            .Produces<PagedResult<Response.VideoResponse>>(StatusCodes.Status200OK);

        group.MapGet("related", GetRelatedVideos)
            .WithName("GetRelatedVideos")
            .WithSummary("Get related videos")
            .WithDescription("Retrieves videos related to specific entities")
            .Produces<IEnumerable<Response.VideoResponse>>(StatusCodes.Status200OK);

        group.MapPost("", CreateVideo)
            .WithName("CreateVideo")
            .WithSummary("Create a new video")
            .WithDescription("Creates a new video entry")
            .Produces<Result>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .ProducesValidationProblem();

        group.MapPut("{id:guid}", UpdateVideo)
            .WithName("UpdateVideo")
            .WithSummary("Update video")
            .WithDescription("Updates an existing video")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem();

        group.MapDelete("{id:guid}", DeleteVideo)
            .WithName("DeleteVideo")
            .WithSummary("Delete video")
            .WithDescription("Deletes a video")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPatch("{id:guid}/publish", PublishVideo)
            .WithName("PublishVideo")
            .WithSummary("Publish video")
            .WithDescription("Publishes a video")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPatch("{id:guid}/feature", SetVideoAsFeatured)
            .WithName("SetVideoAsFeatured")
            .WithSummary("Set video as featured")
            .WithDescription("Sets a video as featured")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPatch("{id:guid}/view", IncrementViewCount)
            .WithName("IncrementViewCount")
            .WithSummary("Increment video view count")
            .WithDescription("Increments the view count of a video")
            .Produces<Result>(StatusCodes.Status200OK);
    }

    public static async Task<IResult> GetVideos(
        ISender sender,
        int pageIndex = 1,
        int pageSize = 10,
        string? type = null,
        string? searchTerm = null,
        string? platform = null,
        bool? isFeatured = null)
    {
        var query = new Query.GetVideosQuery(pageIndex, pageSize, type, searchTerm, platform, isFeatured);
        var result = await sender.Send(query);
        return Results.Ok(result);
    }

    public static async Task<IResult> GetVideoById(ISender sender, Guid id)
    {
        var query = new Query.GetVideoByIdQuery(id);
        var result = await sender.Send(query);
        
        if (!result.IsSuccess)
            return Results.NotFound(result);
            
        return Results.Ok(result);
    }

    public static async Task<IResult> GetVideoByExternalId(ISender sender, string externalVideoId)
    {
        var query = new Query.GetVideoByExternalIdQuery(externalVideoId);
        var result = await sender.Send(query);
        
        if (!result.IsSuccess)
            return Results.NotFound(result);
            
        return Results.Ok(result);
    }

    public static async Task<IResult> GetFeaturedVideos(ISender sender)
    {
        var query = new Query.GetFeaturedVideosQuery();
        var result = await sender.Send(query);
        return Results.Ok(result);
    }

    public static async Task<IResult> GetVideosByType(
        ISender sender,
        string type,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var query = new Query.GetVideosByTypeQuery(type, pageIndex, pageSize);
        var result = await sender.Send(query);
        return Results.Ok(result);
    }

    public static async Task<IResult> GetVideosByPlatform(
        ISender sender,
        string platform,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var query = new Query.GetVideosByPlatformQuery(platform, pageIndex, pageSize);
        var result = await sender.Send(query);
        return Results.Ok(result);
    }

    public static async Task<IResult> SearchVideos(
        ISender sender,
        string searchTerm,
        int pageIndex = 1,
        int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return Results.BadRequest("Search term is required");
        }

        var query = new Query.SearchVideosQuery(searchTerm, pageIndex, pageSize);
        var result = await sender.Send(query);
        return Results.Ok(result);
    }

    public static async Task<IResult> GetVideosByTag(
        ISender sender,
        string tag,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var query = new Query.GetVideosByTagQuery(tag, pageIndex, pageSize);
        var result = await sender.Send(query);
        return Results.Ok(result);
    }

    public static async Task<IResult> GetRelatedVideos(
        ISender sender,
        Guid? seasonId = null,
        Guid? raceId = null,
        Guid? teamId = null,
        Guid? riderId = null,
        int limit = 5)
    {
        var query = new Query.GetRelatedVideosQuery(seasonId, raceId, teamId, riderId, limit);
        var result = await sender.Send(query);
        return Results.Ok(result);
    }

    public static async Task<IResult> CreateVideo(ISender sender, [FromBody] Command.CreateVideoCommand command)
    {
        var result = await sender.Send(command);
        
        if (!result.IsSuccess)
            return Results.BadRequest(result);
            
        return Results.Created($"/api/v1/motogp/videos", result);
    }

    public static async Task<IResult> UpdateVideo(ISender sender, Guid id, [FromBody] Command.UpdateVideoCommand command)
    {
        if (id != command.Id)
        {
            return Results.BadRequest("Video ID mismatch");
        }

        var result = await sender.Send(command);
        
        if (!result.IsSuccess)
            return Results.BadRequest(result);
            
        return Results.Ok(result);
    }

    public static async Task<IResult> DeleteVideo(ISender sender, Guid id)
    {
        var command = new Command.DeleteVideoCommand(id);
        var result = await sender.Send(command);
        
        if (!result.IsSuccess)
            return Results.NotFound(result);
            
        return Results.Ok(result);
    }

    public static async Task<IResult> PublishVideo(ISender sender, Guid id)
    {
        var command = new Command.PublishVideoCommand(id);
        var result = await sender.Send(command);
        
        if (!result.IsSuccess)
            return Results.NotFound(result);
            
        return Results.Ok(result);
    }

    public static async Task<IResult> SetVideoAsFeatured(ISender sender, Guid id)
    {
        var command = new Command.SetVideoAsFeaturedCommand(id);
        var result = await sender.Send(command);
        
        if (!result.IsSuccess)
            return Results.NotFound(result);
            
        return Results.Ok(result);
    }

    public static async Task<IResult> IncrementViewCount(ISender sender, Guid id)
    {
        var command = new Command.IncrementVideoViewCountCommand(id);
        var result = await sender.Send(command);
        return Results.Ok(result);
    }
}