using Moq;
using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Wonderlust.API.Controllers;
using Wonderlust.API.Requests.Communities;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Communities.Commands.CreateCommunity;
using Wonderlust.Application.Features.Communities.Commands.DeleteCommunity;
using Wonderlust.Application.Features.Communities.Commands.UpdateCommunity;
using Wonderlust.Application.Features.Communities.Dtos;
using Wonderlust.Application.Features.Communities.Queries.GetCommunity;

namespace Wonderlust.API.Tests;

public class CommunityControllerTests
{
    private readonly Mock<IMediator> mockMediator;
    private readonly Mock<IMapper> mockMapper;
    private readonly CommunityController controller;

    public CommunityControllerTests()
    {
        mockMediator = new Mock<IMediator>();
        mockMapper = new Mock<IMapper>();
        controller = new CommunityController(mockMediator.Object, mockMapper.Object);
    }

    [Fact]
    public async Task CreateCommunityValidRequestReturnsCreatedResult()
    {
        var userId = Guid.NewGuid();
        var user = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ]));
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        const string communityName = "TestName";
        const string communityDescription = "Test Desc";
        var request = new CreateCommunityRequest(communityName, communityDescription);
        var command = new CreateCommunityCommand { CreatorId = userId };
        var expectedResult =
            new CommunityDto(Guid.NewGuid(), communityName, communityDescription, DateTimeOffset.UtcNow);

        mockMapper.Setup(m => m.Map<CreateCommunityCommand>(request)).Returns(command);
        mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

        var result = await controller.CreateCommunity(request);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(CommunityController.GetCommunity), createdAtActionResult.ActionName);
        Assert.Equal(expectedResult.Id, ((dynamic)createdAtActionResult.Value).Id);
        mockMapper.Verify(m => m.Map<CreateCommunityCommand>(request), Times.Once);
        mockMediator.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetCommunityValidIdReturnsOkResult()
    {
        var communityId = Guid.NewGuid();
        var query = new GetCommunityQuery(communityId);
        const string communityName = "TestName";
        const string communityDescription = "Test Desc";
        var expectedResult =
            new CommunityDto(Guid.NewGuid(), communityName, communityDescription, DateTimeOffset.UtcNow);

        mockMediator.Setup(m => m.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

        var result = await controller.GetCommunity(communityId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
        mockMediator.Verify(m => m.Send(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetCommunityNotFoundReturnsNotFoundResult()
    {
        var communityId = Guid.NewGuid();
        var query = new GetCommunityQuery(communityId);
        const string exceptionMessage = "Community not found.";

        mockMediator.Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException(exceptionMessage));

        var result = await controller.GetCommunity(communityId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task GetCommunityGeneralExceptionReturnsProblemResult()
    {
        var communityId = Guid.NewGuid();
        var query = new GetCommunityQuery(communityId);
        const string exceptionMessage = "An error occurred.";

        mockMediator.Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(exceptionMessage));

        var result = await controller.GetCommunity(communityId);

        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, problemResult.StatusCode);

        var problemDetails = Assert.IsType<ProblemDetails>(problemResult.Value);
        Assert.Equal(exceptionMessage, problemDetails.Detail);
    }

    [Fact]
    public async Task UpdateCommunityValidRequestReturnsOkResult()
    {
        var userId = Guid.NewGuid();
        var communityId = Guid.NewGuid();
        var user = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ]));

        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        const string newCommunityName = "TestName";
        const string newCommunityDescription = "Test Desc";

        var request = new UpdateCommunityRequest(newCommunityName, newCommunityDescription);
        var command = new UpdateCommunityCommand { CommunityId = communityId };

        var expectedResult =
            new CommunityDto(communityId, newCommunityName, newCommunityDescription, DateTimeOffset.UtcNow);

        mockMapper.Setup(m => m.Map<UpdateCommunityCommand>(request)).Returns(command);
        mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await controller.UpdateCommunity(communityId, request);

        Assert.IsType<OkObjectResult>(result);
        mockMapper.Verify(m => m.Map<UpdateCommunityCommand>(request), Times.Once);
        mockMediator.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCommunityArgumentExceptionReturnsBadRequest()
    {
        var userId = Guid.NewGuid();
        var communityId = Guid.NewGuid();
        var user = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ]));
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        const string newName = "newname";
        const string newDescription = "newdesc";
        var request = new UpdateCommunityRequest(newName, newDescription);
        var command = new UpdateCommunityCommand
            { Name = newName, Description = newDescription, SenderId = userId, CommunityId = communityId };
        const string exceptionMessage = "Invalid argument.";

        mockMapper.Setup(m => m.Map<UpdateCommunityCommand>(request)).Returns(command);
        mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ArgumentException(exceptionMessage));

        var result = await controller.UpdateCommunity(communityId, request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(exceptionMessage, badRequestResult.Value);
    }


    [Fact]
    public async Task UpdateCommunityNotFoundExceptionReturnsNotFoundResult()
    {
        var communityId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var user = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ]));
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        const string newName = "newname";
        const string newDesc = "newdesc";

        var request = new UpdateCommunityRequest(newName, newDesc);
        var command = new UpdateCommunityCommand
            { Name = newName, Description = newDesc, SenderId = userId, CommunityId = communityId };
        const string exceptionMessage = "Community not found.";

        mockMapper.Setup(m => m.Map<UpdateCommunityCommand>(request)).Returns(command);
        mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException(exceptionMessage));

        var result = await controller.UpdateCommunity(communityId, request);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task UpdateCommunityUnauthorizedUserReturnsForbidResult()
    {
        var userId = Guid.NewGuid();
        var communityId = Guid.NewGuid();
        var user = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ]));
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        const string newName = "newname";
        const string newDesc = "newdesc";
        var request = new UpdateCommunityRequest(newName, newDesc);

        var command = new UpdateCommunityCommand
            { Name = newName, Description = newDesc, CommunityId = communityId, SenderId = userId };
        const string exceptionMessage = "Unauthorized";

        mockMapper.Setup(mapper => mapper.Map<UpdateCommunityCommand>(request)).Returns(command);
        mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new UnauthorizedAccessException(exceptionMessage));

        var result = await controller.UpdateCommunity(communityId, request);
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task DeleteCommunityValidRequestReturnsNoContent()
    {
        var userId = Guid.NewGuid();
        var communityId = Guid.NewGuid();
        var user = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ]));
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        var command = new DeleteCommunityCommand(communityId, userId);

        mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await controller.DeleteCommunity(communityId);

        Assert.IsType<NoContentResult>(result);
        mockMediator.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteCommunityUnauthorizedUserReturnsForbidResult()
    {
        var userId = Guid.NewGuid();
        var communityId = Guid.NewGuid();
        var user = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ]));
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        var command = new DeleteCommunityCommand(communityId, userId);
        const string exceptionMessage = "Unauthorized access.";

        mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new UnauthorizedAccessException(exceptionMessage));

        var result = await controller.DeleteCommunity(communityId);

        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task DeleteCommunityGeneralExceptionReturnsProblemResult()
    {
        var userId = Guid.NewGuid();
        var communityId = Guid.NewGuid();
        var user = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ]));
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        var command = new DeleteCommunityCommand(communityId, userId);
        const string exceptionMessage = "An error occurred.";

        mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(exceptionMessage));

        var result = await controller.DeleteCommunity(communityId);

        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, problemResult.StatusCode);

        var problemDetails = Assert.IsType<ProblemDetails>(problemResult.Value);
        Assert.Equal(exceptionMessage, problemDetails.Detail);
    }
}