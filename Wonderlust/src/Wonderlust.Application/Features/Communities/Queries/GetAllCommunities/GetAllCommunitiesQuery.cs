using MediatR;
using Wonderlust.Application.Features.Communities.Dtos;

namespace Wonderlust.Application.Features.Communities.Queries.GetAllCommunities;

public record GetAllCommunitiesQuery() : IRequest<IEnumerable<CommunityDto>>;
