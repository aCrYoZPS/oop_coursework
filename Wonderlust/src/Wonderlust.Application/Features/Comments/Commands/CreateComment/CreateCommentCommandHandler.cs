using AutoMapper;
using MediatR;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Comments.Dtos;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Comments.Commands.CreateComment;

public class CreateCommentCommandHandler(
    IMapper mapper,
    ICommentRepository commentRepository,
    IUserRepository userRepository,
    IPostRepository postRepository
) : IRequestHandler<CreateCommentCommand, CommentDto>
{
    public async Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetByIdAsync(request.AuthorId);
        if (existingUser == null)
        {
            throw new NotFoundException($"User with id {request.AuthorId} not found");
        }

        var existingPost = await postRepository.GetByIdAsync(request.PostId);
        if (existingPost == null)
        {
            throw new NotFoundException($"Post with id {request.PostId} not found");
        }

        if (request.ParentCommentId != null)
        {
            var existingParentComment = await commentRepository.GetByIdAsync(request.ParentCommentId.Value);
            if (existingParentComment == null)
            {
                throw new NotFoundException($"Comment with id {request.ParentCommentId} not found");
            }
        }

        var comment = mapper.Map<Comment>(request);
        await commentRepository.AddAsync(comment);

        return mapper.Map<CommentDto>(comment);
    }
}