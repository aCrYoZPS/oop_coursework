namespace Wonderlust.Domain.Entities;

public class Community
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTimeOffset CreationDate { get; private set; }

    public Guid CreatorId { get; private set; }

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

    public void UpdateName(string newName)
    {
        if (string.IsNullOrEmpty(newName))
        {
            throw new ArgumentException("Community name cannot be empty.", nameof(newName));
        }

        Name = newName;
    }

    public void UpdateDescription(string newDescription)
    {
        Description = newDescription;
    }
}