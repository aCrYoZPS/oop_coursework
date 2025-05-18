namespace Wonderlust.Domain.Entities;

public class Post
{
    public Guid Id { get; private set; }

    public string Title { get; private set; }
    public Guid? ImageId { get; private set; }
    public string? Content { get; private set; }
    public DateTimeOffset CreationDate { get; private set; }
    public DateTimeOffset LastUpdateDate { get; private set; }

    public Guid CommunityId { get; private set; }

    public Guid AuthorId { get; private set; }

    private Post() { }

    public Post(string title, Guid? imageId, string? content, Guid communityId, Guid authorId)
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

    public void UpdateContent(string newContent)
    {
        Content = newContent;
    }

    public void UpdateTitle(string newTitle)
    {
        Title = newTitle;
    }

    public void UpdateImage(Guid? newImageId)
    {
        ImageId = newImageId;
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