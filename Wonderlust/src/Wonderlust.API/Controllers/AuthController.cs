using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Wonderlust.API.Requests.Users;
using Wonderlust.API.Responses.Auth;
using Wonderlust.API.Responses.Users;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Users.Commands.CreateUser;
using Wonderlust.Application.Features.Users.Queries.AuthorizeUser;
using Wonderlust.Application.Security;

namespace Wonderlust.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IMediator mediator, IMapper mapper, IConfiguration configuration)
    : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        [FromBody] CreateUserRequest request)
    {
        var command = mapper.Map<CreateUserCommand>(request);
        try
        {
            var result = await mediator.Send(command);
            var response = new CreateUserResponse(result,
                new TokenManager(configuration).GenerateJwtToken(result.Id, result.Email));
            return CreatedAtAction(actionName: nameof(UserController.GetUser),
                controllerName: nameof(User),
                new { id = result.Id }, response);
        }
        catch (AlreadyExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        string? token;
        var query = mapper.Map<AuthorizeUserQuery>(request);
        try
        {
            token = await mediator.Send(query);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }

        if (token == null)
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(new AuthenticateUserResponse(token));
    }
}