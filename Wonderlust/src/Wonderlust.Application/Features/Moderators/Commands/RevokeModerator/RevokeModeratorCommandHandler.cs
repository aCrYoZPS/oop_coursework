using MediatR;
using Wonderlust.Application.Exceptions;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Moderators.Commands.RevokeModerator;

public class RevokeModeratorCommandHandler(
    IModeratorRepository moderatorRepository,
    IUserRepository userRepository,
    ICommunityRepository communityRepository
) : IRequestHandler<RevokeModeratorCommand>
{
    public async Task Handle(RevokeModeratorCommand request, CancellationToken cancellationToken)
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

        var sender = await userRepository.GetByIdAsync(request.SenderId);
        if (existingCommunity.CreatorId != sender!.Id)
        {
            throw new UnauthorizedAccessException($"Only creator can revoke moderator rights.");
        }

        var moderator = await moderatorRepository
            .GetByUserAndCommunityAsync(request.UserId, request.CommunityId);

        if (moderator == null)
        {
            return;
        }

        await moderatorRepository.DeleteAsync(moderator.Id);
    }
}