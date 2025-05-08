using Wonderlust.Domain.Enums;

namespace Wonderlust.Domain.Entities;

public class CommentReaction : Reaction
{
    public Guid CommentId { get; private set; }
    public Post Comment { get; private set; }

    private CommentReaction() { }

    public CommentReaction(ReactionType reactionType, Guid userId, Guid commentId)
    {
        Id = Guid.NewGuid();
        ReactionType = reactionType;
        UserId = userId;

        CommentId = commentId;
    }
}