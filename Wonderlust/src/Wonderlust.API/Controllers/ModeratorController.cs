using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wonderlust.API.Requests.Moderators;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Moderators.Commands.AssignModerator;
using Wonderlust.Application.Features.Moderators.Commands.RevokeModerator;
using Wonderlust.Application.Features.Moderators.Queries.GetModerators;

namespace Wonderlust.API.Controllers;

[ApiController]
[Authorize]
[Route("moderator/{communityId:guid}")]
public class ModeratorController(IMediator mediator) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> GetAllModerators(Guid communityId)
    {
        var query = new GetModeratorsQuery(communityId);
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
    public async Task<IActionResult> AssignModerator(Guid communityId, [FromBody] AssignModeratorRequest request)
    {
        var senderId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var command = new AssignModeratorCommand(request.UserId, communityId, senderId);

        try
        {
            await mediator.Send(command);
            return Created();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
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

    [HttpDelete("")]
    public async Task<IActionResult> RevokeModerator(Guid communityId, [FromBody] RevokeModeratorRequest request)
    {
        var senderId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var command = new RevokeModeratorCommand(request.UserId, communityId, senderId);

        try
        {
            await mediator.Send(command);
            return Created();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
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