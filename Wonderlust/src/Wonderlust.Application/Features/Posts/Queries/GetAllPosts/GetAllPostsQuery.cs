using MediatR;
using Wonderlust.Application.Features.Posts.Dtos;

namespace Wonderlust.Application.Features.Posts.Queries.GetAllPosts;

public record GetAllPostsQuery(Guid CommunityId) : IRequest<IEnumerable<PostDto>>;