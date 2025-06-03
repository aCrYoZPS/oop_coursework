using Wonderlust.UI.Domain.Entities;

namespace Wonderlust.UI.Application.Services.Subscriptions;

public class SubscriptionService(HttpClient httpClient) : ISubscriptionService
{
    private static readonly List<Subscription> subscriptions =
    [
        new Subscription(Guid.Parse("a98e1225-3916-4c79-9775-7d9a737c5027"),
            Guid.Parse("874325f5-3994-4c5f-a81e-4f9a836b689a"))
    ];

    public async Task<IEnumerable<Subscription>> GetSubscriptionsAsync(Guid userId)
    {
        return subscriptions;
    }

    public async Task SubscribeAsync(Guid communityId, Guid userId)
    {
        var existing = subscriptions.FirstOrDefault(s => s.UserId == userId && s.CommunityId == communityId);
        if (existing != null)
        {
            return;
        }

        subscriptions.Add(new Subscription(communityId, userId));
    }

    public async Task UnsubscribeAsync(Guid communityId, Guid userId)
    {
        var existing = subscriptions.FirstOrDefault(s => s.UserId == userId && s.CommunityId == communityId);
        if (existing != null)
        {
            subscriptions.Remove(existing);
        }
    }

    public async Task<bool> IsSubscribed(Guid communityId, Guid userId)
    {
        var existing = subscriptions.FirstOrDefault(s => s.UserId == userId && s.CommunityId == communityId);
        return existing != null;
    }
}