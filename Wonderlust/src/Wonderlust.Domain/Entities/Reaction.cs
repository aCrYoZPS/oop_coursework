using Wonderlust.Domain.Enums;

namespace Wonderlust.Domain.Entities;

public class Reaction
{
    public Guid Id { get; protected set; }

    public ReactionType ReactionType { get; protected set; }

    public Guid UserId { get; protected set; }
    public User User { get; protected set; }
}