namespace Wonderlust.Domain.Entities;

public class Subscription
{
    public Guid Id { get; private set; }

    public DateTimeOffset SubscriptionDate { get; private set; }
    public bool EnableNotifications { get; set; } = true;

    public Guid UserId { get; private set; }
    public User User { get; private set; }

    public Guid CommunityId { get; private set; }
    public Community Community { get; private set; }

    private Subscription() { }

    public Subscription(Guid userId, Guid communityId, bool enableNotifications)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CommunityId = communityId;
        EnableNotifications = enableNotifications;
        SubscriptionDate = DateTimeOffset.UtcNow;
    }
}