using MediatR;
using Wonderlust.Application.Features.Comments.Dtos;

namespace Wonderlust.Application.Features.Comments.Queries.GetComment;

public record GetCommentQuery(Guid CommentId) : IRequest<CommentDto>;