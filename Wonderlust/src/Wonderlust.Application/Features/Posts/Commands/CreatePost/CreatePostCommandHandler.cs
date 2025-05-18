using AutoMapper;
using MediatR;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Posts.Dtos;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Posts.Commands.CreatePost;

public class CreatePostCommandHandler(
    IMapper mapper,
    IUserRepository userRepository,
    IPostRepository postRepository,
    ICommunityRepository communityRepository
) : IRequestHandler<CreatePostCommand, PostDto>
{
    public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var existingCommunity = await communityRepository.GetByIdAsync(request.CommunityId);

        if (existingCommunity == null)
        {
            throw new NotFoundException($"Community with id {request.CommunityId} not found");
        }

        var existingUser = await userRepository.GetByIdAsync(request.AuthorId);

        if (existingUser == null)
        {
            throw new NotFoundException($"User with id {request.AuthorId} not found");
        }

        var post = mapper.Map<Post>(request);
        await postRepository.AddAsync(post);

        return mapper.Map<PostDto>((existingUser, post));
    }
}