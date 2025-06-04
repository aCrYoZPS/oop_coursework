using AutoMapper;
using MediatR;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Communities.Dtos;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Communities.Commands.UpdateCommunity;

public class UpdateCommunityCommandHandler(
    IMapper mapper,
    ICommunityRepository communityRepository,
    IModeratorRepository moderatorRepository)
    : IRequestHandler<UpdateCommunityCommand, CommunityDto>
{
    public async Task<CommunityDto> Handle(UpdateCommunityCommand request, CancellationToken cancellationToken)
    {
        var updated = false;
        var existingCommunity = await communityRepository.GetByIdAsync(request.CommunityId);
        if (existingCommunity == null)
        {
            throw new NotFoundException($"Community with id {request.CommunityId} not found");
        }

        var moderators = await moderatorRepository.GetByCommunityAsync(request.CommunityId);
        if (existingCommunity.CreatorId == request.SenderId ||
            moderators.FirstOrDefault(m => m.Id == request.SenderId) != null)
        {
            if (request.Name != null)
            {
                existingCommunity.UpdateName(request.Name);
                updated = true;
            }

            if (request.Description != null)
            {
                existingCommunity.UpdateDescription(request.Description);
                updated = true;
            }

            if (updated)
            {
                await communityRepository.UpdateAsync(existingCommunity);
            }

            return mapper.Map<CommunityDto>(existingCommunity);
        }

        throw new UnauthorizedAccessException(
            $"The user with id {request.SenderId} is not authorized to delete the community with id {request.CommunityId}"
        );
    }
}