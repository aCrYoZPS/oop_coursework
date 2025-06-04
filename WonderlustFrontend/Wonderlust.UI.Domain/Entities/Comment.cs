namespace Wonderlust.UI.Domain.Entities;

public class Comment
{
    public Comment() { }

    public Comment(Guid id, string content, Guid authorId, Guid postId, Guid? parentCommentId)
    {
        Id = id;
        Content = content;
        AuthorId = authorId;
        PostId = postId;
        ParentCommentId = parentCommentId;
    }

    public Guid Id { get; set; }

    public string Content { get; set; }
    public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastUpdateDate { get; set; } = DateTimeOffset.UtcNow;
    public bool Deleted { get; set; } = false;
    public Guid AuthorId { get; set; }
    public Guid PostId { get; set; }
    public Guid? ParentCommentId { get; set; }
    public List<Comment> Replies { get; set; } = new List<Comment>();
}