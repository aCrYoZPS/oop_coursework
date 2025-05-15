using AutoMapper;
using MediatR;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Communities.Dtos;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Communities.Queries.GetCommunity;

public class GetUserQueryHandler(ICommunityRepository repository, IMapper mapper)
    : IRequestHandler<GetCommunityQuery, CommunityDto>
{
    public async Task<CommunityDto> Handle(GetCommunityQuery request, CancellationToken cancellationToken)
    {
        var community = await repository.GetByIdAsync(request.CommunityId);

        if (community == null)
        {
            throw new NotFoundException($"Community with id {request.CommunityId} not found");
        }

        return mapper.Map<CommunityDto>(community);
    }
}
