namespace Wonderlust.Domain.Entities;

public class CommentNotification : Notification
{
    public Guid CommentId { get; private set; }
    public Comment Comment { get; private set; }

    private CommentNotification() { }

    public CommentNotification(Guid userId, Guid commentId)
    {
        Id = Guid.NewGuid();
        UserId = userId;

        CommentId = commentId;
    }
}