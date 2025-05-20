using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wonderlust.Application.Exceptions;
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
    
    [HttpDelete()]
}