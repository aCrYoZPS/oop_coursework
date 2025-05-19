using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wonderlust.API.Requests.Communities;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Communities.Commands.CreateCommunity;
using Wonderlust.Application.Features.Communities.Commands.DeleteCommunity;
using Wonderlust.Application.Features.Communities.Commands.UpdateCommunity;
using Wonderlust.Application.Features.Communities.Queries.GetAllCommunities;
using Wonderlust.Application.Features.Communities.Queries.GetCommunity;

namespace Wonderlust.API.Controllers;

[ApiController]
[Authorize]
[Route("communities")]
public class CommunityController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCommuntity([FromBody] CreateCommunityRequest request)
    {
        var command = mapper.Map<CreateCommunityCommand>(request);
        command.CreatorId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetCommunity), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetCommunities()
    {
        var query = new GetAllCommunitiesQuery();
        try
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                detail: ex.Message
            );
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCommunity(Guid id)
    {
        var query = new GetCommunityQuery(id);
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

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCommunity(Guid id, [FromBody] UpdateCommunityRequest request)
    {
        var command = mapper.Map<UpdateCommunityCommand>(request);
        command.CommunityId = id;
        command.SenderId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        try
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
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

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCommunity(Guid id)
    {
        var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var command = new DeleteCommunityCommand(id, userId);
        try
        {
            await mediator.Send(command);
            return NoContent();
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