namespace Wonderlust.Domain.Entities;

public class Comment
{
    public Guid Id { get; private set; }

    public string Content { get; private set; }
    public DateTimeOffset CreationDate { get; private set; }
    public DateTimeOffset LastUpdateDate { get; private set; }

    public Guid AuthorId { get; private set; }

    public Guid PostId { get; private set; }

    public Guid ParentCommentId { get; private set; }

    private Comment() { }

    public Comment(string content, Guid authorId, Guid postId, Guid parentCommentId)
    {
        Id = Guid.NewGuid();
        Content = content;
        LastUpdateDate = CreationDate = DateTimeOffset.UtcNow;

        AuthorId = authorId;
        PostId = postId;
        ParentCommentId = parentCommentId;
    }
}