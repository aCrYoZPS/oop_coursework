namespace Wonderlust.Domain.Entities;

public class PostNotification : Notification
{
    public Guid PostId { get; private set; }
    public Post Post { get; private set; }

    private PostNotification() { }

    public PostNotification(Guid userId, Guid postId)
    {
        Id = Guid.NewGuid();
        UserId = userId;

        PostId = postId;
    }
}