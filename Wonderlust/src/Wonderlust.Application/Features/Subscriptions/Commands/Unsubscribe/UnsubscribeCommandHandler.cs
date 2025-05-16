using AutoMapper;
using MediatR;
using Wonderlust.Domain.Repositories;
using Wonderlust.Application.Exceptions;

namespace Wonderlust.Application.Features.Subscriptions.Commands.Unsubscribe;

public class UnsubscribeCommandHandler(
        ISubscriptionRepository subscriptionRepository,
        ICommunityRepository communityRepository,
        IUserRepository userRepository
    ) : IRequestHandler<UnsubscribeCommand>
{
    public async Task Handle(UnsubscribeCommand request, CancellationToken cancellationToken)
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
        var subscription = await subscriptionRepository
            .GetByUserAndCommunityAsync(request.UserId, request.CommunityId);

        if (subscription == null)
        {
            return;
        }

        await subscriptionRepository.DeleteAsync(subscription.Id);
    }
}
