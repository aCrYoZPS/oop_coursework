using Wonderlust.UI.Domain.Entities;

namespace Wonderlust.UI.Application.Services.Comments;

public interface ICommentService
{
    Task<IEnumerable<Comment>> GetComments(Guid communityId, Guid postId);
    Task<Comment> AddCommentAsync(Guid communityId, Comment comment);
    Task<Comment> UpdateCommentAsync(Guid communityId, Comment comment);
    Task DeleteCommentAsync(Guid communityId, Guid postId, Guid commentId);
}