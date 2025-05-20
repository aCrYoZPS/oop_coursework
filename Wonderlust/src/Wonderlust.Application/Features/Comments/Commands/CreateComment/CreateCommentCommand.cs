using MediatR;
using Wonderlust.Application.Features.Comments.Dtos;

namespace Wonderlust.Application.Features.Comments.Commands.CreateComment;

public record CreateCommentCommand(
    string Content,
    Guid? ParentCommentId,
    Guid PostId,
    Guid AuthorId
) : IRequest<CommentDto>;
