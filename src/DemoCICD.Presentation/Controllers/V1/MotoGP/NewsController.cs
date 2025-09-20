using Asp.Versioning;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.MotoGP.News;
using DemoCICD.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoCICD.Presentation.Controllers.V1.MotoGP;

[ApiVersion(1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class NewsController : ApiController
{
    public NewsController(ISender sender) : base(sender)
    {
    }

    [HttpGet]
    [ProducesResponseType(typeof(Result<PagedResult<Response.NewsResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetNews(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? category = null,
        [FromQuery] string? searchTerm = null,
        [FromQuery] bool? isFeatured = null,
        [FromQuery] bool? isBreaking = null)
    {
        var query = new Query.GetNewsQuery(pageIndex, pageSize, category, searchTerm, isFeatured, isBreaking);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Result<Response.NewsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNewsById(Guid id)
    {
        var query = new Query.GetNewsByIdQuery(id);
        var result = await Sender.Send(query);
        
        if (result.IsFailure)
            return HandlerFailure(result);
            
        return Ok(result);
    }

    [HttpGet("slug/{slug}")]
    [ProducesResponseType(typeof(Result<Response.NewsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNewsBySlug(string slug)
    {
        var query = new Query.GetNewsBySlugQuery(slug);
        var result = await Sender.Send(query);
        
        if (result.IsFailure)
            return HandlerFailure(result);
            
        return Ok(result);
    }

    [HttpGet("featured")]
    [ProducesResponseType(typeof(Result<IEnumerable<Response.NewsResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFeaturedNews([FromQuery] int limit = 5)
    {
        var query = new Query.GetFeaturedNewsQuery(limit);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpGet("breaking")]
    [ProducesResponseType(typeof(Result<IEnumerable<Response.NewsResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBreakingNews()
    {
        var query = new Query.GetBreakingNewsQuery();
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpGet("category/{category}")]
    [ProducesResponseType(typeof(Result<PagedResult<Response.NewsResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNewsByCategory(
        string category,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new Query.GetNewsByCategoryQuery(category, pageIndex, pageSize);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    // Admin endpoints
    [HttpPost]
    [ProducesResponseType(typeof(Result<Response.NewsResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateNews([FromBody] Command.CreateNewsCommand command)
    {
        var result = await Sender.Send(command);
        
        if (result.IsFailure)
            return HandlerFailure(result);
            
        return CreatedAtAction(nameof(GetNewsById), new { id = result.Value.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateNews(Guid id, [FromBody] Command.UpdateNewsCommand command)
    {
        if (id != command.Id)
            return BadRequest("URL ID does not match command ID");
            
        var result = await Sender.Send(command);
        
        if (result.IsFailure)
            return HandlerFailure(result);
            
        return Ok(result);
    }

    [HttpPost("{id:guid}/publish")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PublishNews(Guid id)
    {
        var command = new Command.PublishNewsCommand(id);
        var result = await Sender.Send(command);
        
        if (result.IsFailure)
            return HandlerFailure(result);
            
        return Ok(result);
    }

    [HttpPost("{id:guid}/archive")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ArchiveNews(Guid id)
    {
        var command = new Command.ArchiveNewsCommand(id);
        var result = await Sender.Send(command);
        
        if (result.IsFailure)
            return HandlerFailure(result);
            
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteNews(Guid id)
    {
        var command = new Command.DeleteNewsCommand(id);
        var result = await Sender.Send(command);
        
        if (result.IsFailure)
            return HandlerFailure(result);
            
        return Ok(result);
    }

    [HttpPut("{id:guid}/slug")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateNewsSlug(Guid id, [FromBody] UpdateSlugRequest request)
    {
        var command = new Command.UpdateNewsSlugCommand(id, request.Slug);
        var result = await Sender.Send(command);
        
        if (result.IsFailure)
            return HandlerFailure(result);
            
        return Ok(result);
    }
}

public record UpdateSlugRequest(string Slug);