using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Wonderlust.API.Controllers;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Communities.Dtos;
using Wonderlust.Application.Features.Subscriptions.Commands.Subscribe;
using Wonderlust.Application.Features.Subscriptions.Commands.Unsubscribe;
using Wonderlust.Application.Features.Subscriptions.Queries.GetSubscribers;
using Wonderlust.Application.Features.Subscriptions.Queries.GetSubscriptions;
using Wonderlust.Application.Features.Users.Dtos;

namespace Wonderlust.API.Tests;

public class SubscriptionControllerTests
{
    private readonly Mock<IMediator> mockMediator;
    private readonly SubscriptionController controller;

    public SubscriptionControllerTests()
    {
        mockMediator = new Mock<IMediator>();
        controller = new SubscriptionController(mockMediator.Object);
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
    public async Task SubscribeValidRequestReturnsCreatedResult()
    {
        var userId = Guid.NewGuid();
        var communityId = Guid.NewGuid();
        SetupUserContext(userId);

        mockMediator.Setup(m => m.Send(It.Is<SubscribeCommand>(c =>
                c.UserId == userId && c.CommunityId == communityId), CancellationToken.None))
            .Returns(Task.CompletedTask);

        var result = await controller.Subscribe(communityId);

        Assert.IsType<CreatedResult>(result);
        mockMediator.Verify(m => m.Send(It.IsAny<SubscribeCommand>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task SubscribeWhenCommunityNotFoundReturnsNotFound()
    {
        var userId = Guid.NewGuid();
        SetupUserContext(userId);
        const string exceptionMessage = "Community not found";

        mockMediator.Setup(m => m.Send(It.IsAny<SubscribeCommand>(), CancellationToken.None))
            .ThrowsAsync(new NotFoundException(exceptionMessage));

        var result = await controller.Subscribe(Guid.NewGuid());

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task UnsubscribeValidRequestReturnsNoContent()
    {
        var userId = Guid.NewGuid();
        var communityId = Guid.NewGuid();
        SetupUserContext(userId);

        mockMediator.Setup(m => m.Send(It.Is<UnsubscribeCommand>(c =>
                c.UserId == userId && c.CommunityId == communityId), CancellationToken.None))
            .Returns(Task.CompletedTask);

        var result = await controller.Unsubscribe(communityId);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetAllSubscribersWhenCommunityExistsReturnsOkResult()
    {
        var communityId = Guid.NewGuid();
        var expectedSubscribers = Array.Empty<UserDto>();

        mockMediator.Setup(m => m.Send(It.Is<GetSubscribersQuery>(q =>
                q.CommunityId == communityId), CancellationToken.None))
            .ReturnsAsync(expectedSubscribers);

        var result = await controller.GetAllSubscribers(communityId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedSubscribers, okResult.Value);
    }

    [Fact]
    public async Task GetAllSubscriptionsWhenUserExistsReturnsOkResult()
    {
        var userId = Guid.NewGuid();
        var expectedSubscriptions = Array.Empty<CommunityDto>();

        mockMediator.Setup(m => m.Send(It.Is<GetSubscriptionsQuery>(q =>
                q.UserId == userId), CancellationToken.None))
            .ReturnsAsync(expectedSubscriptions);

        var result = await controller.GetAllSubscriptions(userId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedSubscriptions, okResult.Value);
    }

    [Fact]
    public async Task UnsubscribeWhenNotFoundReturnsNotFound()
    {
        var userId = Guid.NewGuid();
        SetupUserContext(userId);
        const string exceptionMessage = "Subscription not found";

        mockMediator.Setup(m => m.Send(It.IsAny<UnsubscribeCommand>(), CancellationToken.None))
            .ThrowsAsync(new NotFoundException(exceptionMessage));

        var result = await controller.Unsubscribe(Guid.NewGuid());

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task SubscribeGeneralExceptionReturnsProblemResult()
    {
        var userId = Guid.NewGuid();
        SetupUserContext(userId);
        const string exceptionMessage = "Database error";

        mockMediator.Setup(m => m.Send(It.IsAny<SubscribeCommand>(), CancellationToken.None))
            .ThrowsAsync(new Exception(exceptionMessage));

        var result = await controller.Subscribe(Guid.NewGuid());

        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, problemResult.StatusCode);
    }

    [Fact]
    public async Task GetAllSubscriptionsWhenUserNotFoundReturnsNotFound()
    {
        var userId = Guid.NewGuid();
        const string exceptionMessage = "User not found";

        mockMediator.Setup(m => m.Send(It.IsAny<GetSubscriptionsQuery>(), CancellationToken.None))
            .ThrowsAsync(new NotFoundException(exceptionMessage));

        var result = await controller.GetAllSubscriptions(userId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task GetAllSubscribersGeneralExceptionReturnsProblemResult()
    {
        var communityId = Guid.NewGuid();
        const string exceptionMessage = "Service unavailable";

        mockMediator.Setup(m => m.Send(It.IsAny<GetSubscribersQuery>(), CancellationToken.None))
            .ThrowsAsync(new Exception(exceptionMessage));

        var result = await controller.GetAllSubscribers(communityId);

        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, problemResult.StatusCode);
    }
}