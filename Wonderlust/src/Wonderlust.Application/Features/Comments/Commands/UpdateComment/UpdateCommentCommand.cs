using MediatR;
using Wonderlust.Application.Features.Comments.Dtos;

namespace Wonderlust.Application.Features.Comments.Commands.UpdateComment;

public record UpdateCommentCommand(Guid CommentId, string Content, Guid SenderId) : IRequest<CommentDto>;