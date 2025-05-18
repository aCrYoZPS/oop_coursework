using AutoMapper;
using MediatR;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Posts.Dtos;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Posts.Queries.GetPost;

public class GetPostQueryHandler(IPostRepository postRepository, IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<GetPostQuery, PostDto>
{
    public async Task<PostDto> Handle(GetPostQuery request, CancellationToken cancellationToken)
    {
        var existingPost = await postRepository.GetByIdAsync(request.PostId);
        if (existingPost == null)
        {
            throw new NotFoundException($"The post with id {request.PostId} not found.");
        }

        var existingUser = await userRepository.GetByIdAsync(existingPost.AuthorId);
        if (existingUser == null)
        {
            throw new NotFoundException($"The user with id {existingPost.AuthorId} not found.");
        }

        return mapper.Map<PostDto>((existingUser, existingPost));
    }
}