using Moq;
using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Wonderlust.API.Controllers;
using Wonderlust.API.Requests.Comments;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Comments.Commands.CreateComment;
using Wonderlust.Application.Features.Comments.Commands.DeleteComment;
using Wonderlust.Application.Features.Comments.Commands.UpdateComment;
using Wonderlust.Application.Features.Comments.Dtos;
using Wonderlust.Application.Features.Comments.Queries.GetAllComments;
using Wonderlust.Application.Features.Comments.Queries.GetComment;

namespace Wonderlust.API.Tests;

public class CommentControllerTests
{
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<IMediator> mockMediator;
    private readonly CommentController controller;

    public CommentControllerTests()
    {
        mockMapper = new Mock<IMapper>();
        mockMediator = new Mock<IMediator>();
        controller = new CommentController(mockMapper.Object, mockMediator.Object);
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
    public async Task GetAllCommentsWhenPostExistsReturnsOkResult()
    {
        var postId = Guid.NewGuid();
        var expectedResult = Array.Empty<CommentDto>();
        mockMediator.Setup(m => m.Send(It.IsAny<GetAllCommentsQuery>(), CancellationToken.None))
            .ReturnsAsync(expectedResult);

        var result = await controller.GetAllComments(Guid.NewGuid(), postId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    [Fact]
    public async Task GetAllCommentsWhenPostNotFoundReturnsNotFound()
    {
        var postId = Guid.NewGuid();
        mockMediator.Setup(m => m.Send(It.IsAny<GetAllCommentsQuery>(), CancellationToken.None))
            .ThrowsAsync(new NotFoundException("Not found"));

        var result = await controller.GetAllComments(Guid.NewGuid(), postId);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetCommentWhenExistsReturnsOkResult()
    {
        var commentId = Guid.NewGuid();
        var expectedResult = new CommentDto(commentId, "content", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow);
        mockMediator.Setup(m => m.Send(It.IsAny<GetCommentQuery>(), CancellationToken.None))
            .ReturnsAsync(expectedResult);

        var result = await controller.GetComment(commentId, Guid.NewGuid(), Guid.NewGuid());

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    [Fact]
    public async Task PostCommentWithValidRequestReturnsCreatedResult()
    {
        var userId = Guid.NewGuid();
        var postId = Guid.NewGuid();
        SetupUserContext(userId);

        var request = new CreateCommentRequest("Test content", null);
        var command = new CreateCommentCommand(userId, null, postId, "Test content");
        var expectedResult = new CommentDto(Id: Guid.NewGuid(), "Test content", userId, Guid.NewGuid(),
            DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);

        mockMapper.Setup(m => m.Map<CreateCommentCommand>(request)).Returns(command);
        mockMediator.Setup(m => m.Send(command, CancellationToken.None)).ReturnsAsync(expectedResult);

        var result = await controller.PostComment(Guid.NewGuid(), postId, request);

        var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(expectedResult.Id, ((CommentDto)createdAtResult.Value).Id);
    }

    [Fact]
    public async Task PostCommentWithInvalidUserReturnsProblem()
    {
        controller.ControllerContext = new ControllerContext();
        var result =
            await controller.PostComment(Guid.NewGuid(), Guid.NewGuid(), new CreateCommentRequest("Content", null));
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task DeleteCommentWhenAuthorizedReturnsNoContent()
    {
        var userId = Guid.NewGuid();
        var commentId = Guid.NewGuid();
        SetupUserContext(userId);

        mockMediator.Setup(m => m.Send(It.IsAny<DeleteCommentCommand>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        var result = await controller.DeleteComment(commentId, Guid.NewGuid(), Guid.NewGuid());
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteCommentWhenNotFoundReturnsNotFound()
    {
        var userId = Guid.NewGuid();
        SetupUserContext(userId);

        mockMediator.Setup(m => m.Send(It.IsAny<DeleteCommentCommand>(), CancellationToken.None))
            .ThrowsAsync(new NotFoundException("Not found"));

        var result = await controller.DeleteComment(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task UpdateCommentWithValidRequestReturnsOkResult()
    {
        var userId = Guid.NewGuid();
        var commentId = Guid.NewGuid();
        var parentCommentId = Guid.NewGuid();
        SetupUserContext(userId);

        var request = new UpdateCommentRequest("Updated content");
        var command = new UpdateCommentCommand(commentId, "Updated content", userId);
        var expectedResult = new CommentDto(commentId, "content", userId, parentCommentId, DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow);

        mockMapper.Setup(m => m.Map<UpdateCommentCommand>(request)).Returns(command);
        mockMediator.Setup(m => m.Send(command, CancellationToken.None)).ReturnsAsync(expectedResult);

        var result = await controller.UpdateComment(commentId, Guid.NewGuid(), Guid.NewGuid(), request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    [Fact]
    public async Task UpdateCommentWithInvalidArgumentsReturnsBadRequest()
    {
        var userId = Guid.NewGuid();
        SetupUserContext(userId);

        mockMediator.Setup(m => m.Send(It.IsAny<UpdateCommentCommand>(), CancellationToken.None))
            .ThrowsAsync(new ArgumentException("Invalid"));

        var result = await controller.UpdateComment(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            new UpdateCommentRequest("Content"));
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateCommentWhenNotFoundReturnsNotFound()
    {
        var userId = Guid.NewGuid();
        SetupUserContext(userId);

        mockMediator.Setup(m => m.Send(It.IsAny<UpdateCommentCommand>(), CancellationToken.None))
            .ThrowsAsync(new NotFoundException("Not found"));

        var result = await controller.UpdateComment(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            new UpdateCommentRequest("Content"));
        Assert.IsType<NotFoundObjectResult>(result);
    }
}