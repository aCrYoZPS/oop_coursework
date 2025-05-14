using AutoMapper;
using MediatR;
using Wonderlust.Application.Features.Communities.Dtos;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Communities.Commands.CreateCommunity;

public class CreateCommunityCommandHandler(IMapper mapper, ICommunityRepository communityRepository)
    : IRequestHandler<CreateCommunityCommand, CommunityDto>
{
    public async Task<CommunityDto> Handle(CreateCommunityCommand request, CancellationToken cancellationToken)
    {
        var community = mapper.Map<Community>(request);
        await communityRepository.AddAsync(community);
        return mapper.Map<CommunityDto>(community);
    }
}