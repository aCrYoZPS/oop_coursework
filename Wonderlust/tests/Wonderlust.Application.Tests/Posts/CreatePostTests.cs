using Moq;
using AutoMapper;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Posts.Commands.CreatePost;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Tests.Posts;

public class CreatePostCommandHandlerTests
{
    private readonly Mock<IPostRepository> mockPostRepository;
    private readonly Mock<IUserRepository> mockUserRepository;
    private readonly Mock<ICommunityRepository> mockCommunityRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly CreatePostCommandHandler handler;

    public CreatePostCommandHandlerTests()
    {
        mockPostRepository = new Mock<IPostRepository>();
        mockUserRepository = new Mock<IUserRepository>();
        mockCommunityRepository = new Mock<ICommunityRepository>();
        mockMapper = new Mock<IMapper>();
        handler = new CreatePostCommandHandler(
                mockMapper.Object,
                mockUserRepository.Object,
                mockPostRepository.Object,
                mockCommunityRepository.Object
        );
    }

    [Fact]
    public async Task HandleCommunityNotFoundThrowsNotFoundException()
    {
        var communityId = Guid.NewGuid();
        var command = new CreatePostCommand("newtitle", "newcontent", null)
        {
            CommunityId = communityId
        };
        mockCommunityRepository.Setup(x => x.GetByIdAsync(command.CommunityId)).ReturnsAsync((Community)null);
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}
