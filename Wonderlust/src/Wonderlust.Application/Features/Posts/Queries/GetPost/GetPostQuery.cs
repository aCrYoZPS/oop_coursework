using MediatR;
using Wonderlust.Application.Features.Posts.Dtos;

namespace Wonderlust.Application.Features.Posts.Queries.GetPost;

public record GetPostQuery(Guid PostId) : IRequest<PostDto>;