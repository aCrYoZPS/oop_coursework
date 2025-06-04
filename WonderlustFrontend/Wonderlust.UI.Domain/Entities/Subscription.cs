namespace Wonderlust.UI.Domain.Entities;

public class Subscription
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid CommunityId { get; private set; }
    public DateTimeOffset SubscriptionDate { get; private set; }

    public Subscription() { }

    public Subscription(Guid communityId, Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CommunityId = communityId;
        SubscriptionDate = DateTimeOffset.UtcNow;
    }
}