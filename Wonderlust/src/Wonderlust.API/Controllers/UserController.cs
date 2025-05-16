using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Wonderlust.API.Requests.Users;
using Wonderlust.API.Responses.Users;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Users.Commands.CreateUser;
using Wonderlust.Application.Features.Users.Commands.DeleteUser;
using Wonderlust.Application.Features.Users.Commands.UpdateUser;
using Wonderlust.Application.Features.Users.Dtos;
using Wonderlust.Application.Features.Users.Queries.GetUser;
using Wonderlust.Application.Security;

namespace Wonderlust.API.Controllers;

[ApiController]
[Authorize]
[Route("users")]
public class UserController(IMediator mediator, IMapper mapper) : ControllerBase
{

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        if (User.FindFirst(ClaimTypes.NameIdentifier)?.Value != id.ToString())
        {
            return Forbid();
        }

        var query = new GetUserQuery(id);
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
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
    {
        if (User.FindFirst(ClaimTypes.NameIdentifier)?.Value != id.ToString())
        {
            return Forbid();
        }

        var command = mapper.Map<UpdateUserCommand>(request);
        command.UserId = id;
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

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var command = new DeleteUserCommand(id);
        await mediator.Send(command);
        return NoContent();
    }
}