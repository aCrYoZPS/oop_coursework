using AutoMapper;
using MediatR;
using Wonderlust.Application.Exceptions;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Subscriptions.Commands.Subscribe;

public class SubscribeCommandHandler(
    IUserRepository userRepository,
    ICommunityRepository communityRepository,
    ISubscriptionRepository subscriptionRepository
)
    : IRequestHandler<SubscribeCommand>
{
    public async Task Handle(SubscribeCommand request, CancellationToken cancellationToken)
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

        if (await subscriptionRepository.GetByUserAndCommunityAsync(request.UserId, request.CommunityId) != null)
        {
            return;
        }

        await subscriptionRepository.AddAsync(new Subscription(existingUser.Id, existingCommunity.Id, false));
    }
}