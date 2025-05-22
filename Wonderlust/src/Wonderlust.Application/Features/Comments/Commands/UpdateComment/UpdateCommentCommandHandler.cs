using AutoMapper;
using MediatR;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Comments.Dtos;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Comments.Commands.UpdateComment;

public class UpdateCommentCommandHandler(
    IMapper mapper,
    ICommentRepository commentRepository,
    IUserRepository userRepository) : IRequestHandler<UpdateCommentCommand, CommentDto>
{
    public async Task<CommentDto> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetByIdAsync(request.SenderId);
        if (existingUser == null)
        {
            throw new NotFoundException($"User with id {request.SenderId} not found");
        }

        var existingComment = await commentRepository.GetByIdAsync(request.CommentId);
        if (existingComment == null)
        {
            throw new NotFoundException($"Comment with id {request.CommentId} not found");
        }

        if (existingComment.AuthorId != existingUser.Id)
        {
            throw new UnauthorizedAccessException(
                $"User with id {existingUser.Id} has no access to comment {existingComment.Id}"
            );
        }

        existingComment.UpdateContent(request.Content);
        await commentRepository.UpdateAsync(existingComment);
        return mapper.Map<CommentDto>(existingComment);
    }
}