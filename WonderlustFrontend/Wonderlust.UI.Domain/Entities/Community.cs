namespace Wonderlust.UI.Domain.Entities;

public class Community
{
    public Guid Id { get; private set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public int SubscriberCount { get; set; }

    public Guid CreatorId { get; private set; }

    public Community() { }

    public Community(string name, string description, Guid creatorId)
    {
        Name = name;
        Description = description;
        CreationDate = DateTimeOffset.UtcNow;
        CreatorId = creatorId;
        SubscriberCount = 0;
    }
}