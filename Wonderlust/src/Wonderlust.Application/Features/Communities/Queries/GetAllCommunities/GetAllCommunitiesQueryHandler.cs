using AutoMapper;
using MediatR;
using Wonderlust.Application.Features.Communities.Dtos;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Communities.Queries.GetAllCommunities;

public class GetAllCommunitiesQueryHandler(IMapper mapper, ICommunityRepository communityRepository)
    : IRequestHandler<GetAllCommunitiesQuery, IEnumerable<CommunityDto>>
{
    public async Task<IEnumerable<CommunityDto>> Handle(GetAllCommunitiesQuery request,
        CancellationToken cancellationToken)
    {
        var communities = await communityRepository.GetAllAsync();
        var communityDtos = communities.Select(mapper.Map<CommunityDto>).ToList();
        return communityDtos.Where(community => community != null);
    }
}