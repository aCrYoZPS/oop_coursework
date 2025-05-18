using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wonderlust.API.Requests.Posts;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Posts.Commands.CreatePost;
using Wonderlust.Application.Features.Posts.Commands.DeletePost;
using Wonderlust.Application.Features.Posts.Commands.UpdatePost;
using Wonderlust.Application.Features.Posts.Queries.GetAllPosts;
using Wonderlust.Application.Features.Posts.Queries.GetPost;

namespace Wonderlust.API.Controllers;

[ApiController]
[Authorize]
[Route("{communityId:guid}/posts")]
public class PostController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpGet("{postId:guid}")]
    public async Task<IActionResult> GetPost(Guid communityId, Guid postId)
    {
        var query = new GetPostQuery(postId);
        try
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                detail: ex.Message
            );
        }
    }

    [HttpGet("")]
    public async Task<IActionResult> GetCommunityPosts(Guid communityId)
    {
        var query = new GetAllPostsQuery(communityId);
        try
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                detail: ex.Message
            );
        }
    }

    [HttpPost("")]
    public async Task<IActionResult> CreatePost(Guid communityId, [FromBody] CreatePostRequest request)
    {
        var command = mapper.Map<CreatePostCommand>(request);
        command.CommunityId = communityId;
        command.AuthorId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        try
        {
            var result = await mediator.Send(command);
            return CreatedAtAction(nameof(GetPost), new { communityId = communityId, postId = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                detail: ex.Message
            );
        }
    }

    [HttpPut("{postId:guid}")]
    public async Task<IActionResult> UpdatePost(Guid communityId, Guid postId, [FromBody] UpdatePostRequest request)
    {
        var command = mapper.Map<UpdatePostCommand>(request);
        command.SenderId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        command.PostId = postId;
        try
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                detail: ex.Message
            );
        }
    }

    [HttpDelete("{postId:guid}")]
    public async Task<IActionResult> DeletePost(Guid communityId, Guid postId)
    {
        var command = new DeletePostCommand(postId);
        await mediator.Send(command);
        return NoContent();
    }
}