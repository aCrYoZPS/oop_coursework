using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wonderlust.API.Requests.Comments;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Comments.Commands.CreateComment;
using Wonderlust.Application.Features.Comments.Commands.DeleteComment;
using Wonderlust.Application.Features.Comments.Commands.UpdateComment;
using Wonderlust.Application.Features.Comments.Queries.GetAllComments;
using Wonderlust.Application.Features.Comments.Queries.GetComment;

namespace Wonderlust.API.Controllers;

[ApiController]
[Authorize]
[Route("{communityId:guid}/{postId:guid}/comments")]
public class CommentController(IMapper mapper, IMediator mediator) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> GetAllComments(Guid communityId, Guid postId)
    {
        var query = new GetAllCommentsQuery(postId);
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

    [HttpGet("{commentId:guid}")]
    public async Task<IActionResult> GetComment(Guid commentId, Guid postId, Guid communityId)
    {
        var query = new GetCommentQuery(commentId);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> PostComment(Guid communityId, Guid postId, [FromBody] CreateCommentRequest request)
    {
        request.PostId = postId;
        try
        {
            request.AuthorId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        }
        catch (Exception ex)
        {
            return Unauthorized();
        }

        var command = mapper.Map<CreateCommentCommand>(request);
        try
        {
            var result = await mediator.Send(command);
            return Ok(result);
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

    [HttpDelete("{commentId:guid}")]
    public async Task<IActionResult> DeleteComment(Guid commentId, Guid communityId, Guid postId)
    {
        var senderId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var command = new DeleteCommentCommand(commentId, senderId);
        try
        {
            await mediator.Send(command);
            return NoContent();
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

    [HttpPut("{commentId:guid}")]
    public async Task<IActionResult> UpdateComment(
        Guid commentId, Guid communityId, Guid postId, [FromBody] UpdateCommentRequest request
    )
    {
        request.SenderId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        request.CommentId = commentId;
        var command = mapper.Map<UpdateCommentCommand>(request);
        try
        {
            var result = await mediator.Send(command);
            return Ok(result);
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
}