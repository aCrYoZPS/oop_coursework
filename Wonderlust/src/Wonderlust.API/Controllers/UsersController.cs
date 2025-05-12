using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wonderlust.API.Requests.Users;
using Wonderlust.Application.Features.Users.Commands.CreateUser;
using Wonderlust.Application.Features.Users.Queries.GetUser;

namespace Wonderlust.API.Controllers;

[ApiController]
[Route("users")]
public class UsersController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserRequest request)
    {
        var command = mapper.Map<CreateUserCommand>(request);
        try
        {
            var result = await mediator.Send(command);
            return CreatedAtAction(nameof(GetUser), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var query = new GetUserQuery(id);
        var result = await mediator.Send(query);

        return result != null ? Ok(result) : NotFound();
    }
}