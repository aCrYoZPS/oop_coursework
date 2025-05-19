using MediatR;
using Wonderlust.Application.Features.Comments.Dtos;

namespace Wonderlust.Application.Features.Comments.Queries.GetAllComments;

public record GetAllCommentsQuery(Guid PostId) : IRequest<IEnumerable<CommentDto>>;
