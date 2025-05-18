using AutoMapper;
using MediatR;
using Wonderlust.Application.Exceptions;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Moderators.Commands.AssignModerator;

public class AssignModeratorCommandHandler(
    IMapper mapper,
    IModeratorRepository moderatorRepository,
    IUserRepository userRepository,
    ICommunityRepository communityRepository)
    : IRequestHandler<AssignModeratorCommand>
{
    public async Task Handle(AssignModeratorCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetByIdAsync(request.UserId);
        var existingCommunity = await communityRepository.GetByIdAsync(request.CommunityId);

        if (existingUser == null)
        {
            throw new NotFoundException($"User with id {request.UserId} not found.");
        }

        if (existingCommunity == null)
        {
            throw new NotFoundException($"Community with id {request.CommunityId} not found.");
        }

        if (existingCommunity.CreatorId != request.SenderId)
        {
            throw new UnauthorizedAccessException($"Only creator of community can assign moderators");
        }

        var existingModerators = await moderatorRepository.GetByCommunityAsync(existingCommunity.Id);
        var existingModerator = existingModerators.FirstOrDefault(mod =>
            mod.UserId == existingUser.Id && mod.CommunityId == existingCommunity.Id);

        if (existingModerator != null)
        {
            return;
        }

        var moderator = mapper.Map<Moderator>(request);
        await moderatorRepository.AddAsync(moderator);
    }
}