using Xunit;
using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using Wonderlust.API.Controllers;
using Wonderlust.API.Requests.Moderators;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Moderators.Commands.AssignModerator;
using Wonderlust.Application.Features.Moderators.Commands.RevokeModerator;
using Wonderlust.Application.Features.Moderators.Queries.GetModerators;
using Wonderlust.Application.Features.Users.Dtos;

namespace Wonderlust.API.Tests;

public class ModeratorControllerTests
{
    private readonly Mock<IMediator> mockMediator;
    private readonly ModeratorController controller;

    public ModeratorControllerTests()
    {
        mockMediator = new Mock<IMediator>();
        controller = new ModeratorController(mockMediator.Object);
    }

    private void SetupUserContext(Guid userId)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ]));
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task GetAllModeratorsWhenCommunityExistsReturnsOkResult()
    {
        var communityId = Guid.NewGuid();
        var expectedModerators = Array.Empty<UserDto>();

        mockMediator.Setup(m =>
                m.Send(It.Is<GetModeratorsQuery>(q => q.CommunityId == communityId), CancellationToken.None))
            .ReturnsAsync(expectedModerators);

        var result = await controller.GetAllModerators(communityId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedModerators, okResult.Value);
    }

    [Fact]
    public async Task GetAllModeratorsWhenCommunityNotFoundReturnsNotFound()
    {
        var communityId = Guid.NewGuid();
        const string exceptionMessage = "Community not found";

        mockMediator.Setup(m => m.Send(It.IsAny<GetModeratorsQuery>(), CancellationToken.None))
            .ThrowsAsync(new NotFoundException(exceptionMessage));

        var result = await controller.GetAllModerators(communityId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task AssignModeratorValidRequestReturnsCreatedResult()
    {
        var senderId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var communityId = Guid.NewGuid();
        SetupUserContext(senderId);

        var request = new AssignModeratorRequest(userId);
        var command = new AssignModeratorCommand(userId, communityId, senderId);

        mockMediator.Setup(m => m.Send(command, CancellationToken.None))
            .Returns(Task.CompletedTask);

        var result = await controller.AssignModerator(communityId, request);

        Assert.IsType<CreatedResult>(result);
        mockMediator.Verify(m => m.Send(command, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task AssignModeratorUnauthorizedReturnsForbid()
    {
        var senderId = Guid.NewGuid();
        var communityId = Guid.NewGuid();
        SetupUserContext(senderId);

        var request = new AssignModeratorRequest(Guid.NewGuid());
        const string exceptionMessage = "Unauthorized access";

        mockMediator.Setup(m => m.Send(It.IsAny<AssignModeratorCommand>(), CancellationToken.None))
            .ThrowsAsync(new UnauthorizedAccessException(exceptionMessage));

        var result = await controller.AssignModerator(communityId, request);

        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task RevokeModeratorValidRequestReturnsCreatedResult()
    {
        var senderId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var communityId = Guid.NewGuid();
        SetupUserContext(senderId);

        var request = new RevokeModeratorRequest(userId);
        var command = new RevokeModeratorCommand(userId, communityId, senderId);

        mockMediator.Setup(m => m.Send(command, CancellationToken.None))
            .Returns(Task.CompletedTask);

        var result = await controller.RevokeModerator(communityId, request);

        Assert.IsType<CreatedResult>(result);
        mockMediator.Verify(m => m.Send(command, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task RevokeModeratorNotFoundReturnsNotFound()
    {
        var senderId = Guid.NewGuid();
        var communityId = Guid.NewGuid();
        SetupUserContext(senderId);

        var request = new RevokeModeratorRequest(Guid.NewGuid());
        var exceptionMessage = "Moderator not found";

        mockMediator.Setup(m => m.Send(It.IsAny<RevokeModeratorCommand>(), CancellationToken.None))
            .ThrowsAsync(new NotFoundException(exceptionMessage));

        var result = await controller.RevokeModerator(communityId, request);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task AssignModeratorGeneralExceptionReturnsProblemResult()
    {
        var senderId = Guid.NewGuid();
        var communityId = Guid.NewGuid();
        SetupUserContext(senderId);

        var request = new AssignModeratorRequest(Guid.NewGuid());
        var exceptionMessage = "Internal server error";

        mockMediator.Setup(m => m.Send(It.IsAny<AssignModeratorCommand>(), CancellationToken.None))
            .ThrowsAsync(new Exception(exceptionMessage));

        var result = await controller.AssignModerator(communityId, request);

        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, problemResult.StatusCode);
    }

    [Fact]
    public async Task RevokeModeratorUnauthorizedReturnsForbid()
    {
        var senderId = Guid.NewGuid();
        var communityId = Guid.NewGuid();
        SetupUserContext(senderId);

        var request = new RevokeModeratorRequest(Guid.NewGuid());
        var exceptionMessage = "Unauthorized access";

        mockMediator.Setup(m => m.Send(It.IsAny<RevokeModeratorCommand>(), CancellationToken.None))
            .ThrowsAsync(new UnauthorizedAccessException(exceptionMessage));

        var result = await controller.RevokeModerator(communityId, request);

        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task GetAllModeratorsGeneralExceptionReturnsProblemResult()
    {
        var communityId = Guid.NewGuid();
        var exceptionMessage = "Database connection failed";

        mockMediator.Setup(m => m.Send(It.IsAny<GetModeratorsQuery>(), CancellationToken.None))
            .ThrowsAsync(new Exception(exceptionMessage));

        var result = await controller.GetAllModerators(communityId);

        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, problemResult.StatusCode);
    }
}