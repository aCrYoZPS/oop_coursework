using MediatR;
using Wonderlust.Application.Features.Comments.Dtos;

namespace Wonderlust.Application.Features.Comments.Commands.CreateComment;

public record CreateCommentCommand(Guid AuthorId, Guid? ParentCommentId, Guid PostId, string Content)
    : IRequest<CommentDto>;