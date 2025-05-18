using AutoMapper;
using MediatR;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Posts.Dtos;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Posts.Queries.GetAllPosts;

public class GetAllPostsQueryHandler(
    IMapper mapper,
    ICommunityRepository communityRepository,
    IPostRepository postRepository,
    IUserRepository userRepository) : IRequestHandler<GetAllPostsQuery, IEnumerable<PostDto>>
{
    public async Task<IEnumerable<PostDto>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        var existingCommunity = await communityRepository.GetByIdAsync(request.CommunityId);

        if (existingCommunity == null)
        {
            throw new NotFoundException($"The community with id {request.CommunityId} not found.");
        }

        var posts = await postRepository.GetAllByCommunityAsync(existingCommunity.Id);
        var res = await Task.WhenAll(posts.Select(async post =>
            mapper.Map<PostDto>((await userRepository.GetByIdAsync(post.AuthorId), post))));
        return res;
    }
}