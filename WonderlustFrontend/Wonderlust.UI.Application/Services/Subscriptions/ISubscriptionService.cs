using Wonderlust.UI.Domain.Entities;

namespace Wonderlust.UI.Application.Services.Subscriptions;

public interface ISubscriptionService
{
    Task<IEnumerable<Subscription>> GetSubscriptionsAsync(Guid userId);
    Task<IEnumerable<Subscription>> GetCommunitySubscriptionsAsync(Guid communityId);
    Task SubscribeAsync(Guid communityId, Guid userId);
    Task UnsubscribeAsync(Guid communityId, Guid userId);
    Task<bool> IsSubscribed(Guid communityId, Guid userId);
}