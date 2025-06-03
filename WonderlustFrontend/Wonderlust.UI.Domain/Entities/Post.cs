namespace Wonderlust.UI.Domain.Entities;

public class Post
{
    public Guid Id { get; private set; }

    public string Title { get; set; }
    public string? Content { get; set; }
    public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastUpdateDate { get; set; } = DateTimeOffset.UtcNow;
    public int LikeCount { get; set; } = 0;

    public Guid CommunityId { get; private set; }

    public Guid AuthorId { get; private set; }

    public Post() { }

    public Post(string title, string content, Guid communityId, Guid authorId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Content = content;
        CommunityId = communityId;
        AuthorId = authorId;
    }
}