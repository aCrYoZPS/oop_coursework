using AutoMapper;
using MediatR;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Users.Dtos;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Subscriptions.Queries.GetSubscribers;

public class GetSubscribersQueryHandler(
    IMapper mapper,
    ICommunityRepository communityRepository,
    IUserRepository userRepository,
    ISubscriptionRepository subscriptionRepository) : IRequestHandler<GetSubscribersQuery, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> Handle(GetSubscribersQuery request, CancellationToken cancellationToken)
    {
        var existingCommunity = await communityRepository.GetByIdAsync(request.CommunityId);

        if (existingCommunity == null)
        {
            throw new NotFoundException($"The community with id {request.CommunityId} not found");
        }

        var subscriptions = await subscriptionRepository.GetAllByCommunityAsync(existingCommunity.Id);

        var userTasks = subscriptions
            .Select(subscription => userRepository.GetByIdAsync(subscription.UserId))
            .ToList();
        var users = await Task.WhenAll(userTasks);
        var subscribers = users.Select(mapper.Map<UserDto>).ToList();

        return subscribers.Where(dto => dto != null);
    }
}