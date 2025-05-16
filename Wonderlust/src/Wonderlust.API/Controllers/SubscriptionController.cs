using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Subscriptions.Commands.Subscribe;
using Wonderlust.Application.Features.Subscriptions.Commands.Unsubscribe;

namespace Wonderlust.API.Controllers;

[ApiController]
[Authorize]
[Route("subscriptions")]
public class SubscriptionController(IMediator mediator) : ControllerBase
{
    [HttpPost("{communityId:guid}/subscribe")]
    public async Task<IActionResult> Subscribe(Guid communityId)
    {
        var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var subscribeCommand = new SubscribeCommand(userId, communityId);
        try
        {
            await mediator.Send(subscribeCommand);
            return Created();
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

    [HttpDelete("{communityId:guid}/unsubscribe")]
    public async Task<IActionResult> Unsubscribe(Guid communityId)
    {
        var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var unsubscribeCommand = new UnsubscribeCommand(userId, communityId);
        try
        {
            await mediator.Send(unsubscribeCommand);
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

    [HttpGet("community/{communityId:guid}/all")]
    public async Task<IActionResult> GetAllSubscribers(Guid communityId)
    {

    }

    [HttpGet("user/{userId:guid}/all")]
    public async Task<IActionResult> GetAllSubscriptions(Guid userId)
    {

    }
}