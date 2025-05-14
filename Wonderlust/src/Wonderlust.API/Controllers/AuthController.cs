using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Wonderlust.API.Responses.Auth;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Users.Queries.AuthorizeUser;

namespace Wonderlust.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IMediator mediator, IMapper mapper)
    : ControllerBase
{
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