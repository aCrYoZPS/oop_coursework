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
    IPostRepository postRepository
) : IRequestHandler<CreateCommentCommand, CommentDto>
{
    public async Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var existingPost = await postRepository.GetByIdAsync(request.PostId);

        if (existingPost == null)
        {
            throw new NotFoundException($"The post with id {request.PostId} not found");
        }

        if (request.ParentCommentId != null && await commentRepository.GetByIdAsync(request.ParentCommentId)) { }

        var comment = mapper.Map<Comment>(request);
        await commentRepository.AddAsync(comment);
        return mapper.Map<CommentDto>(comment);
    }
}
