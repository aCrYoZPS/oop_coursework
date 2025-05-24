using Moq;
using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Wonderlust.API.Controllers;
using Wonderlust.API.Requests.Posts;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Posts.Commands.CreatePost;
using Wonderlust.Application.Features.Posts.Commands.DeletePost;
using Wonderlust.Application.Features.Posts.Commands.UpdatePost;
using Wonderlust.Application.Features.Posts.Dtos;
using Wonderlust.Application.Features.Posts.Queries.GetAllPosts;
using Wonderlust.Application.Features.Posts.Queries.GetPost;

public class PostControllerTests
{
    private readonly Mock<IMediator> mockMediator;
    private readonly Mock<IMapper> mockMapper;
    private readonly PostController controller;

    public PostControllerTests()
    {
        mockMediator = new Mock<IMediator>();
        mockMapper = new Mock<IMapper>();
        controller = new PostController(mockMediator.Object, mockMapper.Object);
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
    public async Task GetPostWhenExistsReturnsOkResult()
    {
        var postId = Guid.NewGuid();
        var expectedPost = new PostDto();
        mockMediator.Setup(m => m.Send(It.Is<GetPostQuery>(q => q.PostId == postId), CancellationToken.None))
            .ReturnsAsync(expectedPost);

        var result = await controller.GetPost(Guid.NewGuid(), postId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedPost, okResult.Value);
    }

    [Fact]
    public async Task GetPostWhenNotFoundReturnsNotFoundResult()
    {
        var postId = Guid.NewGuid();
        const string exceptionMessage = "Post not found";
        mockMediator.Setup(m => m.Send(It.IsAny<GetPostQuery>(), CancellationToken.None))
            .ThrowsAsync(new NotFoundException(exceptionMessage));

        var result = await controller.GetPost(Guid.NewGuid(), postId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task GetCommunityPostsWhenCommunityExistsReturnsOkResult()
    {
        var communityId = Guid.NewGuid();
        var expectedPosts = Array.Empty<PostDto>();
        mockMediator.Setup(m =>
                m.Send(It.Is<GetAllPostsQuery>(q => q.CommunityId == communityId), CancellationToken.None))
            .ReturnsAsync(expectedPosts);

        var result = await controller.GetCommunityPosts(communityId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedPosts, okResult.Value);
    }

    [Fact]
    public async Task CreatePostValidRequestReturnsCreatedResult()
    {
        var userId = Guid.NewGuid();
        var communityId = Guid.NewGuid();
        SetupUserContext(userId);

        var request = new CreatePostRequest("TITLE", "content", Guid.NewGuid());
        var command = new CreatePostCommand("TITLE", "content", Guid.NewGuid())
            { CommunityId = Guid.NewGuid(), AuthorId = userId };
        var expectedResult = new PostDto { Id = Guid.NewGuid() };

        mockMapper.Setup(m => m.Map<CreatePostCommand>(request)).Returns(command);
        mockMediator.Setup(m => m.Send(command, CancellationToken.None)).ReturnsAsync(expectedResult);

        var result = await controller.CreatePost(communityId, request);

        var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(PostController.GetPost), createdAtResult.ActionName);
        Assert.Equal(expectedResult.Id, ((PostDto)createdAtResult.Value).Id);
    }

    [Fact]
    public async Task CreatePostInvalidRequestReturnsBadRequest()
    {
        var userId = Guid.NewGuid();
        SetupUserContext(userId);

        var request = new CreatePostRequest("TITLE", "content", Guid.NewGuid());
        var command = new CreatePostCommand("TITLE", "content", Guid.NewGuid())
            { CommunityId = Guid.NewGuid(), AuthorId = userId };
        const string exceptionMessage = "Invalid request";

        mockMapper.Setup(m => m.Map<CreatePostCommand>(request)).Returns(command);
        mockMediator.Setup(m => m.Send(command, CancellationToken.None))
            .ThrowsAsync(new ArgumentException(exceptionMessage));

        var result = await controller.CreatePost(Guid.NewGuid(), request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(exceptionMessage, badRequestResult.Value);
    }

    [Fact]
    public async Task UpdatePostValidRequestReturnsOkResult()
    {
        var userId = Guid.NewGuid();
        var postId = Guid.NewGuid();
        SetupUserContext(userId);

        var request = new UpdatePostRequest("TITLE", "content", null);
        var command = new UpdatePostCommand("TITLE", "content", null)
        {
            SenderId = userId
        };
        var expectedResult = new PostDto();

        mockMapper.Setup(m => m.Map<UpdatePostCommand>(request)).Returns(command);
        mockMediator.Setup(m => m.Send(command, CancellationToken.None)).ReturnsAsync(expectedResult);

        var result = await controller.UpdatePost(Guid.NewGuid(), postId, request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    [Fact]
    public async Task UpdatePostWhenNotFoundReturnsNotFound()
    {
        var userId = Guid.NewGuid();
        var postId = Guid.NewGuid();
        SetupUserContext(userId);

        var request = new UpdatePostRequest("TITLE", "content", null);
        var command = new UpdatePostCommand("TITLE", "content", null)
        {
            SenderId = userId
        };
        const string exceptionMessage = "Post not found";

        mockMapper.Setup(m => m.Map<UpdatePostCommand>(request)).Returns(command);
        mockMediator.Setup(m => m.Send(command, CancellationToken.None))
            .ThrowsAsync(new NotFoundException(exceptionMessage));

        var result = await controller.UpdatePost(Guid.NewGuid(), postId, request);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task DeletePostValidRequestReturnsNoContent()
    {
        var postId = Guid.NewGuid();
        var command = new DeletePostCommand(postId);

        mockMediator.Setup(m => m.Send(command, CancellationToken.None))
            .Returns(Task.CompletedTask);

        var result = await controller.DeletePost(Guid.NewGuid(), postId);

        Assert.IsType<NoContentResult>(result);
        mockMediator.Verify(m => m.Send(command, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetCommunityPostsGeneralExceptionReturnsProblemResult()
    {
        var communityId = Guid.NewGuid();
        const string exceptionMessage = "Database error";

        mockMediator.Setup(m => m.Send(It.IsAny<GetAllPostsQuery>(), CancellationToken.None))
            .ThrowsAsync(new Exception(exceptionMessage));

        var result = await controller.GetCommunityPosts(communityId);

        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, problemResult.StatusCode);
    }

    [Fact]
    public async Task CreatePostWithoutUserContextReturnsProblemResult()
    {
        var request = new CreatePostRequest("TITLE", "content", null);
        var command = new CreatePostCommand("TITLE", "content", null);

        mockMapper.Setup(m => m.Map<CreatePostCommand>(request)).Returns(command);
        controller.ControllerContext = new ControllerContext();
        var result = await controller.CreatePost(Guid.NewGuid(), request);
        Assert.IsType<UnauthorizedResult>(result);
    }
}