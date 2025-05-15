using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Wonderlust.API.Requests.Communities;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Communities.Commands.CreateCommunity;
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
}
