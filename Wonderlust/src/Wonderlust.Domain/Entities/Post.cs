namespace Wonderlust.Domain.Entities;

public class Post
{
    public Guid Id { get; private set; }

    public string Title { get; private set; }
    public string? ImageId { get; private set; }
    public string? Content { get; private set; }
    public DateTimeOffset CreationDate { get; private set; }
    public DateTimeOffset LastUpdateDate { get; private set; }

    public Guid CommunityId { get; private set; }
    public Community Community { get; private set; }

    public Guid AuthorId { get; private set; }
    public User Author { get; private set; }

    public ICollection<PostReaction> Reactions { get; private set; } = new List<PostReaction>();

    private Post() { }

    public Post(string title, string? imageId, string? content, Guid communityId, Guid authorId)
    {
        Id = Guid.NewGuid();

        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentException("Post title cannot be empty.", nameof(title));
        }

        Title = title;
        ImageId = imageId;
        Content = content;
        LastUpdateDate = CreationDate = DateTimeOffset.UtcNow;

        CommunityId = communityId;
        AuthorId = authorId;
    }

    public void UpdateLastUpdateDate(DateTimeOffset lastUpdateDate)
    {
        if (lastUpdateDate < CreationDate)
        {
            throw new ArgumentException("Update date cannot be earlier than creation date", nameof(lastUpdateDate));
        }

        LastUpdateDate = lastUpdateDate;
    }
}