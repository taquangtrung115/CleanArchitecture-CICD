using Asp.Versioning;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.Video;
using DemoCICD.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoCICD.Presentation.Controllers.V1.MotoGP;

[ApiVersion(1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class VideoController : ApiController
{
    public VideoController(ISender sender) : base(sender)
    {
    }

    [HttpGet]
    [ProducesResponseType(typeof(Result<PagedResult<Response.VideoResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetVideos(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? type = null,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? platform = null,
        [FromQuery] bool? isFeatured = null)
    {
        var query = new Query.GetVideosQuery(pageIndex, pageSize, type, searchTerm, platform, isFeatured);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Result<Response.VideoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVideoById(Guid id)
    {
        var query = new Query.GetVideoByIdQuery(id);
        var result = await Sender.Send(query);
        
        if (!result.IsSuccess)
            return NotFound(result);
            
        return Ok(result);
    }

    [HttpGet("external/{externalVideoId}")]
    [ProducesResponseType(typeof(Result<Response.VideoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVideoByExternalId(string externalVideoId)
    {
        var query = new Query.GetVideoByExternalIdQuery(externalVideoId);
        var result = await Sender.Send(query);
        
        if (!result.IsSuccess)
            return NotFound(result);
            
        return Ok(result);
    }

    [HttpGet("featured")]
    [ProducesResponseType(typeof(Result<IEnumerable<Response.VideoResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFeaturedVideos()
    {
        var query = new Query.GetFeaturedVideosQuery();
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpGet("type/{type}")]
    [ProducesResponseType(typeof(Result<PagedResult<Response.VideoResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetVideosByType(
        string type,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new Query.GetVideosByTypeQuery(type, pageIndex, pageSize);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpGet("platform/{platform}")]
    [ProducesResponseType(typeof(Result<PagedResult<Response.VideoResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetVideosByPlatform(
        string platform,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new Query.GetVideosByPlatformQuery(platform, pageIndex, pageSize);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(Result<PagedResult<Response.VideoResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchVideos(
        [FromQuery] string searchTerm,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return BadRequest("Search term is required");
        }

        var query = new Query.SearchVideosQuery(searchTerm, pageIndex, pageSize);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpGet("tag/{tag}")]
    [ProducesResponseType(typeof(Result<PagedResult<Response.VideoResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetVideosByTag(
        string tag,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new Query.GetVideosByTagQuery(tag, pageIndex, pageSize);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpGet("related")]
    [ProducesResponseType(typeof(Result<IEnumerable<Response.VideoResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRelatedVideos(
        [FromQuery] Guid? seasonId = null,
        [FromQuery] Guid? raceId = null,
        [FromQuery] Guid? teamId = null,
        [FromQuery] Guid? riderId = null,
        [FromQuery] int limit = 5)
    {
        var query = new Query.GetRelatedVideosQuery(seasonId, raceId, teamId, riderId, limit);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Result), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateVideo([FromBody] Command.CreateVideoCommand command)
    {
        var result = await Sender.Send(command);
        
        if (!result.IsSuccess)
            return BadRequest(result);
            
        return CreatedAtAction(nameof(CreateVideo), result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateVideo(Guid id, [FromBody] Command.UpdateVideoCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("Video ID mismatch");
        }

        var result = await Sender.Send(command);
        
        if (!result.IsSuccess)
            return BadRequest(result);
            
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteVideo(Guid id)
    {
        var command = new Command.DeleteVideoCommand(id);
        var result = await Sender.Send(command);
        
        if (!result.IsSuccess)
            return NotFound(result);
            
        return Ok(result);
    }

    [HttpPatch("{id:guid}/publish")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PublishVideo(Guid id)
    {
        var command = new Command.PublishVideoCommand(id);
        var result = await Sender.Send(command);
        
        if (!result.IsSuccess)
            return NotFound(result);
            
        return Ok(result);
    }

    [HttpPatch("{id:guid}/feature")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetVideoAsFeatured(Guid id)
    {
        var command = new Command.SetVideoAsFeaturedCommand(id);
        var result = await Sender.Send(command);
        
        if (!result.IsSuccess)
            return NotFound(result);
            
        return Ok(result);
    }

    [HttpPatch("{id:guid}/view")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> IncrementViewCount(Guid id)
    {
        var command = new Command.IncrementVideoViewCountCommand(id);
        var result = await Sender.Send(command);
        return Ok(result);
    }
}