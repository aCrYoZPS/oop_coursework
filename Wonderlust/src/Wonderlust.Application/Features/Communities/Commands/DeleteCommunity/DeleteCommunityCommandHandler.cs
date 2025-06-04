using AutoMapper;
using MediatR;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Communities.Commands.DeleteCommunity;

public class DeleteCommunityCommandHandler(
    ICommunityRepository communityRepository,
    IUserRepository userRepository,
    ISubscriptionRepository subscriptionRepository)
    : IRequestHandler<DeleteCommunityCommand>
{
    public async Task Handle(DeleteCommunityCommand request, CancellationToken cancellationToken)
    {
        var existingCommunity = await communityRepository.GetByIdAsync(request.CommunityId);
        if (existingCommunity == null)
        {
            return;
        }

        var user = await userRepository.GetByIdAsync(existingCommunity.CreatorId);
        if (user!.Id != request.SenderId)
        {
            throw new UnauthorizedAccessException(
                $"The user with id {request.SenderId} is not authorized to delete the community with id {request.CommunityId}"
            );
        }

        await communityRepository.DeleteAsync(existingCommunity.Id);
        await subscriptionRepository.DeleteByCommunityAsync(existingCommunity.Id);
    }
}