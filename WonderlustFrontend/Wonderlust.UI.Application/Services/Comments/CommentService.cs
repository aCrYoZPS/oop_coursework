using Wonderlust.UI.Domain.Entities;

namespace Wonderlust.UI.Application.Services.Comments;

public class CommentService(HttpClient client) : ICommentService
{
    private static List<Comment> comments =
    [
        new Comment("cntnt", Guid.NewGuid(), Guid.NewGuid(), null)
        {
            Replies =
            [
                new Comment("Reply1", Guid.NewGuid(), Guid.NewGuid(), null)
                {
                    Replies =
                    [
                        new Comment("InnerReply", Guid.NewGuid(), Guid.NewGuid(), null),
                    ]
                },
                new Comment("Reply2", Guid.NewGuid(), Guid.NewGuid(), null),
                new Comment("Reply3", Guid.NewGuid(), Guid.NewGuid(), null),
            ]
        },
        new Comment("cntnt", Guid.NewGuid(), Guid.NewGuid(), null),
        new Comment("cntnt", Guid.NewGuid(), Guid.NewGuid(), null),
        new Comment("cntnt", Guid.NewGuid(), Guid.NewGuid(), null),
        new Comment("cntnt", Guid.NewGuid(), Guid.NewGuid(), null),
    ];

    public async Task<IEnumerable<Comment>> GetComments(Guid postId)
    {
        return comments;
    }

    public async Task<Comment> AddCommentAsync(Comment comment)
    {
        if (comment.ParentCommentId == null)
        {
            comments.Insert(0, comment);
        }
        else
        {
            var parentComment = comments.First(c => c.Id == comment.ParentCommentId);
            parentComment.Replies.Insert(0, comment);
        }

        return comment;
    }

    public async Task<Comment> UpdateCommentAsync(Comment comment)
    {
        var existingComment = comments.FirstOrDefault(c => c.Id == comment.Id);
        if (existingComment != null)
        {
            comments[comments.IndexOf(existingComment)] = comment;
        }

        return comment;
    }

    public async Task DeleteCommentAsync(Guid commentId)
    {
        var existingComment = comments.FirstOrDefault(c => c.Id == commentId);
        if (existingComment != null)
        {
            existingComment.Content = "Deleted";
        }
    }
}