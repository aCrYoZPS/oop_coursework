using System.ComponentModel.DataAnnotations.Schema;

namespace Wonderlust.Domain.Entities;

public class Community
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTimeOffset CreationDate { get; private set; }

    public Guid CreatorId { get; private set; }
    public User Creator { get; private set; }

    public virtual ICollection<Subscription> Subscriptions { get; private set; } = new List<Subscription>();

    public virtual ICollection<Moderator> Moderators { get; private set; } = new List<Moderator>();

    private Community() { }

    public Community(string name, string description, Guid creatorId)
    {
        Id = Guid.NewGuid();
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Community name cannot be empty.", nameof(name));
        }

        Name = name;
        Description = description;
        CreatorId = creatorId;
        CreationDate = DateTimeOffset.UtcNow;
    }
}