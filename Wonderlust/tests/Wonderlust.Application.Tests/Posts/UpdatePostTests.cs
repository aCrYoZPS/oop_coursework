using Moq;
using AutoMapper;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Posts.Commands.UpdatePost;
using Wonderlust.Application.Features.Posts.Dtos;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Tests.Posts;

public class UpdatePostCommandHandlerTests
{
    private readonly Mock<IPostRepository> mockPostRepository;
    private readonly Mock<IUserRepository> mockUserRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly UpdatePostCommandHandler handler;

    public UpdatePostCommandHandlerTests()
    {
        mockPostRepository = new Mock<IPostRepository>();
        mockUserRepository = new Mock<IUserRepository>();
        mockMapper = new Mock<IMapper>();
        handler = new UpdatePostCommandHandler(mockMapper.Object, mockPostRepository.Object, mockUserRepository.Object);
    }

    [Fact]
    public async Task HandlePostNotFoundThrowsNotFoundException()
    {
        var command = new UpdatePostCommand("newtitle", "newcontent", null);
        mockPostRepository.Setup(x => x.GetByIdAsync(command.PostId)).ReturnsAsync((Post)null);
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleUnauthorizedUserThrowsUnauthorizedAccessException()
    {
        var existingPost = new Post("title", null, "content", Guid.NewGuid(), Guid.NewGuid());
        var command = new UpdatePostCommand("newtitle", "newcontent", null)
        {
            PostId = existingPost.Id,
            SenderId = Guid.NewGuid()
        };

        mockPostRepository.Setup(x => x.GetByIdAsync(existingPost.Id)).ReturnsAsync(existingPost);
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleValidTitleUpdateUpdatesAndSavesPost()
    {
        var senderId = Guid.NewGuid();
        var existingPost = new Post("title", null, "content", Guid.NewGuid(), senderId);
        var command = new UpdatePostCommand("newtitle", "newcontent", null)
        {
            PostId = existingPost.Id,
            SenderId = senderId
        };

        mockPostRepository.Setup(x => x.GetByIdAsync(existingPost.Id)).ReturnsAsync(existingPost);
        await handler.Handle(command, CancellationToken.None);

        Assert.Equal("newtitle", existingPost.Title);
        mockPostRepository.Verify(x => x.UpdateAsync(existingPost), Times.Once);
    }

    [Fact]
    public async Task HandleNoChangesDoesNotCallUpdate()
    {
        var senderId = Guid.NewGuid();
        var existingPost = new Post("title", null, "content", Guid.NewGuid(), senderId);
        var command = new UpdatePostCommand(null, null, null)
        {
            PostId = existingPost.Id,
            SenderId = senderId
        };

        mockPostRepository.Setup(x => x.GetByIdAsync(existingPost.Id)).ReturnsAsync(existingPost);
        await handler.Handle(command, CancellationToken.None);

        mockPostRepository.Verify(x => x.UpdateAsync(It.IsAny<Post>()), Times.Never);
    }

    [Fact]
    public async Task HandleValidRequestReturnsMappedDto()
    {
        var user = new User("a", "b", "c@email.com");
        var post = new Post("title", null, "content", Guid.NewGuid(), user.Id);
        var expectedDto = new PostDto();
        var command = new UpdatePostCommand(null, null, null)
        {
            PostId = post.Id,
            SenderId = user.Id
        };

        mockPostRepository.Setup(x => x.GetByIdAsync(post.Id)).ReturnsAsync(post);
        mockUserRepository.Setup(x => x.GetByIdAsync(user.Id)).ReturnsAsync(user);
        var tuple = (user, post);
        mockMapper.Setup(x => x.Map<PostDto>(tuple)).Returns(expectedDto);

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.Same(expectedDto, result);
    }

    [Fact]
    public async Task HandleMultipleChangesUpdatesOnce()
    {
        var senderId = Guid.NewGuid();
        var existingPost = new Post("title", null, "content", Guid.NewGuid(), senderId);
        var command = new UpdatePostCommand("newtitle", "newcontent", Guid.NewGuid())
        {
            PostId = existingPost.Id,
            SenderId = senderId
        };

        mockPostRepository.Setup(x => x.GetByIdAsync(existingPost.Id)).ReturnsAsync(existingPost);
        await handler.Handle(command, CancellationToken.None);

        mockPostRepository.Verify(x => x.UpdateAsync(existingPost), Times.Once);
    }
}