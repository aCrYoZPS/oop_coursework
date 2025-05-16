using AutoMapper;
using MediatR;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Communities.Dtos;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Subscriptions.Queries.GetSubscriptions;

public class GetSubscriptionsQueryHandler(
    IMapper mapper,
    ICommunityRepository communityRepository,
    IUserRepository userRepository,
    ISubscriptionRepository subscriptionRepository) : IRequestHandler<GetSubscriptionsQuery, IEnumerable<CommunityDto>>
{
    public async Task<IEnumerable<CommunityDto>> Handle(GetSubscriptionsQuery request,
        CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetByIdAsync(request.UserId);

        if (existingUser == null)
        {
            throw new NotFoundException($"The user with id {request.UserId} not found");
        }

        var subscriptions = await subscriptionRepository.GetAllByUserAsync(existingUser.Id);

        var userTasks = subscriptions
            .Select(subscription => communityRepository.GetByIdAsync(subscription.CommunityId))
            .ToList();
        var users = await Task.WhenAll(userTasks);
        var subscribers = users.Select(mapper.Map<CommunityDto>).ToList();

        return subscribers.Where(subscriber => subscriber != null);
    }
}