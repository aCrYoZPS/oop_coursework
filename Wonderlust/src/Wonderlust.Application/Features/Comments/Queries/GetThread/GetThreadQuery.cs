namespace Wonderlust.Application.Features.Comments.Queries.GetThread;

public class GetThreadQuery(Guid parentCommentId)
{
    public Guid ParentCommentId { get; set; } = parentCommentId;
    public int RepliesLimit { get; set; } = 3;
}
