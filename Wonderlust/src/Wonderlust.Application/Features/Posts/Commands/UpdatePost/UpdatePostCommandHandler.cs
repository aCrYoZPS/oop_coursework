using AutoMapper;
using MediatR;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Posts.Dtos;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Posts.Commands.UpdatePost;

public class UpdatePostCommandHandler(IMapper mapper, IPostRepository postRepository, IUserRepository userRepository)
    : IRequestHandler<UpdatePostCommand, PostDto>
{
    public async Task<PostDto> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var existingPost = await postRepository.GetByIdAsync(request.PostId);

        if (existingPost == null)
        {
            throw new NotFoundException($"The post with id {request.PostId} not found.");
        }

        if (existingPost.AuthorId != request.SenderId)
        {
            throw new UnauthorizedAccessException(
                $"The user {request.SenderId} has no access to the post {existingPost.Id}"
            );
        }

        var changed = false;

        if (request.Title != null)
        {
            existingPost.UpdateTitle(request.Title);
            changed = true;
        }

        if (request.Content != null)
        {
            existingPost.UpdateContent(request.Content);
            changed = true;
        }

        if (request.ImageId != null)
        {
            existingPost.UpdateImage(request.ImageId);
            changed = true;
        }

        if (changed)
        {
            await postRepository.UpdateAsync(existingPost);
        }

        return mapper.Map<PostDto>((await userRepository.GetByIdAsync(existingPost.AuthorId), existingPost));
    }
}