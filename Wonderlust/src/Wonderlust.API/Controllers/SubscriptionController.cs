using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Subscriptions.Commands.Subscribe;
using Wonderlust.Application.Features.Subscriptions.Commands.Unsubscribe;
using Wonderlust.Application.Features.Subscriptions.Queries.GetSubscribers;
using Wonderlust.Application.Features.Subscriptions.Queries.GetSubscriptions;

namespace Wonderlust.API.Controllers;

[ApiController]
[Authorize]
[Route("subscriptions")]
public class SubscriptionController(IMediator mediator) : ControllerBase
{
    [HttpPost("community/{communityId:guid}/subscribe")]
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

    [HttpDelete("community/{communityId:guid}/unsubscribe")]
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

    [HttpGet("community/{communityId:guid}")]
    public async Task<IActionResult> GetAllSubscribers(Guid communityId)
    {
        var query = new GetSubscribersQuery(communityId);
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

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetAllSubscriptions(Guid userId)
    {
        var query = new GetSubscriptionsQuery(userId);
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
}