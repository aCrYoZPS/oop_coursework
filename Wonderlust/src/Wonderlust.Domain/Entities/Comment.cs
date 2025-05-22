namespace Wonderlust.Domain.Entities;

public class Comment
{
    public Guid Id { get; private set; }

    public string Content { get; private set; }
    public DateTimeOffset CreationDate { get; private set; }
    public DateTimeOffset LastUpdateDate { get; private set; }
    public bool Deleted { get; private set; } = false;

    public Guid AuthorId { get; private set; }

    public Guid PostId { get; private set; }

    public Guid? ParentCommentId { get; private set; }

    private Comment() { }

    public void SetDeleted()
    {
        Content = "Deleted";
        Deleted = true;
        SetLastUpdateDate(DateTimeOffset.UtcNow);
    }

    public void SetLastUpdateDate(DateTimeOffset dto)
    {
        LastUpdateDate = dto;
    }

    public void UpdateContent(string newContent)
    {
        if (string.IsNullOrEmpty(newContent))
        {
            throw new ArgumentException("Comment content cannot be empty.", nameof(newContent));
        }

        Content = newContent;
        SetLastUpdateDate(DateTimeOffset.UtcNow);
    }

    public Comment(string content, Guid authorId, Guid postId, Guid? parentCommentId)
    {
        Id = Guid.NewGuid();

        if (string.IsNullOrEmpty(content))
        {
            throw new ArgumentException("Comment content cannot be empty.", nameof(content));
        }

        Content = content;

        LastUpdateDate = CreationDate = DateTimeOffset.UtcNow;

        AuthorId = authorId;
        PostId = postId;
        ParentCommentId = parentCommentId;
    }
}