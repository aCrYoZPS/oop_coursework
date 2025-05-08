namespace Wonderlust.Domain.Entities;

public class Notification
{
    public Guid Id { get; protected set; }

    public Guid UserId { get; protected set; }
    public User User { get; protected set; }
}