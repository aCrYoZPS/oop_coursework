using Wonderlust.Domain.Enums;

namespace Wonderlust.Domain.Entities;

public class PostReaction : Reaction
{
    public Guid PostId { get; private set; }
    public Post Post { get; private set; }

    private PostReaction() { }

    public PostReaction(ReactionType reactionType, Guid userId, Guid postId)
    {
        Id = Guid.NewGuid();
        ReactionType = reactionType;
        UserId = userId;

        PostId = postId;
    }
}