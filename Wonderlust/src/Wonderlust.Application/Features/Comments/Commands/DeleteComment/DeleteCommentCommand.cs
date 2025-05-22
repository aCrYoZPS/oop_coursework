using MediatR;

namespace Wonderlust.Application.Features.Comments.Commands.DeleteComment;

public class DeleteCommentCommand(Guid commentId, Guid senderId) : IRequest
{
    public Guid CommentId { get; init; } = commentId;
    public Guid SenderId { get; init; } = senderId;
}