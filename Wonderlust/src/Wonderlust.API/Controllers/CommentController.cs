using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wonderlust.API.Controllers;

[ApiController]
[Authorize]
[Route("{communityId:guid}/{postId:guid}/comments")]
public class CommentController(IMapper mapper, IMediator mediator) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> GetAllComments()
    {

    }
}
