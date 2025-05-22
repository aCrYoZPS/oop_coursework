using MediatR;
using Wonderlust.Application.Exceptions;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Comments.Commands.DeleteComment;

public class DeleteCommentCommandHandler(ICommentRepository commentRepository, IUserRepository userRepository)
    : IRequestHandler<DeleteCommentCommand>
{
    public async Task Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetByIdAsync(request.SenderId);
        if (existingUser == null)
        {
            throw new NotFoundException($"The user with id {request.SenderId} not found");
        }

        var existingComment = await commentRepository.GetByIdAsync(request.CommentId);
        if (existingComment == null)
        {
            return;
        }

        if (existingComment.AuthorId != existingUser.Id)
        {
            throw new UnauthorizedAccessException(
                $"User with id {existingUser.Id} has no access to comment {existingComment.Id}"
            );
        }

        await commentRepository.DeleteAsync(existingComment.Id);
    }
}