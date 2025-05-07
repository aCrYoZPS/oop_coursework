namespace Wonderlust.Domain.Entities;

public class Moderator
{
    public Guid Id { get; private set; }
    public DateTimeOffset GrantedDate { get; private set; } = DateTimeOffset.UtcNow;

    public Guid UserId { get; private set; }
    public User User { get; private set; }

    public Guid CommunityId { get; private set; }
    public Community Community { get; private set; }

    private Moderator() { }

    public Moderator(Guid communityId, Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CommunityId = communityId;
    }

}
